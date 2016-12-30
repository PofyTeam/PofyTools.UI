using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PofyTools
{
	public class DialogPanel : Panel
	{
		public Text message, title;
		public Text confirmButtonText;
		public UIVoidDelegate confirmer;
		public Button cancel, confirm;

		public void ShowDialog (string message, bool cancelable, UIVoidDelegate onConfirm = null, string title = "dialog")
		{
			this.message.text = message;
			this.title.text = title.ToUpper ();
			this.confirmer = Close;
			this.confirmer += onConfirm;

			this.cancel.gameObject.SetActive (true);
			if (!cancelable) {
				this.cancel.gameObject.SetActive (false);
				this.confirmer = Close;
			}

			this.confirm.gameObject.SetActive (true);
			this.gameObject.SetActive (true);
		}

		//		protected override void Start ()
		//		{
		//			base.Start ();
		//			//Close ();
		//		}

		public override void Subscribe ()
		{
			base.Subscribe ();
			AlertCanvas.Instance.dialog += this.ShowDialog;
			AlertCanvas.Instance.dialog += this.LogDialog;
		}

		public override void Unsubscribe ()
		{
			base.Unsubscribe ();
			AlertCanvas.Instance.dialog -= this.ShowDialog;
			AlertCanvas.Instance.dialog -= this.LogDialog;
		}

		void LogDialog (string msg, bool confirm, UIVoidDelegate onConfirm, string title)
		{
			Debug.LogFormat ("{0} : {1}", confirm, msg);
		}

		public void OnConfirm ()
		{
			this.confirmer ();	
		}
	}
}