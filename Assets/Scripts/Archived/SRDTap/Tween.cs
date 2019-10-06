//reference: https://github.com/zhangxinxu/Tween

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tweenmode {
	None,
	Linear,
	Quad_easeIn,
	Quad_easeOut,
	Quad_easeInOut,
	Cubic_easeIn,
	Cubic_easeOut,
	Cubic_easeInOut,
	Quart_easeIn,
	Quart_easeOut,
	Quart_easeInOut,
	Quint_easeIn,
	Quint_easeOut,
	Quint_easeInOut,
	Sine_easeIn,
	Sine_easeOut,
	Sine_easeInOut,
	Expo_easeIn,
	Expo_easeOut,
	Expo_easeInOut,
	Circ_easeIn,
	Circ_easeOut,
	Circ_easeInOut,
	Elastic_easeIn,
	Elastic_easeOut,
	Elastic_easeInOut,
	Back_easeIn,
	Back_easeOut,
	Back_easeInOut,
	Bounce_easeIn,
	Bounce_easeOut,
	Bounce_easeInOut,
}

public class Tween {
	

	private float t,d,b,c;
	private Tweenmode tweenmode;
	public Tween (int v1, float v2, float v3, int v4, Tweenmode v5){
		t = (float) v1;
		b = v2;
		c = v3;
		d = (float) v4;
		tweenmode = v5;
	}

