using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;

namespace PofyTools
{
    public class ButtonRateUsView : ButtonView
    {
        public string url;

        #region implemented abstract members of ButtonView

        protected override void OnClick()
        {
//            Analytics.CustomEvent("rateUsRequested", null);
            Application.OpenURL(url);
        }

        #endregion
		
    }
}
