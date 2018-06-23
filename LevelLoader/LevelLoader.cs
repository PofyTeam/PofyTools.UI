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
        private static LevelLoader _instance;

        //public Text progress;
        private float offset = 0;

        bool isTargerScene;
        public string toLoad;

        bool _shouldClose = false;

        AsyncOperation asyncOperation = null;

        #endregion

        #region Mono

        protected override void Start()
        {
            base.Start();
            //LoadCustomScene (this.toLoad);
        }

        #endregion

        #region ISubscribable

        public override bool Subscribe()
        {
            if (base.Subscribe())
            {
                //SceneManager.sceneLoaded += this.OnSceneLoaded;
                return true;
            }
            return false;
        }

        public override bool Unsubscribe()
        {
            if (base.Unsubscribe())
            {
                //SceneManager.sceneLoaded -= this.OnSceneLoaded;

                RemoveAllLoadProgressListener();
                return true;
            }
            return false;
        }

        #endregion

        #region IInitializable

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                if (_instance == null)
                    _instance = this;
                else if (_instance != this)
                {
                    Destroy(this.gameObject);
                    return false;
                }

                DontDestroyOnLoad(this.gameObject);

                if (!string.IsNullOrEmpty(_loadQueue))
                {
                    LoadCustomScene(_loadQueue);
                    _loadQueue = string.Empty;
                }
                return true;
            }
            return false;
        }

        #endregion

        //#region Listeners
        //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    if (this._shouldClose)
        //    {
        //        this._shouldClose = false;
        //    }
        //}
        //#endregion

        #region Progress Listeners

        protected List<ILoadProgressListener> _listeners = new List<ILoadProgressListener>();

        public static void AddLoadProgressListener(ILoadProgressListener listener)
        {
            _instance._listeners.Add(listener);
        }

        public static void RemoveLoadProgressListener(ILoadProgressListener listener)
        {
            _instance._listeners.Remove(listener);
        }

        public static void RemoveAllLoadProgressListener()
        {
            _instance._listeners.Clear();
        }

        protected void OnProgressChange(float progress)
        {
            for (int i = this._listeners.Count - 1; i >= 0; --i)
            {
                this._listeners[i].OnLoadProgressChange(progress);
            }
        }

        protected void OnLoadStart()
        {
#if UNITY_EDITOR
            Debug.Log(TAG + "OnLoadStart");
#endif
            for (int i = this._listeners.Count - 1; i >= 0; --i)
            {
                this._listeners[i].OnLoadStart();
            }
        }

        protected void OnLoadComplete()
        {
#if UNITY_EDITOR
            Debug.Log(TAG + "OnLoadComplete");
#endif
            for (int i = this._listeners.Count - 1; i >= 0; --i)
            {
                this._listeners[i].OnLoadComplete();
            }
        }

        #endregion

        #region API
        private static string _loadQueue;

        public static void LoadCustomScene(string sceneName)
        {
            if (_instance)
            {
                if (string.IsNullOrEmpty(sceneName))
                {
                    Debug.LogWarning(TAG + "Scene name can not be empty!");
                }

                _instance.Open();
                Debug.Log(TAG + "Loading " + sceneName);
                //            LevelLoader._Loader.RefreshTip();
                _instance.toLoad = sceneName;
                Resources.UnloadUnusedAssets();
                _instance.LoadLoaderScene();

                _instance.OnLoadStart();
            }
            else
            {
                _loadQueue = sceneName;
            }
        }

        #endregion

        private void LoadLoaderScene()
        {
            //this.progress.text = "0%";
            this.offset = 0;
            this.isTargerScene = false;
            this.asyncOperation = SceneManager.LoadSceneAsync("Load");
            StartCoroutine(this.CheckAsyncOperationProgress());
        }

        private void LoadTargetScene()
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
                //this.progress.text = string.Format("{0}%", (int)(progress * 100));
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

                OnLoadComplete();
            }
        }

        #endregion
    }

    public interface ILoadProgressListener : ISubscribable
    {
        void OnLoadProgressChange(float progress);
        void OnLoadComplete();
        void OnLoadStart();
    }
}
