/// <summary>
/// Asyncronly loads next level and unloads the previous.
/// </summary>
namespace PofyTools.LevelLoader
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    public class LevelLoader : GameActor
	{
        //Script logging tag
        public const string TAG = "<color=yellow><b>LevelLoader:</color><b> ";

        //Persistant singleton instance
		public static LevelLoader Loader;

        //List of tips to display on loading
		public List<string> tips;
        
		public Panel loadingPanel;
		public RectTransform tipPanel;
		public Text progress;
		public Text tip;
		private int offset = 0;
		bool isTargerScene;
		public string toLoad;

		AsyncOperation asyncOperation = null;


		void Awake ()
		{
			if (Loader == null)
				Loader = this;
			if (Loader != this)
				Destroy (this.gameObject);
			DontDestroyOnLoad (this.gameObject);
		}

		protected override void Start ()
		{
			base.Start ();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
            this.tipPanel.gameObject.SetActive(false);
            LoadCustomScene (this.toLoad);
		}

		public static void SetTips (params string[] tips)
		{
            LevelLoader.Loader.tips.Clear();
            AddTips(tips);
		}

        public static void AddTips(params string[] tips)
        {
            LevelLoader.Loader.tips.AddRange(tips);
            LevelLoader.Loader.tips.RemoveAll(x => string.IsNullOrEmpty(x));
        }

		void ShowCustomTip (string msg)
		{
			this.tipPanel.gameObject.SetActive (true);
			this.tip.text = msg;
		}

		bool _shouldClose = false;

		void OnSceneLoaded (Scene scene, LoadSceneMode mode)
		{
			if (this._shouldClose) {
				this._shouldClose = false;
				this.loadingPanel.Close ();
			}
		}

		static void CreateLoader ()
		{
			if (Loader == null) {
				GameObject go = Resources.Load ("Loader") as GameObject;
				GameObject.Instantiate (go);
			}
		}

		public static void LoadLevel ()
		{
			LoadCustomScene ("Game");
		}

		public static void LoadMenu ()
		{
			LoadCustomScene ("Menu");
		}

		public static void LoadCustomScene (string sceneName)
		{
            Debug.Log(TAG + "Loading " + sceneName);
			Loader.RefreshTip ();
			Loader.toLoad = sceneName;
			Resources.UnloadUnusedAssets ();
			Loader.LoadLoaderScene ();
		}

		void LoadLoaderScene ()
		{
			this.loadingPanel.Open ();
			this.progress.gameObject.SetActive (true);
			this.progress.text = "0%";
			this.offset = 0;
			this.isTargerScene = false;
			this.asyncOperation = SceneManager.LoadSceneAsync ("Load");
			AddState (this.CheckAsyncOperationProgress);
		}

		void LoadTargetScene ()
		{
			this.offset = 50;
			this.isTargerScene = true;
			this.asyncOperation = SceneManager.LoadSceneAsync (toLoad);
			AddState (this.CheckAsyncOperationProgress);
		}

		void RefreshTip ()
		{
			this.tip.text = this.tips [Random.Range (0, this.tips.Count)];
			this.tipPanel.gameObject.SetActive (true);
		}

		void CheckAsyncOperationProgress ()
		{
			this.progress.text = string.Format ("{0}%", (int)(this.asyncOperation.progress * 50) + this.offset);

			if (this.asyncOperation.isDone) {
				this.asyncOperation = null;
				System.GC.Collect ();
				Resources.UnloadUnusedAssets ();
				RemoveState (this.CheckAsyncOperationProgress);

				if (!isTargerScene) {
					LoadTargetScene ();
				} else {
					this.loadingPanel.Close ();
				}
			}
		}
	}
}
