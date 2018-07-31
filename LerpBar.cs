using UnityEngine;
using UnityEngine.UI;

namespace PofyTools.UI
{
    public class LerpBar : Panel
    {

        [SerializeField]
        protected AnimationCurve _lerpCurve;

        [SerializeField]
        protected Image _frame, _fill, _fillLerp;

        private float _currnetValue;

        public float duration = 0.5f;

        #region API

        public void SetLerpValue(float normalizedValue)
        {


            if (normalizedValue < this._currnetValue)
            {
                this._fill.fillAmount = normalizedValue;
                this._currnetValue = normalizedValue;
                LerpDown();
            }
            else
            {
                this._fillLerp.fillAmount = normalizedValue;
                this._currnetValue = normalizedValue;
                LerpUp();
            }
        }

        public void SetValue(float normalizedValue)
        {
            this._currnetValue = normalizedValue;
            this._fill.fillAmount = normalizedValue;
            this._fillLerp.fillAmount = normalizedValue;

        }

        private void LerpDown()
        {
            //if (!this._lerpDownState.isActive)
            AddState(this._lerpDownState);

        }

        private void LerpUp()
        {
            //if (!this._lerpUpState.isActive)
            AddState(this._lerpUpState);
        }

        #endregion

        #region IStateable

        private LerpDownState _lerpDownState;
        private LerpUpState _lerpUpState;

        public override void ConstructAvailableStates()
        {
            base.ConstructAvailableStates();
            this._lerpDownState = new LerpDownState(this, new Range(this.duration), this._lerpCurve);
            this._lerpUpState = new LerpUpState(this, new Range(this.duration), this._lerpCurve);
        }

        public class LerpDownState : TimedStateObject<LerpBar>
        {
            private float _startValue;

            public LerpDownState(LerpBar controlledObject, Range timeRange, AnimationCurve curve) : base(controlledObject, timeRange, curve)
            {

            }

            public override void EnterState()
            {
                base.EnterState();
                this._startValue = this[0]._fillLerp.fillAmount;

                this._timeRange.Current = 0f;
            }

            public override bool LateUpdateState()
            {
                this._timeRange.Current += Time.unscaledDeltaTime;

                var time = this._curve.Evaluate(this._timeRange.CurrentToMaxRatio);
                this[0]._fillLerp.fillAmount = Mathf.Lerp(this._startValue, this[0]._fill.fillAmount, time);

                if (this._timeRange.AtMax)
                    return true;

                return false;
            }
        }

        public class LerpUpState : TimedStateObject<LerpBar>
        {
            private float _startValue;

            public LerpUpState(LerpBar controlledObject, Range timeRange, AnimationCurve curve) : base(controlledObject, timeRange, curve)
            {

            }

            public override void EnterState()
            {
                base.EnterState();
                this._startValue = this[0]._fillLerp.fillAmount;

                this._timeRange.Current = 0f;
            }

            public override bool LateUpdateState()
            {
                this._timeRange.Current += Time.unscaledDeltaTime;

                var time = this._curve.Evaluate(this._timeRange.CurrentToMaxRatio);
                this[0]._fill.fillAmount = Mathf.Lerp(this._startValue, this[0]._fillLerp.fillAmount, time);

                if (this._timeRange.AtMax)
                    return true;

                return false;
            }
        }
        #endregion
    }
}