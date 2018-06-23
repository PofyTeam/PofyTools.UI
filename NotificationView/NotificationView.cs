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
        [Header("UI Components")]
        public Text message;
        public Image icon;
        public Image progress;
        public GameObject progressFrame;
        public State state = State.Inactive;

        public NotifyState notifyState;
        //public Image backShine;
        public bool animateProgress = false;
        public float _progressTarget = 0;
        private Package _currentPackage;

        #region IInitializable

        public override bool Initialize()
        {
            //Debug.Log (TAG + "Initialize");
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
                //if (_instance != null)
                //    Debug.LogWarning(TAG + "Singleton already exists! Overwritting...");
                //_instance = this;

                CheckQueue();
                return true;
            }
            return false;
        }

        public override bool Unsubscribe()
        {
            if (base.Unsubscribe())
            {
                if (_instance == this)
                    _instance = null;

                return true;
            }
            return false;
        }

        #endregion

        #region IToggleable

        public override void Close()
        {
            this.state = State.Inactive;
            //this.backShine.gameObject.SetActive (false);
            base.Close();

            CheckQueue();
        }

        public override void Open()
        {
            base.Open();

            //Color color = Color.white;
            //color.a = 0.25f;
            //this.backShine.color = color;
            //this.backShine.gameObject.SetActive (true);
            AddState(this.notifyState);
        }

        #endregion

        #region API
        public void Notify(Package package)
        {
#if UNITY_EDITOR
            Debug.Log(TAG + package.message);
#endif
            if (this.state == State.Inactive)
            {
                this._currentPackage = package;

                this.message.gameObject.SetActive(false);
                this.progressFrame.gameObject.SetActive(false);
                this.animateProgress = false;

                //message
                if (!string.IsNullOrEmpty(package.message))
                {
                    this.message.gameObject.SetActive(true);
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

                    this.progressFrame.gameObject.SetActive(true);
                    this.progress.fillAmount = package.progress;
                    //if (package.progress < 1)
                    //    this.icon.material = UIResourceManager.Resources.inactiveMaterial;
                }
                else if (package.progress < 0)
                {
                    this.progressFrame.gameObject.SetActive(true);
                    this.animateProgress = true;
                    this.progress.fillAmount = 0;
                    this._progressTarget = -package.progress;
                }

                //Open ();
                FadeIn();
            }
            else
            {
                if (package.Equals(this._currentPackage))
                    return;

                int queueSize = _queue.Count;
                if (queueSize > 0 && package.Equals(_queue[queueSize - 1]))
                {
                    return;
                }

                AddToQueue(package);
            }
        }

        #endregion

        #region Static API
        private static List<Package> _queue = new List<Package>();

        public static void Show(Package notificationPackage)
        {
            if (NotificationView._instance != null)
                NotificationView._instance.Notify(notificationPackage);
            else
                NotificationView.AddToQueue(notificationPackage);

        }

        public static void Show(string message, Sprite icon, float progress)
        {
            NotificationView.Show(new Package(message, icon, progress));
        }

        public static void Hide()
        {
            if (NotificationView._instance != null)
                NotificationView._instance.Close();
        }

        private static void AddToQueue(Package packageToQueue)
        {
            if (!_queue.Contains(packageToQueue))
            {
                _queue.Add(packageToQueue);
            }
        }

        private static void CheckQueue()
        {
            if (_queue.Count > 0)
            {
                Package next = _queue[0];
                _queue.RemoveAt(0);
                Show(next);
            }
        }
        #endregion

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        public class Package
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        {
            public string message = "";
            public Sprite icon = null;
            public float progress = 0f;

            public Package()
            {

            }

            public Package(string message, Sprite icon, float progress)
            {
                this.message = message;
                this.icon = icon;
                this.progress = progress;
            }

            public override bool Equals(object obj)
            {
                Package other = (Package)obj;
                if (other != null)
                {
                    return other.message == this.message && other.icon == this.icon && other.progress == this.progress;
                }
                return false;
            }
        }

        //public override void InitializeStateStack()
        //{
        //    this._stateStack = new List<IState>(1);
        //}

        public override void ConstructAvailableStates()
        {
            base.ConstructAvailableStates();
            this.notifyState = new NotifyState(this);
        }
    }

    public class NotifyState : StateObject<NotificationView>
    {
        private float _timer, _duration;

        public NotifyState()
        {
            InitializeState();
        }

        public NotifyState(NotificationView co)
        {
            this.controlledObject = co;
            InitializeState();
        }

        public override void InitializeState()
        {
            this.hasUpdate = true;
        }

        public override void EnterState()
        {
            //        Debug.LogError("Entering...");
            this.controlledObject.state = NotificationView.State.Active;
            this._duration = this.controlledObject.duration;
            this._timer = this._duration;
        }

        public override bool UpdateState()
        {
            this._timer -= Time.unscaledDeltaTime;

            if (this._timer < 0)
                this._timer = 0;
            if (this.controlledObject.animateProgress)
            {
                float normalizedTime = 1 - this._timer / this._duration;
                this.controlledObject.progress.fillAmount = Mathf.Lerp(0, this.controlledObject._progressTarget, normalizedTime * 1.33f);
            }

            if (this._timer <= 0)
                return true;

            return false;
        }

        public override void ExitState()
        {
            //        Debug.LogError("Exiting");
            //this.controlledObject.Close();
            this.controlledObject.FadeOut();
        }
    }

}