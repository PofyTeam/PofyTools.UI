using UnityEngine;
using System.Collections;

namespace PofyTools
{
	public class ButtonOpenPanelView : ButtonView
	{
		public Panel panel;

		#region implemented abstract members of ButtonView

		protected override void OnClick ()
		{
			this.panel.Open ();
			//AlertCanvas.Instance.alert ("Opening " + panel.name, AlertPanel.Type.INFO);
		}

		#endregion
	}
}
