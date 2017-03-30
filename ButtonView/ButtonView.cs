using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PofyTools
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonView : StateableActor
    {
        protected Button _button;
        protected Image icon;
        protected Text _label;

        protected abstract void OnClick();

        #region Subscribe

        public override bool Subscribe()
        {
            if (base.Subscribe())
            {
                this._button.onClick.AddListener(this.OnClick);
                return true;
            }
            return false;
        }

        public override bool Unsubscribe()
        {
            if (base.Unsubscribe())
            {
                this._button.onClick.RemoveListener(this.OnClick);
                return true;
            }
            return false;
        }

        #endregion

        #region Initialize

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                this._button = GetComponent<Button>();
                this._label = GetComponentInChildren<Text>();

                return true;
            }
            return false;
        }

        #endregion
    }
}
