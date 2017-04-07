using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PofyTools
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : StateableActor, IToggleable
    {
        #region Variables

        public CanvasGroup canvasGroup;
        protected List<Selectable> _selectebles = new List<Selectable>();
        public bool closeOnSubscribe = false;

        [Header("Fade State")]
        public float fadeDuration = 0.5f;
        protected float _fadeTimer = 0;
        public bool fadeOut = true;
        public bool fadeIn = false;

        #endregion

        #region Mono

        //        protected virtual void OnEnable()
        //        {
        //            EnableElements();
        //        }
        //
        //        protected virtual void OnDisable()
        //        {
        //            DisableElements();
        //        }

        protected virtual void OnTransformChildrenChanged()
        {
            GetComponentsInChildren<Selectable>(true, this._selectebles);
        }

        #endregion

        #region IToggleable implementation

        protected bool _isOpen;

        public bool isOpen
        {
            get { return this._isOpen; }
        }

        public virtual void Toggle()
        {
            if (this._isOpen)
                Close();
            else
                Open();
        }

        public virtual void Open()
        {
            if (!this._isOpen)
            {
                this.gameObject.SetActive(true);
                StopAllCoroutines();
                this.EnableElements();
                this._isOpen = true;

//                if (this.fadeIn)
//                {
//                    StartCoroutine(this.FadeIn());
//                }
//                else
//                {
                    this.canvasGroup.alpha = 1f;
//                }
            }
        }

        public virtual void ForceClose()
        {
            StopAllCoroutines();
            this.DisableElements();
            this._isOpen = false;
            this.canvasGroup.alpha = 0f;
            this.gameObject.SetActive(false);
        }

        public virtual void ForceOpen()
        {
            StopAllCoroutines();
            this.EnableElements();
            this._isOpen = true;
            this.canvasGroup.alpha = 1f;
            this.gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            if (this._isOpen)
            {
                StopAllCoroutines();
                this.DisableElements();
                this._isOpen = false;
//                if (this.fadeOut)
//                {
//                    StartCoroutine(this.FadeOut());
//                }
//                else
//                {
                    this.canvasGroup.alpha = 0f;
                    this.gameObject.SetActive(false);
//                }
            }
        }

        public virtual void EnableElements()
        {
            foreach (var selectable in this._selectebles)
                selectable.interactable = true;
        }

        public virtual void DisableElements()
        {
            foreach (var selectable in this._selectebles)
                selectable.interactable = false;
        }

        #endregion

        #region Initialize

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                if (!this.canvasGroup)
                    this.canvasGroup = GetComponent<CanvasGroup>();
                
                OnTransformChildrenChanged();
                return true;
            }
            return false;
        }

        #endregion

        #region Subscribe

        public override bool Subscribe()
        {
            if (base.Subscribe())
            {
                //
                if (this.closeOnSubscribe)
                {
                    Close();
                }
                return true;
            }
            return false;
        }

        public override bool Unsubscribe()
        {
            if (base.Unsubscribe())
            {
                //
                return true;
            }
            return false;
        }

        #endregion

//        #region Coroutines
//
//        IEnumerator FadeIn()
//        {
//            this._fadeTimer = this.fadeDuration;
//            this.canvasGroup.alpha = 0;
//
//            while (_fadeTimer > 0)
//            {
//                this._fadeTimer -= Time.unscaledDeltaTime;
//                if (this._fadeTimer < 0)
//                    this._fadeTimer = 0;
//                float normalizedTime = 0;
////                if(UIResourceManager.Resources != null)
//                normalizedTime = UIResourceManager.Resources.fadeCurve.Evaluate(1 - this._fadeTimer / this.fadeDuration);
//
//                this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, normalizedTime);
//                yield return null;
//            }
//
//            this.canvasGroup.alpha = 1;
//        }
//
//        IEnumerator FadeOut()
//        {
//            this._fadeTimer = this.fadeDuration;
//            this.canvasGroup.alpha = 1;
//            while (_fadeTimer > 0)
//            {
//                this._fadeTimer -= Time.unscaledDeltaTime;
//                if (this._fadeTimer < 0)
//                    this._fadeTimer = 0;
//
//                float normalizedTime = UIResourceManager.Resources.fadeCurve.Evaluate(1 - this._fadeTimer / this.fadeDuration);
//                this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, normalizedTime);
////                Debug.LogError(this.canvasGroup.alpha);
//
//                yield return null;
//            }
//
//            this.canvasGroup.alpha = 0;
//            this.gameObject.SetActive(false);
//        }
//
//
//        #endregion
    }
}