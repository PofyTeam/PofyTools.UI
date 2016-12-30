using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace PofyTools
{
	public class Panel : GameActor, IToggleable
	{
		protected List<Selectable> _selectebles = new List<Selectable> ();
		//public bool selfSubscribe = true;
		public bool closeOnSubscribe = false;

		#region MONO

		protected override void Start ()
		{
			base.Start ();
			OnTransformChildrenChanged ();
//			Close ();

		}

		protected virtual void OnEnable ()
		{
			EnableElements ();
		}

		protected virtual void OnDisable ()
		{
			DisableElements ();
		}

		protected virtual void OnTransformChildrenChanged ()
		{
			GetComponentsInChildren<Selectable> (true, this._selectebles);
		}

		#endregion

		#region IToggleable implementation

		public virtual void Toggle ()
		{
			if (this.gameObject.activeSelf)
				Close ();
			else
				Open ();
		}

		public virtual void Open ()
		{
			this.gameObject.SetActive (true);
		}

		public virtual void Close ()
		{
			this.gameObject.SetActive (false);
		}

		#endregion

		public virtual void EnableElements ()
		{
			foreach (var selectable in this._selectebles)
				selectable.interactable = true;
		}

		public virtual void DisableElements ()
		{
			foreach (var selectable in this._selectebles)
				selectable.interactable = false;
		}


		public override void Subscribe ()
		{
			base.Subscribe ();
			if (AlertCanvas.Instance != null) {
				AlertCanvas.Instance.inputsDisabled += this.DisableElements;
				AlertCanvas.Instance.inputsEnabled += this.EnableElements;
			}
			if (this.closeOnSubscribe) {
				Close ();
			}
		}

		public override void Unsubscribe ()
		{
			base.Unsubscribe ();
			if (AlertCanvas.Instance != null) {
				AlertCanvas.Instance.inputsDisabled -= this.DisableElements;
				AlertCanvas.Instance.inputsEnabled -= this.EnableElements;
			}
		}
	}
}