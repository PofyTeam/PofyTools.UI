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
        public const string TAG = "<color=yellow><b>LevelLoader:</b></color> ";

        //Persistant singleton instance
        private static LevelLoader _Loader;

        public Text progress;
        private float offset = 0;

        bool isTargerScene;
        public string toLoad;

        bool _shouldClose = false;

        AsyncOperation asyncOperation = null;

        #endregion

        #region Mono

        protected override void Awake()
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

            SceneManager.sceneLoaded += this.OnSceneLoaded;

            LoadCustomScene(this.toLoad);
        }

        protected override void OnDestroy()
        {
            RemoveAllLoadProgressListener();
            base.OnDestroy();
        }

        #endregion

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (this._shouldClose)
            {
                this._shouldClose = false;
            }
        }

        #region Progress Listeners

        protected List<ILoadProgressListener> _listeners = new List<ILoadProgressListener>();

        public static void AddLoadProgressListener(ILoadProgressListener listener)
        {
            _Loader._listeners.Add(listener);
        }

        public static void RemoveLoadProgressListener(ILoadProgressListener listener)
        {
            _Loader._listeners.Remove(listener);
        }

        public static void RemoveAllLoadProgressListener()
        {
            _Loader._listeners.Clear();
        }

        protected void OnProgressChange(float progress)
        {
            for (int i = this._listeners.Count - 1; i >= 0; --i)
            {
                this._listeners[i].OnLoadProgressChange(progress);
            }
        }

        #endregion

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
//            LevelLoader._Loader.RefreshTip();
            LevelLoader._Loader.toLoad = sceneName;
            Resources.UnloadUnusedAssets();
            LevelLoader._Loader.LoadLoaderScene();
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
            this.offset = 0.5F;
            this.isTargerScene = true;

            this.asyncOperation = SceneManager.LoadSceneAsync(toLoad);
            StartCoroutine(this.CheckAsyncOperationProgress());
        }

        #region Coroutines

        IEnumerator CheckAsyncOperationProgress()
        {
            while (!this.asyncOperation.isDone)
            {
                float progress = this.asyncOperation.progress * 0.5f + this.offset;
                this.progress.text = string.Format("{0}%", (int)(progress * 100));
                OnProgressChange(progress);
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

    public interface ILoadProgressListener:ISubscribable
    {
        void OnLoadProgressChange(float progress);
    }
}
