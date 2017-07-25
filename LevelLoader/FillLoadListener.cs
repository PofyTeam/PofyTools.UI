namespace PofyTools.LevelLoader
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class FillLoadListener : MonoBehaviour,ILoadProgressListener,IInitializable
    {
        protected Image _fill;

        #region IInitializable implementation

        public bool Initialize()
        {
            if (!this.isInitialized)
            {
                this._fill = GetComponent<Image>();
                this.isInitialized = true;
                return true;
            }
            return false;
        }


        public bool isInitialized
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

        #endregion

        #region ISubscribable implementation

        public bool Subscribe()
        {
            if (!this.isSubscribed)
            {
                LevelLoader.AddLoadProgressListener(this);
                this.isSubscribed = true;
                return true;
            }
            return false;
        }

        public bool Unsubscribe()
        {
            if (this.isSubscribed)
            {
                LevelLoader.RemoveLoadProgressListener(this);
                this.isSubscribed = false;
                return true;
            }
            return false;
        }

        public bool isSubscribed
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