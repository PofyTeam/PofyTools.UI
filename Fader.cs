//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//
//namespace PofyTools
//{
//	[RequireComponent (typeof(UnityEngine.UI.Image))]
//	public class Fader : GameActor
//	{
//		
//		public enum Type
//		{
//			None,
//			In,
//			Out,
//			InOut,
//			OutIn,
//			flicker
//		}
//
//		public Type type;
//		public Image image;
//		FloatFloatDelegate fader = null;
//		public bool onStart = false;
//		public bool repeat = false;
//
//		public float duration;
//		private float _timer;
//
//		// Use this for initialization
//		protected override void Start ()
//		{
//			base.Start ();
//
//			if (this.onStart) {
//				EnterFadeState ();
//			}
//		}
//
//		void EnterFadeState ()
//		{
//			this._timer = this.duration;
//			AddState (this.Fade);
//
//			switch (this.type) {
//			case Type.None:
//				this.enabled = false;
//				break;
//			case Type.In:
//				this.fader = this.FadeIn;
//				break;
//			case Type.Out:
//				this.fader = this.FadeOut;
//				break;
//			case Type.InOut:
//				this.fader = this.FadeInOut;
//				break;
//			case Type.OutIn:
//				this.fader = this.FadeOutIn;
//				break;
//			case Type.flicker:
//				this.fader = this.Flicker;
//				break;
//			default:
//				this.enabled = false;
//				break;
//			}
//
//		}
//
//		void Fade ()
//		{
//			this._timer -= Time.smoothDeltaTime;
//			if (this._timer <= 0)
//				this._timer = 0;
//
//			float normalizedTime = 1 - (this._timer / this.duration);
//			Color color = this.image.color;
//			color.a = fader (normalizedTime);
//			this.image.color = color;
//
//			if (this._timer <= 0)
//				ExitFadeState ();
//		}
//
//		float FadeIn (float time)
//		{
//			return time;
//		}
//
//		float FadeOut (float time)
//		{
//			return 1 - time;
//		}
//
//		float FadeInOut (float time)
//		{
//			float factor = 2 * time - 1;
//			return 1 - factor * factor;
//		}
//
//		float FadeOutIn (float time)
//		{
//			float factor = 2 * time - 1;
//			return factor * factor;
//		}
//
//		float Flicker (float time)
//		{
//			return Random.Range (0f, 1f);
//		}
//
//		void ExitFadeState ()
//		{
//			if (!this.repeat) {
//				RemoveAllStates ();
//			} else
//				EnterFadeState ();
//			
//		}
//	}
//}