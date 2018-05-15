using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PofyTools
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : StateableActor, IToggleable, IBackButtonListener
    {
        #region Variables

        public CanvasGroup canvasGroup;
        protected List<Selectable> _selectebles = new List<Selectable>();
        public bool closeOnSubscribe = false;

        #endregion

        #region Mono

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
                this.EnableElements();
                this.canvasGroup.alpha = 1f;
                this._isOpen = true;
            }
        }

        //        public virtual void ForceClose()
        //        {
        //            this.DisableElements();
        //            this._isOpen = false;
        //            this.canvasGroup.alpha = 0f;
        //            this.gameObject.SetActive(false);
        //        }
        //
        //        public virtual void ForceOpen()
        //        {
        //            this.EnableElements();
        //            this._isOpen = true;
        //            this.canvasGroup.alpha = 1f;
        //            this.gameObject.SetActive(true);
        //        }

        public virtual void Close()
        {
            if (this._isOpen)
            {
                this.DisableElements();
                this._isOpen = false;
                this.canvasGroup.alpha = 0f;
                this.gameObject.SetActive(false);
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

        #region IBackButtonListener implementation

        public virtual bool OnBackButton()
        {
            return true;
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
    }
}