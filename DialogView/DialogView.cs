namespace PofyTools.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using PofyTools;
    using TMPro;

    public delegate void VoidDelegate();

    public class DialogView : Panel
    {
        public const string TAG = "<color=green><b>DialogView: </b></color>";
        private static DialogView _instance = null;

        public static bool InitDone
        {
            get { return _instance != null; }
        }

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
        public TextMeshProUGUI message;
        public TextMeshProUGUI confirmButtonText;
        public VoidDelegate confirmer;
        public VoidDelegate canceler;
        public Button cancel, confirm;
        #endregion

        #region API
        public static bool IsActive
        {
            get { return _instance._isOpen; }
        }
        public static void Show(string message, Type type, Selectable focusOnCancel = null, VoidDelegate onConfirm = null, VoidDelegate onCancel = null)
        {
            DialogView._instance.ShowDialog(message, type, focusOnCancel, onConfirm, onCancel);
        }

        public static void Hide()
        {
            //_instance.Close ();
            _instance.FadeOut(0.3f);
        }

        public void ShowDialog(string message, Type type, Selectable focusOnCancel, VoidDelegate onConfirm = null, VoidDelegate onCancel = null)
        {
            this.message.text = message;
            this.confirmer = this.Close;

            this._selectOnClose = focusOnCancel;

            if (type == Type.Block)
            {
                this.cancel.gameObject.SetActive(false);
                this.confirm.gameObject.SetActive(false);
            }
            else if (type == Type.Confirm)
            {
                this.confirm.gameObject.SetActive(true);
                this.cancel.gameObject.SetActive(false);
            }
            else if (type == Type.Cancel)
            {
                this.cancel.gameObject.SetActive(true);
                this.confirm.gameObject.SetActive(true);
                canceler += onCancel;
                this.confirmer += onConfirm;
            }

            //this.Open ();
            this.FadeIn(0.5f);
            this.cancel.Select();

#if UNITY_EDITOR
            LogDialog(message, type, onConfirm);
#endif

        }

        void LogDialog(string message, Type type, VoidDelegate onConfirm = null)
        {
            Debug.LogFormat("{2} {0} - {1}", type.ToString(), message, TAG);
        }

        #endregion

        #region IInitializable

        public override bool Initialize()
        {
            if (base.Initialize())
            {

                if (_instance == null)
                {
                    _instance = this;
                    DontDestroyOnLoad(this.gameObject);
                }
                else if (_instance != this)
                {
                    Debug.LogWarning(TAG + "Instance already created. Aborting...");
                    Destroy(this.gameObject);
                    return false;
                }

                return true;
            }
            return false;
        }

        #endregion

        #region ISubscribable

        public override bool Subscribe()
        {
            if (base.Subscribe())
            {
                this.confirm.onClick.AddListener(this.OnConfirm);
                this.cancel.onClick.AddListener(this.Close);

                return true;
            }
            return false;
        }

        public override bool Unsubscribe()
        {
            if (base.Unsubscribe())
            {
                this.confirm.onClick.RemoveAllListeners();
                this.cancel.onClick.RemoveAllListeners();

                if (_instance == this)
                    DialogView._instance = null;

                return true;
            }
            return false;
        }

        #endregion

        #region IToggleable
        private Selectable _selectOnClose;
        
        public override void Open()
        {
            Debug.Log(TAG + "Open");

            base.Open();
        }

        public override void Close()
        {
            Debug.Log(TAG + "Close");
            
            if(canceler != null)
            {
                canceler();
                var listeners = canceler.GetInvocationList();            
                foreach (var listener in listeners)            
                    canceler -= listener as VoidDelegate; // remove all listener, to prevent showing multiple screens after cancel on quit
            }
            
            base.Close();
            if (this._selectOnClose)
            {
                this._selectOnClose.Select();
            }
        }

        #endregion

        #region Mono
        protected override void Awake()
        {
            Debug.Log(TAG + "Awake");
            base.Awake();
        }
        protected override void Start()
        {
            Debug.Log(TAG + "Start");
            base.Start();
        }

        #endregion


        private void OnConfirm()
        {
            this.confirmer();
        }        
    }
}