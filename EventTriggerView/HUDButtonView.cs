namespace PofyTools.UI
{
	using UnityEngine;
	using System.Collections.Generic;
	using UnityEngine.UI;

	public class HUDButtonView : EventTriggerView
	{
		protected Transform _visual;

		public Transform visual {
			get{ return this._visual; }
		}

		protected Image _icon;

		public Image icon{ get { return this._icon; } }

		#region Mono

		protected override void Awake ()
		{
			base.Awake ();
			this._selfTransform = this.transform;
			this._visual = this._selfTransform.FindChild ("Visual");
			this._icon = this._visual.GetComponent<Image> ();
		}

		#endregion

		#region Listeners

		public override void OnPointerDown (UnityEngine.EventSystems.BaseEventData eventData)
		{
			base.OnPointerDown (eventData);
			this.scaleState.scaleIn = true;
			AddState (this.scaleState);

		}

		public override void OnPointerUp (UnityEngine.EventSystems.BaseEventData eventData)
		{
			base.OnPointerUp (eventData);
			this.scaleState.scaleIn = false;
			AddState (this.scaleState);
		}

		#endregion

		#region Scale

		[Header ("Scale")]
		public float scaleDuration = 1;
		//		private float _scaleTimer = 0;
		public AnimationCurve scaleCurve;
		public float minScale, maxScale;
		private float _startFactor, _targetFactor;

		#endregion

		public HUDButtonScaleState scaleState = null;
		public HUDButtonPulseState pulseState = null;
		public AnimationCurve pulseCurve;

		public override void ConstructAvailableStates ()
		{
			this.scaleState = new HUDButtonScaleState (this);
			this.pulseState = new HUDButtonPulseState (this, this.pulseCurve);
		}

		public override void InitializeStateStack ()
		{
			this._stateStack = new List<IState> (1);
		}
	}

	public class HUDButtonScaleState : StateObject<HUDButtonView>
	{
		public float duration;
		private float _timer;

		private float _startFactor, _targetFactor;
		public bool scaleIn;

		public HUDButtonScaleState ()
		{
		}

		public HUDButtonScaleState (HUDButtonView controlledObject)
		{
			this._controlledObject = controlledObject;
			InitializeState ();
		}

		public override void EnterState ()
		{
			this.duration = this._controlledObject.scaleDuration;
			this._timer = this.duration;

			this._startFactor = this._controlledObject.visual.localScale [0];
			this._targetFactor = (this.scaleIn) ? this._controlledObject.minScale : this._controlledObject.maxScale;
		}

		public override bool UpdateState ()
		{
			this._timer -= Time.smoothDeltaTime;
			if (this._timer < 0)
				this._timer = 0;

			float normalizedTime = 1 - this._timer / this.duration;
			float mappedTime = this._controlledObject.scaleCurve.Evaluate (normalizedTime);
			float factor = Mathf.LerpUnclamped (this._startFactor, this._targetFactor, mappedTime);

			this._controlledObject.visual.localScale = Vector3.one * factor;

			if (this._timer <= 0)
				return true;
			return false;
		}
	}

	public class HUDButtonPulseState : StateObject<HUDButtonView>
	{
		public float maxScale = 0.2f;
		public float duration = 0.3f;
		private float _timer;
		public AnimationCurve curve;

		public HUDButtonPulseState ()
		{
			this._hasUpdate = true;
			InitializeState ();
		}

		public HUDButtonPulseState (HUDButtonView controlledObject, AnimationCurve curve)
		{
			this._controlledObject = controlledObject;
			this._hasUpdate = true;
			this.curve = curve;
			InitializeState ();
		}

		public override void EnterState ()
		{
			this._timer = this.duration;
			this._controlledObject.visual.localScale = Vector3.one;
		}

		public override bool UpdateState ()
		{
			this._timer -= Time.smoothDeltaTime;
			if (this._timer < 0)
				this._timer = 0;
			float normalizedTime = 1 - this._timer / this.duration;
			float newScale = this.curve.Evaluate (normalizedTime) * this.maxScale;
			this._controlledObject.visual.localScale = Vector3.one * (1 + newScale);

			if (this._timer <= 0)
				return true;
			return false;
		}

		public override void ExitState ()
		{
			this._controlledObject.visual.localScale = Vector3.one;
		}
	}
}