namespace PofyTools.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using PofyTools;

    public delegate void VoidDelegate ();

    public class DialogView : Panel
    {
        public const string TAG = "<color=green><b>DialogView: </b></color>";
        private static DialogView _instance = null;

        /// <summary>
        /// Dialog View Type. Block = Does not have Confirm nor Cancel button, Confirm = Only has confirm button, Cacnel = Has both confirm and cancel button
        /// </summary>
        public enum Type
        {
            Block,
            Confirm,
            Cancel,
        }

        #region UI Components
        public Text message;
        public Text confirmButtonText;
        public VoidDelegate confirmer;
        public Button cancel, confirm;
        #endregion

        #region API

        public static void Show (string message, Type type, VoidDelegate onConfirm = null)
        {
            DialogView._instance.ShowDialog (message, type, onConfirm);
        }

        public static void Hide ()
        {
            DialogView._instance.Close ();
        }

        public void ShowDialog (string message, Type type, VoidDelegate onConfirm = null)
        {
            this.message.text = message;
            this.confirmer = this.Close;

            if (type == Type.Block)
            {
                this.cancel.gameObject.SetActive (false);
                this.confirm.gameObject.SetActive (false);
            }
            else if (type == Type.Confirm)
            {
                this.confirm.gameObject.SetActive (true);
                this.cancel.gameObject.SetActive (false);
            }
            else if (type == Type.Cancel)
            {
                this.cancel.gameObject.SetActive (true);
                this.confirm.gameObject.SetActive (true);
                this.confirmer += onConfirm;
            }

            this.Open ();

#if UNITY_EDITOR
            LogDialog (message, type, onConfirm);
#endif

        }

        void LogDialog (string message, Type type, VoidDelegate onConfirm = null)
        {
            Debug.LogFormat ("{2} {0} - {1}", type.ToString (), message, TAG);
        }

        #endregion

        #region ISubscribable

        public override bool Subscribe ()
        {
            if (base.Subscribe ())
            {
                this.confirm.onClick.AddListener (this.OnConfirm);
                this.cancel.onClick.AddListener (this.Close);

                if (_instance != null)
                    Debug.LogWarning (TAG + "Singleton already exists! Overwritting...");

                DialogView._instance = this;

                return true;
            }
            return false;
        }

        public override bool Unsubscribe ()
        {
            if (base.Unsubscribe ())
            {
                this.confirm.onClick.RemoveAllListeners ();
                this.cancel.onClick.RemoveAllListeners ();

                if (_instance == this)
                    DialogView._instance = null;

                return true;
            }
            return false;
        }

        #endregion

        private void OnConfirm ()
        {
            this.confirmer ();
        }
    } 
}