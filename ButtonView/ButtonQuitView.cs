using UnityEngine;
using System.Collections;

namespace PofyTools
{
	public class ButtonQuitView : ButtonView
	{
		#region implemented abstract members of ButtonView

		protected override void OnClick ()
		{
			Application.Quit ();
		}

		#endregion
		
	}
}
