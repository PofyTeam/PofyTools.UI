namespace PofyTools
{
	using UnityEngine;
	using System.Collections;

	public class ButtonPromptQuitView : ButtonPromptActionView
	{
		#region implemented abstract members of ButtonPromptActionView

		protected override void OnConfirm ()
		{
			Application.Quit ();
		}

		#endregion
	}
}