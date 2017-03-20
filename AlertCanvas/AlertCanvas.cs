//using UnityEngine;
//using System.Collections;
//
//namespace PofyTools
//{
//	public class AlertCanvas : MonoBehaviour
//	{
//		public AlertPanel alertPanel;
//		public AlertDelegate alert = null;
//
//		public DialogPanel dialogPanel;
//		public DialogDelegate dialog = null;
//
//		public OverlayPanel overlayPanel;
//		public OverlayDelegate overlay = null;
//		public UIVoidDelegate overlayClose = null;
//
//		public UIVoidDelegate inputsDisabled;
//		public UIVoidDelegate inputsEnabled;
//
//		public FocusPanel focusPanel;
//		public BoolDelegate onFocus;
//
//		public Color successColor = Color.green, infoColor = Color.white, warningColor = Color.yellow, errorColor = Color.red;
//
//		private static AlertCanvas _instance;
//
//		public static AlertCanvas Instance {
//			get{ return AlertCanvas._instance; }
//		}
//
//		void Awake ()
//		{
//			if (_instance == null)
//				_instance = this;
//			else if (_instance != this)
//				Destroy (this.gameObject);
//			DontDestroyOnLoad (this.gameObject);
//
//			this.alert = IdleAlert;
//			this.dialog = IdleDialog;
//
//			this.overlay = IdleOverlay;
//			this.overlayClose = IdleUIVoid;
//
//			this.inputsDisabled = IdleUIVoid;
//			this.inputsEnabled = IdleUIVoid;
//
//			this.onFocus = IdleBool;
//
//			this.alertPanel.Subscribe ();
//			this.dialogPanel.Subscribe ();
//			this.overlayPanel.Subscribe ();
//			this.focusPanel.Subscribe ();
//		}
//
//		public static Color GetAlertColor (AlertPanel.Type type)
//		{
//			Color result = Color.black;
//
//			switch (type) {
//			case AlertPanel.Type.ERROR:
//				result = AlertCanvas._instance.errorColor;
//				break;
//			case AlertPanel.Type.WARNING:
//				result = AlertCanvas._instance.warningColor;
//				break;
//			case AlertPanel.Type.INFO:
//				result = AlertCanvas._instance.infoColor;
//				break;
//			case AlertPanel.Type.SUCCESS:
//				result = AlertCanvas._instance.successColor;
//				break;
//			default:
//				result = Color.white;
//				break;
//			}
//
//			return result;
//		}
//
//		public static void IdleAlert (string message, AlertPanel.Type type = AlertPanel.Type.INFO)
//		{
//		}
//
//		public static void IdleDialog (string message, bool confirm, UIVoidDelegate onConfirm, string title)
//		{
//		}
//
//		public static void IdleUIVoid ()
//		{
//		}
//
//		public static void IdleString (string value)
//		{
//		}
//
//		public static void IdleOverlay (string message, Sprite sprite, Color color)
//		{
//		}
//
//		public static void IdleBool (bool value)
//		{
//		}
//	}
//
//	public delegate void AlertDelegate (string msg, AlertPanel.Type type);
//	public delegate void DialogDelegate (string message, bool confirm, UIVoidDelegate onConfirm, string title);
//	public delegate void OverlayDelegate (string message, Sprite sprite, Color color);
//	public delegate void StringDelegate (string value);
//	public delegate void UIVoidDelegate ();
//
//	public delegate void BoolDelegate (bool value);
//	public delegate float FloatFloatDelegate (float value);
//}
