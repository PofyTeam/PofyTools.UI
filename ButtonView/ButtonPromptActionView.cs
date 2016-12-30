namespace PofyTools
{
	using UnityEngine;
	using System.Collections;

	public abstract class ButtonPromptActionView : ButtonView
	{
		public string promptTitle;
		public string promptMessage;
		public bool isDialog = false;

		protected override void OnClick ()
		{
			AlertCanvas.Instance.dialog (this.promptMessage, this.isDialog, this.OnConfirm, this.promptTitle);
		}

		protected abstract void OnConfirm ();
	}
}