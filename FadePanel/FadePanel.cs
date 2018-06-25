using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PofyTools.UI;
using PofyTools;

public class FadePanel : Panel
{
    public const string TAG = "<color=green><b>FadePanel: </b></color>";
    public static FadePanel _instance;
    public static bool Initialized
    {
        get { return _instance != null; }
    }

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

    #region IToggleable

    public override void Open()
    {
        //base.Open();
        this._canvasGroup.alpha = 1f;
    }

    public override void Close()
    {
        this._canvasGroup.alpha = 0f;
    }

    #endregion

    #region Static API
    /// <summary>
    /// Creates Fade In Screen Effect with specified fading duration.
    /// </summary>
    /// <param name="duration"></param>
    public static void In(float duration = 1f)
    {
#if UNITY_EDITOR
        Debug.Log(TAG + "Fade In...");
#endif
        //As the mask fades out, everything else fades in...
        _instance.FadeOut(duration);
    }

    /// <summary>
    /// Creates Fade Out Screen Effect with specified fading duration.
    /// </summary>
    public static void Out(float duration = 1f)
    {
#if UNITY_EDITOR
        Debug.Log(TAG + "Fade Out...");
#endif
        //As the mask fades in, everything else fades out...
        _instance.FadeIn(duration);
    }

    public static void Clear()
    {
        Debug.Log(TAG + "Clear");
        _instance.RemoveAllStates();
        _instance.Close();

    }

    public static void Show()
    {
        //Debug.Log (TAG + "Show");
        _instance.RemoveAllStates();
        _instance.Open();
    }

    #endregion

}
