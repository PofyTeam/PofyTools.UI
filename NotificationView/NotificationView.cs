namespace PofyTools.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PofyTools;
    using UnityEngine.UI;

    public class NotificationView : Panel
    {
        public enum State
        {
            Inactive = 0,
            Active = 1,
        }

        public const string TAG = "<color=green><b>NotificationView: </b></color>";
        private static NotificationView _instance = null;

        public float duration = 3;
        [Header ("UI Components")]
        public Text message;
        public Image icon;
        public Image progress;
        public GameObject progressFrame;
        public State state = State.Inactive;
        private List<Package> _queue = new List<Package> ();
        public NotifyState notifyState;
        public Image backShine;
        public bool animateProgress = false;
        public float _progressTarget = 0;
        private Package _currentPackage;

        #region API
        public void Notify (Package package)
        {
            if (this.state == State.Inactive)
            {
                this._currentPackage = package;

                this.message.gameObject.SetActive (false);
                this.progressFrame.gameObject.SetActive (false);
                this.animateProgress = false;

                //message
                if (!string.IsNullOrEmpty (package.message))
                {
                    this.message.gameObject.SetActive (true);
                    this.message.text = package.message;
                }

                //icon
                if (package.icon != null)
                {
                    this.icon.sprite = package.icon;
                    this.icon.material = null;
                }
                else
                {
                    //this.icon.sprite = UIResourceManager.Resources.locations["button_location"];
                }

                //
                if (package.progress > 0)
                {

                    this.progressFrame.gameObject.SetActive (true);
                    this.progress.fillAmount = package.progress;
                    //if (package.progress < 1)
                    //    this.icon.material = UIResourceManager.Resources.inactiveMaterial;
                }
                else if (package.progress < 0)
                {
                    this.progressFrame.gameObject.SetActive (true);
                    this.animateProgress = true;
                    this.progress.fillAmount = 0;
                    this._progressTarget = -package.progress;
                }

                Open ();
            }
            else
            {


                if (package.Equals (this._currentPackage))
                    return;


                int queueSize = this._queue.Count;
                if (queueSize > 0 && package.Equals (this._queue[queueSize - 1]))
                {
                    return;
                }

                AddToQueue (package);
            }
        }

        public void Notify (string message, Sprite icon, float progress)
        {
            Notify (new Package (message, icon, progress));
        }

        public static void Show (string message, Sprite icon, float progress)
        {
            NotificationView._instance.Notify (message, icon, progress);
        }

        public static void Hide ()
        {
            NotificationView._instance.Close ();
        }

        #endregion

        #region IToggleable

        public override void Close ()
        {
            //        Debug.LogError(TAG + "I'm about to close...");
            this.state = State.Inactive;
            this.backShine.gameObject.SetActive (false);
            base.Close ();

            if (this._queue.Count > 0)
            {
                //            Debug.LogError(TAG + "Hold on! There is something > " + this._queue.Count);
                Package next = this._queue[0];
                this._queue.RemoveAt (0);
                Notify (next);

            }
        }

        public override void Open ()
        {
            base.Open ();
            //        Color color = ColorUtilities.AverageColorFromTexture(this.icon.sprite.texture);
            Color color = Color.white;
            color.a = 0.25f;
            this.backShine.color = color;
            this.backShine.gameObject.SetActive (true);
            AddState (this.notifyState);
            //GlobalEventManager.Events.dockOnNotify ();
        }

        #endregion

        #region Queuing

        void AddToQueue (Package packageToQueue)
        {
            if (!this._queue.Contains (packageToQueue))
            {
                //            Debug.LogError(TAG + "Queuing to the list...");
                this._queue.Add (packageToQueue);
            }
        }

        #endregion

        #region ISubscribable

        public override bool Subscribe ()
        {
            if (base.Subscribe ())
            {
                if (_instance != null)
                    Debug.LogWarning (TAG + "Singleton already exists! Overwritting...");
                _instance = this;

                return true;
            }
            return false;
        }

        public override bool Unsubscribe ()
        {
            if (base.Unsubscribe ())
            {
                if (_instance == this)
                    _instance = null;

                return true;
            }
            return false;
        }

        #endregion

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        public class Package
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        {
            public string message = "";
            public Sprite icon = null;
            public float progress = 0f;

            public Package ()
            {

            }

            public Package (string message, Sprite icon, float progress)
            {
                this.message = message;
                this.icon = icon;
                this.progress = progress;
            }

            public override bool Equals (object obj)
            {
                Package other = (Package)obj;
                if (other != null)
                {
                    return other.message == this.message && other.icon == this.icon && other.progress == this.progress;
                }
                return false;
            }
        }

        public override void InitializeStateStack ()
        {
            this._stateStack = new List<IState> (1);
        }

        public override void ConstructAvailableStates ()
        {
            this.notifyState = new NotifyState (this);
        }
    }

    public class NotifyState : StateObject<NotificationView>
    {
        private float _timer, _duration;

        public NotifyState ()
        {
            InitializeState ();
        }

        public NotifyState (NotificationView co)
        {
            this.controlledObject = co;
            InitializeState ();
        }

        public override void InitializeState ()
        {
            this.hasUpdate = true;
        }

        public override void EnterState ()
        {
            //        Debug.LogError("Entering...");
            this.controlledObject.state = NotificationView.State.Active;
            this._duration = this.controlledObject.duration;
            this._timer = this._duration;
        }

        public override bool UpdateState ()
        {
            this._timer -= Time.unscaledDeltaTime;

            if (this._timer < 0)
                this._timer = 0;
            if (this.controlledObject.animateProgress)
            {
                float normalizedTime = 1 - this._timer / this._duration;
                this.controlledObject.progress.fillAmount = Mathf.Lerp (0, this.controlledObject._progressTarget, normalizedTime * 1.33f);
            }

            if (this._timer <= 0)
                return true;

            return false;
        }

        public override void ExitState ()
        {
            //        Debug.LogError("Exiting");
            this.controlledObject.Close ();
        }
    }

}