	public static float value (float t, float b, float c, float d, Tweenmode tm){
		float p = d * .3f;
		float a = .2f;
		float s = 1.70158f;
		if (Mathf.Abs(c) < 0.0001f) {
			return b;
		}
		switch (tm) {
		case Tweenmode.Linear:
			{
				return c * (t / d) + b; 
			}
		case Tweenmode.Quad_easeIn:
			{
				return c * (t /= d) * t + b;
			}
		case Tweenmode.Quad_easeOut:
			{
				return -c *(t /= d)*(t-2f) + b;
			}
		case Tweenmode.Quad_easeInOut:
			{
				if ((t /= d / 2f) < 1f) return c / 2f * t * t + b;
				return -c / 2f * ((--t) * (t-2f) - 1f) + b;
			}
		case Tweenmode.Cubic_easeIn:
			{
				return c * (t /= d) * t * t + b;
			}
		case Tweenmode.Cubic_easeOut:
			{
				return c * ((t = t/d - 1f) * t * t + 1f) + b;
			}
		case Tweenmode.Cubic_easeInOut:
			{
				if ((t /= d / 2f) < 1f) return c / 2f * t * t*t + b;
				return c / 2f*((t -= 2f) * t * t + 2f) + b;
			}
		case Tweenmode.Quart_easeIn:
			{
				return c * (t /= d) * t * t*t + b;
			}
		case Tweenmode.Quart_easeOut:
			{
				return -c * ((t = t/d - 1f) * t * t*t - 1f) + b;
			}
		case Tweenmode.Quart_easeInOut:
			{
				if ((t /= d / 2f) < 1f) return c / 2f * t * t * t * t + b;
				return -c / 2f * ((t -= 2f) * t * t*t - 2f) + b;
			}
		case Tweenmode.Quint_easeIn:
			{
				return c * (t /= d) * t * t * t * t + b;
			}
		case Tweenmode.Quint_easeOut:
			{
				return c * ((t = t/d - 1f) * t * t * t * t + 1f) + b;
			}
		case Tweenmode.Quint_easeInOut:
			{
				if ((t /= d / 2f) < 1f) return c / 2f * t * t * t * t * t + b;
				return c / 2f*((t -= 2f) * t * t * t * t + 2f) + b;
			}
		case Tweenmode.Sine_easeIn:
			{
				return -c * Mathf.Cos(t/d * (Mathf.PI/2f)) + c + b;
			}
		case Tweenmode.Sine_easeOut:
			{
				return c * Mathf.Sin(t/d * (Mathf.PI/2f)) + b;
			}
		case Tweenmode.Sine_easeInOut:
			{
				return -c / 2f * (Mathf.Cos(Mathf.PI * t/d) - 1f) + b;
			}
		case Tweenmode.Expo_easeIn:
			{
				return (t==0f) ? b : c * Mathf.Pow(2f, 10f * (t/d - 1f)) + b;
			}
		case Tweenmode.Expo_easeOut:
			{
				return (t==d) ? b + c : c * (-Mathf.Pow(2f, -10f * t/d) + 1f) + b;
			}
		case Tweenmode.Expo_easeInOut:
			{
				if (t==0) return b;
				if (t==d) return b+c;
				if ((t /= d / 2f) < 1f) return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
				return c / 2f * (-Mathf.Pow(2f, -10f * --t) + 2f) + b;
			}
		case Tweenmode.Circ_easeIn:
			{
				return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
			}
		case Tweenmode.Circ_easeOut:
			{
				return c * Mathf.Sqrt(1f - (t = t/d - 1f) * t) + b;
			}
		case Tweenmode.Circ_easeInOut:
			{
				if ((t /= d / 2f) < 1f) return -c / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
				return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
			}
		case Tweenmode.Elastic_easeIn:
			{
				if (t==0) return b;
				if ((t /= d) == 1f) return b + c;

				if (a < Mathf.Abs(c)) {
					s = p / 4f;
					a = c;
				} else {
					s = p / (2f * Mathf.PI) * Mathf.Asin(c / a);
				}
				return -(a * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p)) + b;
			}
		case Tweenmode.Elastic_easeOut:
			{
				if (t==0) return b;
				if ((t /= d) == 1f) return b + c;
				if (a < Mathf.Abs(c)) {
					a = c; 
					s = p / 4f;
				} else {
					s = p/(2f*Mathf.PI) * Mathf.Asin(c/a);
				}
				return (a * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p) + c + b);
			}
		case Tweenmode.Elastic_easeInOut:
			{
				if (t==0) return b;
				if ((t /= d / 2f) == 2f) return b+c;
				p = p * 1.5f;
				if (a < Mathf.Abs(c)) {
					a = c; 
					s = p / 4f;
				} else {
					s = p / (2f  *Mathf.PI) * Mathf.Asin(c / a);
				}
				if (t < 1f) return -.5f * (a * Mathf.Pow(2, 10f* (t -=1f )) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p)) + b;
				return a * Mathf.Pow(2, -10f * (t -= 1f)) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p ) * .5f + c + b;
			}
		case Tweenmode.Back_easeIn:
			{
				return c * (t /= d) * t * ((s + 1f) * t - s) + b;
			}
		case Tweenmode.Back_easeOut:
			{
				return c * ((t = t/d - 1f) * t * ((s + 1f) * t + s) + 1f) + b;
			}
		case Tweenmode.Back_easeInOut:
			{
				if ((t /= d / 2f) < 1f) return c / 2f * (t * t * (((s *= (1.525f)) + 1f) * t - s)) + b;
				return c / 2f*((t -= 2f) * t * (((s *= (1.525f)) + 1f) * t + s) + 2f) + b;
			}
		case Tweenmode.Bounce_easeIn:
			{
				return c - Tween.value(d-t, 0, c, d, Tweenmode.Bounce_easeOut) + b;
			}
		case Tweenmode.Bounce_easeOut:
			{
				if ((t /= d) < (1f / 2.75f)) {
					return c * (7.5625f * t * t) + b;
				} else if (t < (2f / 2.75f)) {
					return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
				} else if (t < (2.5f / 2.75f)) {
					return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
				} else {
					return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
				}
			}
		case Tweenmode.Bounce_easeInOut:
			{
				if (t < d / 2f) {
					return Tween.value(t * 2f, 0, c, d, Tweenmode.Bounce_easeIn) * .5f + b;
				} else {
					return Tween.value(t * 2f - d, 0, c, d, Tweenmode.Bounce_easeOut) * .5f + c * .5f + b;
				}
			}
		}
		return b;
	}

	public static Vector3 value (float t, Vector3 b, Vector3 c, float d, Tweenmode tm){
		Vector3 v = new Vector3(value (t, (float)(b.x), (float)(c.x), d, tm), value (t, (float)(b.y), (float)(c.y), d, tm), 0f);
		return v;
	}

	public static Progress value (float t, Progress b, Progress c, float d, Tweenmode tm){
		Progress p = new Progress (0,0);
		p.start = value (t, b.start, c.start, d, tm);
		p.end = value (t, b.end, c.end, d, tm);
		return p;
	}
		
}
