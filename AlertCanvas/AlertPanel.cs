using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PofyTools
{
	//TODO: Fix inhereting issues
	public class AlertPanel : Panel
	{
		//		public float alertDuration = 5f;
		//		private float _alertTimer = 5f;
		//
		public enum Type
		{
			ERROR = 0,
			WARNING,
			INFO,
			SUCCESS
		}
		
		//		public Text alertMessage;
		//
		//		//		protected override void Start ()
		//		//		{
		//		//			base.Start ();
		//		//			//Open ();
		//		//		}
		//
		//		public void ShowAlert (string message, Type type)
		//		{
		//			this._alertTimer = this.alertDuration;
		//
		//			this.alertMessage.text = message;
		//			this.alertMessage.color = AlertCanvas.GetAlertColor (type);
		//
		//			AddState (this.AlertUpdate);
		//			Open ();
		//		}
		//
		//		void AlertUpdate ()
		//		{
		//			this._alertTimer -= Time.unscaledDeltaTime;
		//			if (this._alertTimer <= 0f) {
		//				Close ();
		//			}
		//		}
		//
		//		protected override void OnEnable ()
		//		{
		//			base.OnEnable ();
		//			this._alertTimer = this.alertDuration;
		//			AddState (this.AlertUpdate);
		//		}
		//
		//		//		protected override void OnDisable ()
		//		//		{
		//		//			base.OnDisable ();
		//		//			RemoveAllStates ();
		//		//		}
		//
		//		public override void Subscribe ()
		//		{
		//			base.Subscribe ();
		//			AlertCanvas.Instance.alert += this.ShowAlert;
		//			AlertCanvas.Instance.alert += this.LogAlert;
		//		}
		//
		//		public override void Unsubscribe ()
		//		{
		//			base.Unsubscribe ();
		//			AlertCanvas.Instance.alert -= this.LogAlert;
		//			AlertCanvas.Instance.alert -= this.ShowAlert;
		//		}
		//
		//		void LogAlert (string msg, AlertPanel.Type type)
		//		{
		//			Debug.LogFormat ("{0} : {1}", type.ToString (), msg);
		//		}
	}
}