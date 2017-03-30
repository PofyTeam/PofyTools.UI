using UnityEngine;
using System.Collections;

namespace PofyTools
{
    public class PopUp : GameActor
    {

        public bool onStart = false;
        public AnimationCurve curve;
        public float duration;
        private float _timer;
        public bool repeat;
        private Transform _selfTransform = null;

        #region Initialize

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                this._selfTransform = this.transform;
                return true;
            }
            return false;
        }

        #endregion

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            if (this.onStart)
            {
                EnterPopUpState();
            }
        }

        public void EnterPopUpState()
        {
            this._timer = this.duration;
            AddState(this.PopUpState);
        }

        void PopUpState()
        {
            this._timer -= Time.unscaledDeltaTime;
            if (this._timer <= 0)
                this._timer = 0;

            float normalizedTime = 1 - (this._timer / this.duration);
            float scaleFactor = curve.Evaluate(normalizedTime);
            this._selfTransform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
		
            if (this._timer <= 0)
                ExitPopUpState();
        }

        void ExitPopUpState()
        {

            if (repeat)
            {
                EnterPopUpState();
            }
            else
            {
                RemoveAllStates();
            }
        }
    }
}