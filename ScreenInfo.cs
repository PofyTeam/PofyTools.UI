
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;
using PofyTools.Pool;

namespace UnityEngine.UI
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

        protected override void Awake ()
        {
            base.Awake ();
            this.removeAllStatesOnStart = false;
        }

        #region IPoolable implementation

        public void Free ()
        {
            RemoveAllStates (false);
            this._pool.Free (this);
        }

        public void ResetFromPool ()
        {
            this.gameObject.SetActive (true);
            AddState (this.flowState);
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

        public override void ConstructAvailableStates ()
        {
            this.flowState = new InfoFlowState (this);
        }

        public override void InitializeStateStack ()
        {
            this._stateStack = new List<IState> (1);
        }

        #endregion
    }

    public class InfoFlowState : StateObject<ScreenInfo>
    {
        private float _currentDistance;
        private float _timer;
        private float countMultiplier;

        public InfoFlowState ()
        {
            InitializeState ();
        }

        public InfoFlowState (ScreenInfo co)
        {
            this.controlledObject = co;
            InitializeState ();
        }

        public override void InitializeState ()
        {
            this.hasUpdate = true;
        }

        public override void EnterState ()
        {
            if (this.controlledObject.target != null)
                this.controlledObject.selfTransform.position = this.controlledObject.target.transform.position + this.controlledObject.offset;
            else
                this.controlledObject.selfTransform.position = this.controlledObject.offset;

            this._timer = this.controlledObject.duration;
            this.controlledObject.selfTransform.localRotation = Quaternion.Euler (Vector3.forward * Random.Range (-30, 30));
            this.controlledObject.selfTransform.Translate (this.controlledObject.speed * Vector3.up, Space.Self);
            this.countMultiplier = this.controlledObject.pool.Count * 0.33f;

        }

        public override bool UpdateState ()
        {
            this._timer -= Time.smoothDeltaTime;
            if (this._timer < 0)
                this._timer = 0;
            float normalizedTime = this.controlledObject.alphaCurve.Evaluate (1 - this._timer / this.controlledObject.duration);
            this.controlledObject.canvasGroup.alpha = normalizedTime;
            this.controlledObject.selfTransform.localScale = normalizedTime * Vector3.one * this.countMultiplier;
            this.controlledObject.selfTransform.Translate (this.controlledObject.speed * Time.smoothDeltaTime * Vector3.up, Space.Self);
            if (this._timer <= 0)
                return true;
            return false;
        }

        public override void ExitState ()
        {
            this.controlledObject.Free ();
        }

    }
}