using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PofyTools
{

	public class OverlayPanel : Panel
	{
		public Image backgroundImage;
		public Text overlayText;

		public void ShowOverlay (string message, Sprite sprite, Color color)
		{
			this.overlayText.text = message;
			this.backgroundImage.sprite = sprite;
			this.backgroundImage.color = color;

			Open ();
		}

		public void LogOverlay (string message, Sprite sprite, Color color)
		{
			Debug.LogFormat ("{0} : {1}", this.name, message);
		}

		//		protected override void Start ()
		//		{
		//			base.Start ();
		//			Close ();
		//		}

		public override void Subscribe ()
		{
			base.Subscribe ();
			AlertCanvas.Instance.overlay += this.ShowOverlay;
			AlertCanvas.Instance.overlay += this.LogOverlay;
			AlertCanvas.Instance.overlayClose += this.Close;
		}

		public override void Unsubscribe ()
		{
			base.Unsubscribe ();
			AlertCanvas.Instance.overlay -= this.ShowOverlay;
			AlertCanvas.Instance.overlay -= this.LogOverlay;
			AlertCanvas.Instance.overlayClose -= this.Close;
		}
	}
}