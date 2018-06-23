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
        [SerializeField]
        protected CanvasGroup _canvasGroup;
        protected List<Selectable> _selectebles = new List<Selectable>();
        [SerializeField]
        protected bool _closeOnSubscribe = false;

        #endregion

        #region Mono

        protected virtual void OnTransformChildrenChanged()
        {
            GetComponentsInChildren<Selectable>(true, this._selectebles);
        }

        #endregion

        #region IToggleable implementation

        protected bool _isOpen;

        public virtual bool IsOpen
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

        public virtual void Toggle(bool on)
        {
            if (on)
                Open();
            else
                Close();
        }

        public virtual void Open()
        {
            //if (!this._isOpen)
            //{
            this.gameObject.SetActive(true);
            this.EnableElements();
            this._canvasGroup.alpha = 1f;

            this._isOpen = true;
            //}
        }

        public virtual void Close()
        {
            //if (this._isOpen)
            //{
            this.DisableElements();
            this._canvasGroup.alpha = 0f;
            this.gameObject.SetActive(false);

            this._isOpen = false;
            //}
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
                if (!this._canvasGroup)
                    this._canvasGroup = GetComponent<CanvasGroup>();

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
                if (this._closeOnSubscribe)
                {
                    this._isOpen = true;
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

        #region API

        public void FadeOut(float duration = 1f)
        {
            //Open();
            this._canvasGroup.alpha = 1f;
            AddState(this._fadeState.Set(Mathf.Abs(duration) * -1));
            this._isOpen = false;
        }

        public void FadeIn(float duration = 1f)
        {
            Open();
            AddState(this._fadeState.Set(Mathf.Abs(duration)));
        }

        #endregion

        #region Callbacks

        protected void FadeStart()
        {
            //Debug.Log (TAG + "FadeStart");
        }

        protected void FadeComplete(int direction)
        {
            if (direction < 0)
                Close();
            //Debug.Log (TAG + "FadeComplete");
        }

        #endregion

        #region IStateable
        protected FadeState _fadeState;

        public override void ConstructAvailableStates()
        {
            base.ConstructAvailableStates();
            this._fadeState = new FadeState(this, 1f);
        }

        public class FadeState : TimedStateObject<Panel>//TODO: Make all panels IFadeable
        {
            private int _direction = 1;
            public FadeState(Panel controlledObject, float duration) : base(controlledObject, duration)
            {
            }
            public override void InitializeState()
            {
                base.InitializeState();

                this.ignoreStacking = true;
                this.isPermanent = false;

            }
            public FadeState Set(float duration)
            {
                //Debug.Log (TAG + "Set " + duration);
                this._direction = (duration > 0) ? 1 : -1;
                this._timeRange.max = Mathf.Abs(duration);
                return this;
            }

            public override void EnterState()
            {
                // Debug.Log (TAG + "EnterState");
                base.EnterState();
                this._timeRange.current = (this._direction > 0) ? this._timeRange.min : this._timeRange.max;
                this.controlledObject.FadeStart();
            }

            public override bool LateUpdateState()
            {
                var deltaTime = Time.unscaledDeltaTime * this._direction;
                //Debug.Log (TAG + "deltaTime is " + deltaTime);

                this._timeRange.current += deltaTime;

                this.controlledObject._canvasGroup.alpha = this._timeRange.CurrentToMaxRatio;

                if (this._direction > 0 && this._timeRange.AtMax || this._direction < 0 && this._timeRange.AtMin)
                    return true;

                return false;
            }

            public override void ExitState()
            {
                this.controlledObject.FadeComplete(this._direction);
                base.ExitState();
            }
        }
        #endregion

    }
}