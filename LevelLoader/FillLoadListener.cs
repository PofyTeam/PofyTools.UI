namespace PofyTools.LevelLoader
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class FillLoadListener : MonoBehaviour, ILoadProgressListener, IInitializable
    {
        protected Image _fill;

        #region IInitializable implementation

        public bool Initialize()
        {
            if (!this.IsInitialized)
            {
                this._fill = GetComponent<Image>();
                this.IsInitialized = true;
                return true;
            }
            return false;
        }


        public bool IsInitialized
        {
            get;
            protected set;
        }

        #endregion

        #region ILoadProgressListener implementation

        public void OnLoadProgressChange(float progress)
        {
            this._fill.fillAmount = progress;
        }

        public void OnLoadStart(string levelName) { }
        public void OnLoadComplete(string levelName) { }
        #endregion

        #region ISubscribable implementation

        public bool Subscribe()
        {
            if (!this.IsSubscribed)
            {
                LevelLoader.AddLoadProgressListener(this);
                this.IsSubscribed = true;
                return true;
            }
            return false;
        }

        public bool Unsubscribe()
        {
            if (this.IsSubscribed)
            {
                LevelLoader.RemoveLoadProgressListener(this);
                this.IsSubscribed = false;
                return true;
            }
            return false;
        }

        public bool IsSubscribed
        {
            get;
            protected set;
        }

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Start()
        {
            Subscribe();
        }

        protected virtual void OnDestroy()
        {
            Unsubscribe();
        }

        #endregion
    }
}