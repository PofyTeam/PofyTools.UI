
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;
using PofyTools.Pool;
using UnityEngine.UI;
using TMPro;

namespace PofyTools.UI
{

    public class ScreenInfo : StateableActor, IPoolable<ScreenInfo>
    {
        private Pool<ScreenInfo> _pool;
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI message;
        public float size = 1;
        public Color FillColor
        {
            get { return this.message.color; }
            set { this.message.color = value; }
        }

        public Color OutlineColor
        {
            get { return this.message.outlineColor; }
            set { this.message.outlineColor = value; }
        }

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
        public bool IsActive { get { return this.gameObject.activeSelf; } }
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

        public Pool<ScreenInfo> Pool
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
            if (this[0].target != null)
                this[0].SelfTransform.position = this[0].target.transform.position + this[0].offset;
            else
                this[0].SelfTransform.position = this[0].offset;

            this._timer = this[0].duration;
            this[0].SelfTransform.localRotation = Quaternion.Euler(Vector3.forward * Random.Range(-30, 30));
            this[0].SelfTransform.Translate(this[0].speed * Vector3.up, Space.Self);
            //this.countMultiplier = this[0].Pool.Count * 0.33f;

        }

        public override bool UpdateState()
        {
            this._timer -= Time.smoothDeltaTime;
            if (this._timer < 0)
                this._timer = 0;
            float normalizedTime = this[0].alphaCurve.Evaluate(1 - this._timer / this[0].duration);
            this[0].canvasGroup.alpha = normalizedTime;
            this[0].SelfTransform.localScale = normalizedTime * Vector3.one * this[0].size;// * this.countMultiplier;
            this[0].SelfTransform.Translate(this[0].speed * Time.smoothDeltaTime * Vector3.up, Space.Self);
            if (this._timer <= 0)
                return true;
            return false;
        }

        public override void ExitState()
        {
            this[0].Free();
        }

    }
}