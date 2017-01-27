﻿namespace PofyTools.UI
{
	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;

	[RequireComponent (typeof(EventTrigger))]
	public class EventTriggerView : StateableActor
	{
		protected EventTrigger _eventTrigger;
		protected bool _isClicked = false;

		protected override void Awake ()
		{
			base.Awake ();
			this._eventTrigger = GetComponent<EventTrigger> ();
		}

		public virtual void OnPointerDown (BaseEventData eventData)
		{
			this._isClicked = true;
			//Debug.LogWarning (TAG + "Pointer is down.");
		}


		public virtual void OnPointerUp (BaseEventData eventData)
		{
			if (this._isClicked) {
				this._isClicked = false;
			}
			//Debug.LogWarning (TAG + "Pointer is up.");
		}

		public virtual void OnPointerExit (BaseEventData eventData)
		{
			//Debug.LogWarning (TAG + "Pointer is out.");
			this.OnPointerUp (eventData);
		}

		public virtual void OnPointerEnter (BaseEventData eventData)
		{
			if (Input.GetMouseButton (0))
				this.OnPointerDown (eventData);
		}

		#region implemented abstract members of StateableActor

		public override void ConstructAvailableStates ()
		{
//			throw new System.NotImplementedException ();
		}

		public override void InitializeStateStack ()
		{
//			throw new System.NotImplementedException ();
		}

		#endregion

		#region ISubscribable implementation

		public override void Subscribe ()
		{
			base.Subscribe ();
			this._eventTrigger.AddEventTriggerListener (EventTriggerType.PointerDown, this.OnPointerDown);
			this._eventTrigger.AddEventTriggerListener (EventTriggerType.PointerUp, this.OnPointerUp);
			this._eventTrigger.AddEventTriggerListener (EventTriggerType.PointerExit, this.OnPointerExit);
			this._eventTrigger.AddEventTriggerListener (EventTriggerType.PointerEnter, this.OnPointerEnter);

		}

		public override void Unsubscribe ()
		{
			base.Unsubscribe ();
			if (this._eventTrigger != null)
				this._eventTrigger.triggers.Clear ();
		}

		#endregion
	}
}