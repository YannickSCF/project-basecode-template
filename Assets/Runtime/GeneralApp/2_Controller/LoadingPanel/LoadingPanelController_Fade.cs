/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

/// Dependencies
using System;
using System.Collections;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.View.LoadingPanel.Fade;

namespace YannickSCF.GeneralApp.Controller.LoadingPanel {
    /// <summary>
    /// Loading Panel controller (partial).
    /// This loading panel script refers only for FADE functionalities
    /// </summary>
    public partial class LoadingPanelController {

        [Header("Fade parameters")]
        [SerializeField] private FadeView fadeView;
        [Header("Fade In/Out Initial Options")]
        [SerializeField] private bool initSceneFadedIn = true;
        [SerializeField, ConditionalHide("initSceneFadedIn", true)] private bool autoFadeOut = false;
        [SerializeField, ConditionalHide("initSceneFadedIn", true), ConditionalHide("autoFadeOut", true), Range(1, 5)] private float initialFadeOut = 1f;

        [Header("Fade In/Out time transition options")]
        [SerializeField, Range(1, 5)] private float defaultFadeInTime = 1f;
        [SerializeField, Range(1, 5)] private float defaultFadeOutTime = 1f;
        [SerializeField, Range(1, 5)] private float defaultFadeWaitingTime = 1f;

        #region Initial partial methods
        /// <summary>
        /// Method designed to check if there is a component selected and fix it
        /// TODO: Right now this method just check assigned component and alert the user
        /// </summary>
        private partial void CheckFadeViewInstantiate() {
            if (fadeView == null) {
                throw new Exception("Fade controller has not assigned view!");
            }
        }
        /// <summary>
        /// Method designed to include and execute everything related with initialize this partial class
        /// </summary>
        private partial void InitializeFadeController() {
            if (initSceneFadedIn) {
                fadeView.InstantFadeIn();
                
                if (autoFadeOut) {
                    fadeView.FadeOut(initialFadeOut);
                }
            }
        }
        #endregion

        #region Fade methods
        /// <summary>
        /// Method to execute a fade in followed by a fade out, with acustomizable waiting time between them.
        /// ALERT: This method will only be executed if there is no other fade in progress.
        /// </summary>
        /// <param name="fadeInTime">Time to spent on fade in. If empty or -1, then will be default time.</param>
        /// <param name="fadeOutTime">Time to spent on fade out. If empty or -1, then will be default time.</param>
        /// <param name="waitingTime">Time to spent on waiting time. If empty or -1, then will be default time.</param>
        /// <returns>True if the method could have been executed. False in any other case</returns>
        public bool FadeInAndOut(float fadeInTime = -1f, float fadeOutTime = -1f, float waitingTime = -1f) {
            if (!fadeView.IsFaded() && !fadeView.IsFadingOut && !fadeView.IsFadingIn) {
                StartCoroutine(FadeInAndOutCoroutine(   fadeInTime == -1f ? defaultFadeInTime : fadeInTime,
                                                        fadeOutTime == -1f ? defaultFadeOutTime : fadeOutTime,
                                                        waitingTime == -1f ? defaultFadeWaitingTime : waitingTime));
                return true;
            }
            return false;
        }
        private IEnumerator FadeInAndOutCoroutine(float fadeInTime, float fadeOutTime, float waitingTime) {
            // Execute fade in and wait to finish it
            fadeView.FadeIn(fadeInTime);
            yield return new WaitUntil(() => !fadeView.IsFadingIn);
            // Waiting time execute
            yield return new WaitForSeconds(waitingTime);
            // Execute fade out
            fadeView.FadeOut(fadeOutTime);
        }


        /// <summary>
        /// Method to execute a fade in.
        /// ALERT: This method will only be executed if there is no other fade in progress.
        /// </summary>
        /// <param name="fadeInTime">Time to spent on fade in. If empty or -1, then will be default time.</param>
        /// <returns>True if the method could have been executed. False in any other case</returns>
        public bool FadeIn(float fadeInTime = -1f) {
            if (!fadeView.IsFaded() && !fadeView.IsFadingOut && !fadeView.IsFadingIn) {
                fadeView.FadeIn(fadeInTime == -1f ? defaultFadeInTime : fadeInTime);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Method to execute a fade out. In case there is a fade in in progress, the method will wait
        /// to the fade in ends. Also will fade out loading values if there any showed up. 
        /// ALERT: This method will only be executed if there is no other fade OUT in progress.
        /// </summary>
        /// <param name="fadeOutTime">Time to spent on fade out. If empty or -1, then will be default time.</param>
        /// <returns>True if the method could have been executed. False in any other case</returns>
        public bool FadeOut(float fadeOutTime = -1f) {
            if (!fadeView.IsFadingOut) {
                StartCoroutine(FadeOutCoroutine(fadeOutTime));
                return true;
            }
            return false;
        }
        private IEnumerator FadeOutCoroutine(float fadeOutTime) {
            yield return new WaitUntil(() => !fadeView.IsFadingIn);

            if (IsShowingLoadingValues) {
                HideLoadingValues();
                yield return new WaitUntil(() => !IsShowingLoadingValues);
            }

            fadeView.FadeOut(fadeOutTime == -1f ? defaultFadeOutTime : fadeOutTime);
        }
        #endregion



        #region EDITOR METHODS
#if UNITY_EDITOR
        [ContextMenu("Fade In and Out (default)")]
        internal void SimulateFadeInAndOut() {
            Debug.Log("Simulated Fade In and Out: " + FadeInAndOut());
        }

        [ContextMenu("Fade In (default)")]
        internal void SimulateFadeIn() {
            Debug.Log("Simulated Fade In: " + FadeIn());
        }

        [ContextMenu("Fade Out (default)")]
        internal void SimulateFadeOut() {
            Debug.Log("Simulated Fade Out: " + FadeOut());
        }
#endif
        #endregion
    }
}
