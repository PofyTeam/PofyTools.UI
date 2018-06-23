using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools.LevelLoader;

namespace PofyTools
{
    public class ButtonLoadCustomLevelView : ButtonView
    {
        [SerializeField]
        protected string _sceneToLoad;

        #region implemented abstract members of ButtonView

        protected override void OnClick()
        {
            LevelLoader.LevelLoader.LoadCustomScene(this._sceneToLoad);
        }

        #endregion

    }

}
