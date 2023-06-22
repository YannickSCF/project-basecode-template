/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

/// Dependencies
using System.Collections;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.LoadingPanel.Events;

namespace YannickSCF.GeneralApp.View.UI.LoadingPanel.Fade {
    public abstract class FadeView : MonoBehaviour {

        protected bool isFadingIn = false;
        protected bool isFadingOut = false;

        public bool IsFadingIn { get => isFadingIn; }
        public bool IsFadingOut { get => isFadingOut; }

        #region Abstract methods
        public abstract bool IsFaded();
        public abstract Color GetColor();
        public abstract void SetColor(Color colorToSet);
        #endregion

        #region Fade In methods
        /// <summary>
        /// Method to make an instant fade in
        /// </summary>
        public virtual void InstantFadeIn() {
            Color fadeComponentColor = GetColor();
            SetColor(new Color(fadeComponentColor.r, fadeComponentColor.g, fadeComponentColor.b, 1));
        }

        /// <summary>
        /// Method to start the fade in
        /// </summary>
        /// <param name="fadeInTime">Time to spent on the fade in</param>
        public virtual void FadeIn(float fadeInTime) {
            isFadingIn = true;
            StartCoroutine(FadeCoroutine(true, fadeInTime));
        }

        /// <summary>
        /// Extra function to add any view action at the start of the Fade In
        /// </summary>
        protected virtual void OnInitFadeIn() { }
        #endregion

        #region Fade Out methods
        /// <summary>
        /// Method to make an instant fade out
        /// </summary>
        public virtual void InstantFadeOut() {
            Color fadeComponentColor = GetColor();
            SetColor(new Color(fadeComponentColor.r, fadeComponentColor.g, fadeComponentColor.b, 0));
        }

        /// <summary>
        /// Method to start the fade out
        /// </summary>
        /// <param name="fadeOutTime">Time to spent on the fade out</param>
        public virtual void FadeOut(float fadeOutTime) {
            isFadingOut = true;
            StartCoroutine(FadeCoroutine(false, fadeOutTime));
        }

        /// <summary>
        /// Extra function to add any view action at the end of the Fade Out
        /// </summary>
        protected virtual void OnFinishFadeOut() { }
        #endregion

        #region PRIVATE Methods
        /// <summary>
        /// Coroutine that makes the image fade (in or out)
        /// </summary>
        /// <param name="isFadeIn">If this parameter is true, will make a fade in. If it is false will make a fade out</param>
        /// <param name="timeToFade">Time to spent on the fade to make</param>
        /// <returns>Coroutine IEnumerator</returns>
        private IEnumerator FadeCoroutine(bool isFadeIn, float timeToFade) {
            float c_time = 0f;
            GetFadeValues(isFadeIn, out Color startColor, out Color endColor);

            // Throw the correspondant events, show the image in case of a
            // fade in and wait a little in case of a fade out
            if (isFadeIn) {
                LoadingPanelViewEvents.ThrowOnFadeInStarted();
                OnInitFadeIn();
            } else {
                LoadingPanelViewEvents.ThrowOnFadeOutStarted();
            }

            while (c_time <= timeToFade) {
                yield return new WaitForEndOfFrame();
                c_time += Time.deltaTime;
                SetColor(Color.Lerp(startColor, endColor, c_time / timeToFade));
            }

            // Throw the correspondant events, show the image in case of a
            // fade out and wait a little in case of a fade in
            if (isFadeIn) {
                isFadingIn = false;
                LoadingPanelViewEvents.ThrowOnFadeInFinished();
            } else {
                isFadingOut = false;
                OnFinishFadeOut();
                LoadingPanelViewEvents.ThrowOnFadeOutFinished();
            }
        }

        /// <summary>
        /// Method to get the color of the image based in a fade in or out
        /// </summary>
        /// <param name="isFadeIn">If this parameter is true will give fade in colors. If it is false will give fade out colors</param>
        /// <param name="m_startColor">OUT: color to init the fade</param>
        /// <param name="m_endColor">OUT: color to end the fade</param>
        private void GetFadeValues(bool isFadeIn, out Color m_startColor, out Color m_endColor) {
            Color fadeComponentColor = GetColor();
            if (isFadeIn) {
                m_startColor = new Color(fadeComponentColor.r, fadeComponentColor.g, fadeComponentColor.b, 0);
                m_endColor = new Color(fadeComponentColor.r, fadeComponentColor.g, fadeComponentColor.b, 1);
            } else {
                m_startColor = new Color(fadeComponentColor.r, fadeComponentColor.g, fadeComponentColor.b, 1);
                m_endColor = new Color(fadeComponentColor.r, fadeComponentColor.g, fadeComponentColor.b, 0);
            }
        }
        #endregion
    }
}
