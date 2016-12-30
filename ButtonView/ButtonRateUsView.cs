using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;

namespace PofyTools
{
	public class ButtonRateUsView : ButtonView
	{
		public string url = "https://play.google.com/store/apps/details?id=com.dchvsystems.fatspace.android";

		#region implemented abstract members of ButtonView

		protected override void OnClick ()
		{
			Analytics.CustomEvent ("rateUsRequested");
			Application.OpenURL (url);
		}

		#endregion
		
	}
}
