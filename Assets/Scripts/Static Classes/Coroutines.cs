using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TikTokContentCreator
{
    public static class Coroutines
    {
        public static readonly WaitForEndOfFrame WAIT_ENDOFFRAME;

        public const float WAIT_TEXTDECREMENT = 0.02f;

        static Coroutines()
        {
            WAIT_ENDOFFRAME = new WaitForEndOfFrame();
        }

        public static void StartCoroutines(this MonoBehaviour _monoBehaviour, Action onRoutinesEnd = null, params IEnumerator[] _routines)
        {
            _monoBehaviour.StartCoroutine(RunAllCoroutines(onRoutinesEnd, _routines));
        }

        public static IEnumerator Wait(float t, Action onWaitEnds = null)
        {
            WaitForSeconds wait = new WaitForSeconds(t);
            yield return wait;
            if(onWaitEnds != null) onWaitEnds();
        }

        public static IEnumerator DoWhileWaiting(float d, Action<float> doWhileWaiting = null, Action onWaitEnds = null)
        {
            float t = 0.0f;
            float i = 1.0f / d;

            while (t < 1.0f)
            {
                if (doWhileWaiting != null) doWhileWaiting(t);
                t += (Time.deltaTime * i);
                yield return null;
            }

            if (doWhileWaiting != null) doWhileWaiting(1.0f);
            if (onWaitEnds != null) onWaitEnds();
        }

        public static IEnumerator WaitUntil(Func<bool> condition, Action onWaitEnds = null)
        {
            while(!condition()) yield return null;
        }

        public static IEnumerator Scale(this Transform transform, Vector3 targetScale, float duration, Func<float, float> f = null, Action onScaleEnds = null)
        {
            if (f == null) f = Math.F;

            Vector3 originalScale = transform.localScale;
            float t = 0.0f;
            float i = 1.0f / duration;

            while (t < 1.0f)
            {
                transform.localScale = Vector3.LerpUnclamped(originalScale, targetScale, f(t));
                t += (Time.deltaTime * i);
                yield return null;                
            }

            transform.localScale = targetScale;
            if(onScaleEnds != null) onScaleEnds();
        }

        public static IEnumerator LerpColor(this TextMeshProUGUI textMesh, Color b, float duration, Func<float, float> f = null, Action onLerpEnds = null)
        {
            if (f == null) f = Math.F;

            Color a = textMesh.color;
            float t = 0.0f;
            float i = 1.0f / duration;

            while (t < 1.0f)
            {
                textMesh.color = Color.LerpUnclamped(a, b, f(t));
                t += (Time.deltaTime * i);
                yield return null;
            }

            textMesh.color = b;
            if (onLerpEnds != null) onLerpEnds();
        }

        public static IEnumerator LerpColor(this Image image, Color b, float duration, Func<float, float> f = null, Action onLerpEnds = null)
        {
            if (f == null) f = Math.F;

            Color a = image.color;
            float t = 0.0f;
            float i = 1.0f / duration;

            while (t < 1.0f)
            {
                image.color = Color.LerpUnclamped(a, b, f(t));
                t += (Time.deltaTime * i);
                yield return null;
            }

            image.color = b;
            if (onLerpEnds != null) onLerpEnds();
        }

        public static IEnumerator LerpColorProperty(this Action<Color> propertySetter, Color a, Color b, float duration, Func<float, float> f = null, Action onLerpEnds = null)
        {
            if (f == null) f = Math.F;

            float t = 0.0f;
            float i = 1.0f / duration;

            while(t < 1.0f)
            {
                propertySetter(Color.LerpUnclamped(a, b, f(t)));
                t += (Time.deltaTime * i);
                yield return null;
            }

            propertySetter(b);
            if(onLerpEnds != null) onLerpEnds();
        }

        public static IEnumerator LerpVector2Property(this Action<Vector2> propertySetter, Vector2 a, Vector2 b, float duration, Func<float, float> f = null, Action onLerpEnds = null)
        {
            if (f == null) f = Math.F;

            float t = 0.0f;
            float i = 1.0f / duration;

            while (t < 1.0f)
            {
                propertySetter(Vector2.LerpUnclamped(a, b, f(t)));
                t += (Time.deltaTime * i);
                yield return null;
            }

            propertySetter(b);
            if (onLerpEnds != null) onLerpEnds();
        }

        public static IEnumerator LerpVector3Property(this Action<Vector3> propertySetter, Vector3 a, Vector3 b, float duration, Func<float, float> f = null, Action onLerpEnds = null)
        {
            if (f == null) f = Math.F;

            float t = 0.0f;
            float i = 1.0f / duration;

            while (t < 1.0f)
            {
                propertySetter(Vector3.LerpUnclamped(a, b, f(t)));
                t += (Time.deltaTime * i);
                yield return null;
            }

            propertySetter(b);
            if (onLerpEnds != null) onLerpEnds();
        }

        public static IEnumerator RunAllCoroutines(Action onRoutinesEnd = null, params IEnumerator[] routines)
        {
            int current = 0;
            int count = routines.Length;
            bool allComplete = false;

            while(!allComplete)
            {
                foreach(IEnumerator routine in routines)
                {
                    if(!routine.MoveNext()) current++;
                }

                if(current == count) allComplete = true;
                current = 0;

                yield return WAIT_ENDOFFRAME;
            }

            if(onRoutinesEnd != null) onRoutinesEnd();
        }

        public static IEnumerator InterpolateLayoutGroupRoutine(this RectTransform _rectTransform, TextAnchor _anchor, float _duration, Func<float, float> f = null, Action onInterpolationEnds = null)
        {
            if (f == null) f = Math.F;

            HorizontalOrVerticalLayoutGroup layout = _rectTransform.GetComponent<HorizontalOrVerticalLayoutGroup>();
            Vector2 a = _rectTransform.pivot;
            Vector2 b = _anchor.TextAnchorPivot();
            float t = 0.0f;
            float i = 1.0f / _duration;

            while (t < 1.0f)
            {
                _rectTransform.pivot = Vector2.LerpUnclamped(a, b, f(t));
                t += (Time.deltaTime * i);
                yield return null;
            }

            _rectTransform.pivot = b;
            if (layout != null) layout.childAlignment = _anchor;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            if (onInterpolationEnds != null) onInterpolationEnds();
        }

        public static IEnumerator DecrementTextOverTime(string _text, float _decrementWait = WAIT_TEXTDECREMENT, Action<string> onTextModified = null, Action onTextDecremented = null)
        {
            StringBuilder builder = new StringBuilder(_text);
            float t = 0.0f;

            while (builder.Length > 1)
            {
                if (t >= _decrementWait)
                {
                    builder.Remove(builder.Length - 1, 1);
                    t = 0.0f;
                    if(onTextModified != null) onTextModified(builder.ToString());
                }
                else t += Time.deltaTime;

                yield return null;
            }

            if(onTextModified != null) onTextModified(string.Empty);
            if(onTextDecremented != null) onTextDecremented();
        }
    }
}