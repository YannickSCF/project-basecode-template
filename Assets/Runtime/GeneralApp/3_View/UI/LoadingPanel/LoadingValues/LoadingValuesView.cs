/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

/// Dependencies
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace YannickSCF.GeneralApp.View.UI.LoadingPanel.LoadingValues {
    public class LoadingValuesView : MonoBehaviour {

        [Header("Loading values components")]
        [SerializeField] private Text loadingTMP;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private Text textUpdatesTMP;

        [Header("Show/Hide important values")]
        [SerializeField] private CanvasGroup allGroup;
        [SerializeField, Range(1, 5)] private float showValues = 2;
        [SerializeField, Range(1, 5)] private float hideValues = 2;

        private bool isShowingLoadingValues = false;
        public bool IsShowingLoadingValues { get => isShowingLoadingValues; }

        private Coroutine triggerCoroutine = null;
        private Coroutine progressBarCoroutine = null;

        /// <summary>
        /// Method to set-up the default state of these parameters
        /// </summary>
        public void SetLaodingValuesInitialState() {
            loadingTMP.gameObject.SetActive(false);

            progressSlider.gameObject.SetActive(false);
            progressSlider.value = 0;

            textUpdatesTMP.gameObject.SetActive(false);
            textUpdatesTMP.text = "...";

            allGroup.alpha = 0;
        }

        #region Show/Hide methods
        /// <summary>
        /// Method to show loading values indicated by entry parameters. 
        /// Also set the default values of each loading value.
        /// </summary>
        /// <param name="showLoadingText">Show typical loading text only if the parameter is true</param>
        /// <param name="showSlider">Show a progress bar only if the parameter is true.</param>
        /// <param name="showTextUpdates">Show a text canvas for updates only if the parameter is true.</param>
        public void ShowLoadingValues(bool showLoadingText, bool showSlider, bool showTextUpdates) {
            if (triggerCoroutine == null) {
                loadingTMP.gameObject.SetActive(showLoadingText);

                progressSlider.gameObject.SetActive(showSlider);
                if (showSlider) {
                    progressSlider.value = 0;
                }

                textUpdatesTMP.gameObject.SetActive(showTextUpdates);
                if (showTextUpdates) {
                    textUpdatesTMP.text = "...";
                }

                isShowingLoadingValues = true;
                triggerCoroutine = StartCoroutine(TriggerLoadingValues(true));
            }
        }

        /// <summary>
        /// Method to hide all loading values at the same time.
        /// </summary>
        public void HideLoadingValues() {
            if (triggerCoroutine == null) {
                triggerCoroutine = StartCoroutine(TriggerLoadingValues(false));
            } else {
                StartCoroutine(WaitAndTriggerLoadingValues(false));
            }
        }
        // Coroutine to show/hide the loading values
        private IEnumerator TriggerLoadingValues(bool show) {
            float timeToTrigger = show ? showValues : hideValues;
            float c_time = 0f;

            float initAlpha = show ? 0 : allGroup.alpha;
            float endAlpha = show ? 1 : 0;

            while (c_time < timeToTrigger) {
                yield return new WaitForEndOfFrame();
                c_time += Time.deltaTime;
                allGroup.alpha = Mathf.Lerp(initAlpha, endAlpha, c_time / timeToTrigger);
            }

            if (!show) {
                SetLaodingValuesInitialState();
            }
            isShowingLoadingValues = show;
            triggerCoroutine = null;
        }

        private IEnumerator WaitAndTriggerLoadingValues(bool show) {
            yield return new WaitUntil(() => triggerCoroutine == null);

            triggerCoroutine = StartCoroutine(TriggerLoadingValues(false));
        }
        #endregion

        /// <summary>
        /// Method to update progress bar with a coroutine of 1 second.
        /// </summary>
        /// <param name="progress">Progress to show. This number must be between 0 and 1</param>
        public void UpdateProgressBar(float progress) {
            if (progressBarCoroutine != null) StopCoroutine(progressBarCoroutine);

            progressBarCoroutine = StartCoroutine(UpdateProgressBarCoroutine(progress));
        }
        // Coroutine to update progress bar
        private IEnumerator UpdateProgressBarCoroutine(float progress) {
            float c_time = 0f, timeToUpdate = 1f;
            float c_progress = progressSlider.value;

            while (c_time < timeToUpdate) {
                yield return new WaitForEndOfFrame();
                c_time += Time.deltaTime;
                progressSlider.value = Mathf.Lerp(c_progress, progress, c_time / timeToUpdate);
            }

            progressBarCoroutine = null;
        }

        /// <summary>
        /// Method to update the text for progress updates given by user
        /// </summary>
        /// <param name="loadingDataText"></param>
        public void UpdateLoadingDataText(string loadingDataText) {
            textUpdatesTMP.text = loadingDataText;
        }
    }
}
