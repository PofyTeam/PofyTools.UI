
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;
using PofyTools.Pool;
using UnityEngine.UI;

namespace PofyTools.UI
{

    public class ScreenInfo : StateableActor, IPoolable<ScreenInfo>
    {
        private Pool<ScreenInfo> _pool;
        public CanvasGroup canvasGroup;
        public Text message;

        public AnimationCurve alphaCurve;
        public bool follow;
        public Vector3 offset;
        public float speed = 1;
        public float duration = 1;
        private Transform _target;

        public Transform target
        {
            get { return this._target; }
            set { this._target = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            this.removeAllStatesOnStart = false;
        }

        #region IPoolable implementation

        public void Free()
        {
            RemoveAllStates(false);
            this._pool.Free(this);
        }

        public void ResetFromPool()
        {
            this.gameObject.SetActive(true);
            AddState(this.flowState);
        }

        public Pool<ScreenInfo> pool
        {
            get
            {
                return this._pool;
            }
            set
            {
                this._pool = value;
            }
        }

        #endregion

        #region IIdentifiable implementation

        public string id
        {
            get
            {
                return this.name;
            }
        }

        #endregion

        #region implemented abstract members of StateableActor

        private InfoFlowState flowState;

        public override void ConstructAvailableStates()
        {
            this.flowState = new InfoFlowState(this);
        }

        public override void InitializeStateStack()
        {
            this._stateStack = new List<IState>(1);
        }

        #endregion
    }

    public class InfoFlowState : StateObject<ScreenInfo>
    {
        private float _currentDistance;
        private float _timer;
        private float countMultiplier;

        public InfoFlowState()
        {
            InitializeState();
        }

        public InfoFlowState(ScreenInfo co)
        {
            this.ControlledObject = co;
            InitializeState();
        }

        public override void InitializeState()
        {
            this.HasUpdate = true;
        }

        public override void EnterState()
        {
            if (this.ControlledObject.target != null)
                this.ControlledObject.SelfTransform.position = this.ControlledObject.target.transform.position + this.ControlledObject.offset;
            else
                this.ControlledObject.SelfTransform.position = this.ControlledObject.offset;

            this._timer = this.ControlledObject.duration;
            this.ControlledObject.SelfTransform.localRotation = Quaternion.Euler(Vector3.forward * Random.Range(-30, 30));
            this.ControlledObject.SelfTransform.Translate(this.ControlledObject.speed * Vector3.up, Space.Self);
            this.countMultiplier = this.ControlledObject.pool.Count * 0.33f;

        }

        public override bool UpdateState()
        {
            this._timer -= Time.smoothDeltaTime;
            if (this._timer < 0)
                this._timer = 0;
            float normalizedTime = this.ControlledObject.alphaCurve.Evaluate(1 - this._timer / this.ControlledObject.duration);
            this.ControlledObject.canvasGroup.alpha = normalizedTime;
            this.ControlledObject.SelfTransform.localScale = normalizedTime * Vector3.one * this.countMultiplier;
            this.ControlledObject.SelfTransform.Translate(this.ControlledObject.speed * Time.smoothDeltaTime * Vector3.up, Space.Self);
            if (this._timer <= 0)
                return true;
            return false;
        }

        public override void ExitState()
        {
            this.ControlledObject.Free();
        }

    }
}