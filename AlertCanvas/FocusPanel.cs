//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//
//namespace PofyTools
//{
//	public class FocusPanel : Panel
//	{
//		public enum Type
//		{
//			NONE,
//			AUTO,
//			SCREEN,
//			BUTTON
//		}
//
//		public Type continueOn = Type.AUTO;
//
//		public Button continueButton;
//		public Text continueText;
//
//		public EventTrigger maskTrigger;
//
//
//		protected override void Start ()
//		{
//			base.Start ();
//
//			switch (this.continueOn) {
//			case Type.NONE:
//				this.maskTrigger.gameObject.SetActive (false);
//				this.continueButton.gameObject.SetActive (false);
//				this.continueText.gameObject.SetActive (false);
//				Close ();
//				break;
//			case Type.AUTO:
//				this.continueButton.gameObject.SetActive (false);
//				this.maskTrigger.enabled = false;
//				this.continueText.gameObject.SetActive (false);
//				break;
//			case Type.SCREEN:
//				this.maskTrigger.enabled = true;
//				this.continueButton.gameObject.SetActive (false);
//				this.continueText.gameObject.SetActive (true);
//				this.continueText.text = "TAP TO CONTINUE";
//				break;
//			case Type.BUTTON:
//				this.maskTrigger.enabled = false;
//				this.continueButton.gameObject.SetActive (true);
//				this.continueText.gameObject.SetActive (false);
//				break;
//			default:
//				goto case Type.AUTO;
//			}
//
//			//Close ();
//		}
//
//		public override void Subscribe ()
//		{
//			base.Subscribe ();
//			AlertCanvas.Instance.onFocus += this.LogFocus;
//		}
//
//		public override void Unsubscribe ()
//		{
//			base.Unsubscribe ();
//			AlertCanvas.Instance.onFocus -= this.LogFocus;
//		}
//
//		#region LISTENERS
//
//		public void LogFocus (bool focus)
//		{
//			Debug.LogFormat ("{0} : {1}", this.name, focus.ToString ().ToUpper ());
//		}
//
//		protected void OnApplicationFocus (bool focus)
//		{
//			if (!focus) {
//				if (this.continueOn != Type.NONE)
//					Open ();
//				AlertCanvas.Instance.onFocus (focus);
//			} else if (this.continueOn == Type.AUTO)
//				FocusContinue ();
//		}
//
//		#endregion
//
//		public void FocusContinue ()
//		{
//			Close ();
//			AlertCanvas.Instance.onFocus (true);
//		}
//
//		public override void Close ()
//		{
//			this.maskTrigger.gameObject.SetActive (false);
//
//		}
//
//		public override void Open ()
//		{
//			//base.Open ();
//			this.maskTrigger.gameObject.SetActive (true);
//		}
//	}
//}