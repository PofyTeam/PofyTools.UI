using UnityEngine;
using System.Collections;

namespace PofyTools
{
    public class ButtonLoadMenuView : ButtonView
    {
        #region implemented abstract members of ButtonView

        protected override void OnClick()
        {
            LevelLoader.LevelLoader.LoadMenu();
        }

        #endregion

    }
}
