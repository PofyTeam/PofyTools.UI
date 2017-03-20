/// <summary>
/// Asyncronly loads next level and unloads the previous.
/// </summary>
using System.Collections;


namespace PofyTools.LevelLoader
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    public class LevelLoader : Panel
    {
        #region Variables

        //Script logging tag
        public const string TAG = "<color=yellow><b>LevelLoader:</color><b> ";

        //Persistant singleton instance
        private static LevelLoader _Loader;

        //List of tips to display on loading
        public List<string> tips;
        
        public RectTransform tipPanel;
        public Text progress;
        public Text tip;
        private int offset = 0;
        bool isTargerScene;
        public string toLoad;
        bool _shouldClose = false;

        AsyncOperation asyncOperation = null;

        #endregion

        #region Mono

        void Awake()
        {
            if (_Loader == null)
                _Loader = this;
            else if (_Loader != this)
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        protected override void Start()
        {
            base.Start();

            #if !UNITY_5_3
			SceneManager.sceneLoaded += this.OnSceneLoaded;
            #endif

            this.tipPanel.gameObject.SetActive(false);
            LoadCustomScene(this.toLoad);
        }

        #endregion

        #if !UNITY_5_3
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (this._shouldClose)
            {
                this._shouldClose = false;
            }
        }


#else
        void OnLevelWasLoaded()
        {
            if (this._shouldClose)
            {
                this._shouldClose = false;
                this.Close();
            }
        }
        #endif

        #region API

        public static void LoadLevel()
        {
            LoadCustomScene("Game");
        }

        public static void LoadMenu()
        {
            LoadCustomScene("Menu");
        }

        public static void LoadCustomScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning(TAG + "Scene name can not be empty!");
            }

            LevelLoader._Loader.Open();
            Debug.Log(TAG + "Loading " + sceneName);
            LevelLoader._Loader.RefreshTip();
            LevelLoader._Loader.toLoad = sceneName;
            Resources.UnloadUnusedAssets();
            LevelLoader._Loader.LoadLoaderScene();
        }

        public static void SetTips(params string[] tips)
        {
            LevelLoader._Loader.tips.Clear();
            AddTips(tips);
        }

        public static void AddTips(params string[] tips)
        {
            LevelLoader._Loader.tips.AddRange(tips);
            LevelLoader._Loader.tips.RemoveAll(x => string.IsNullOrEmpty(x));
        }

        public static void ShowCustomTip(string msg)
        {
            LevelLoader._Loader.tipPanel.gameObject.SetActive(true);
            LevelLoader._Loader.tip.text = msg;
        }

        #endregion

        void LoadLoaderScene()
        {
            this.progress.text = "0%";
            this.offset = 0;
            this.isTargerScene = false;
            this.asyncOperation = SceneManager.LoadSceneAsync("Load");
            StartCoroutine(this.CheckAsyncOperationProgress());
        }

        void LoadTargetScene()
        {
            this.offset = 50;
            this.isTargerScene = true;

            this.asyncOperation = SceneManager.LoadSceneAsync(toLoad);
            StartCoroutine(this.CheckAsyncOperationProgress());
        }

        void RefreshTip()
        {
            this.tip.text = this.tips[Random.Range(0, this.tips.Count)];
            this.tipPanel.gameObject.SetActive(true);
        }

        #region Coroutines

        IEnumerator CheckAsyncOperationProgress()
        {
            while (!this.asyncOperation.isDone)
            {
                this.progress.text = string.Format("{0}%", (int)(this.asyncOperation.progress * 50) + this.offset);
                yield return null;
            }
            this.asyncOperation = null;
            System.GC.Collect();
            Resources.UnloadUnusedAssets();

            if (!isTargerScene)
            {
                LoadTargetScene();
            }
            else
            {
                this.Close();
            }
        }

        #endregion
    }
}
