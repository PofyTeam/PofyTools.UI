using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PofyTools
{
	[RequireComponent (typeof(Button))]
	public abstract class ButtonView : MonoBehaviour, ISubscribable
	{
		protected Button _button;
		// Use this for initialization
		protected virtual void Start ()
		{
			this._button = GetComponent<Button> ();
			Subscribe ();
		}



		protected abstract void OnClick ();

		#region ISubscribable implementation

		public virtual void Subscribe ()
		{
			Unsubscribe ();
			this._button.onClick.AddListener (this.OnClick);
			this._isSubscribed = true;
		}

		public virtual void Unsubscribe ()
		{
			if (this._button != null)
				this._button.onClick.RemoveListener (this.OnClick);
			this._isSubscribed = false;
		}

		protected bool _isSubscribed = false;

		public bool isSubscribed {
			get {
				return this._isSubscribed;
			}
		}

		protected virtual void OnDestroy ()
		{
			Unsubscribe ();
		}

		#endregion
	}
}
