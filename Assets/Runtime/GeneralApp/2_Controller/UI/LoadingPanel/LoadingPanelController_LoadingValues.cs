/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

/// Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.LoadingPanel.LoadingValues;

namespace YannickSCF.GeneralApp.Controller.UI.LoadingPanel {
    /// <summary>
    /// Loading Panel controller (partial).
    /// This loading panel script refers only for loading values functionalities
    /// </summary>
    public partial class LoadingPanelController {

        [Space]
        [Header("Loading values parameters")]
        [SerializeField] private LoadingValuesView loadingValuesView;

        public bool IsShowingLoadingValues { get => loadingValuesView.IsShowingLoadingValues; }

        #region Initial partial methods
        /// <summary>
        /// Method designed to check if there is a component selected and fix it
        /// TODO: Right now this method just check assigned component and alert the user
        /// </summary>
        private partial void CheckLoadingValuesViewInstantiate() {
            if (loadingValuesView == null) {
                throw new Exception("Loading values controller has not assigned view!");
            }
        }
        /// <summary>
        /// Method designed to include and execute everything related with initialize this partial class
        /// </summary>
        private partial void InitializeLoadingValuesController() {
            loadingValuesView.SetLaodingValuesInitialState();
        }
        #endregion

        #region Show/Hide loading values methods
        /// <summary>
        /// Show the loading values asked by the user in the entry parameters.
        /// Those parameters will appear with a fade in and they will only dissapear
        /// with a call of the "FadeOut" (LoadingPanelController_Fade) method.
        /// Alert: Once called, cannot be called again without hide it first.
        /// </summary>
        /// <param name="showLoadingText">Show typical loading text only if the parameter is true. True by default</param>
        /// <param name="showSlider">Show a progress bar only if the parameter is true. This bar can be updated using "UpdateProgressBar" method. False by default</param>
        /// <param name="showTextUpdates">Show a text canvas for updates only if the parameter is true. This canvas can be updated using "UpdateLoadingDataText" method. False by default</param>
        public void ShowLoadingValues(bool showLoadingText = true, bool showSlider = false, bool showTextUpdates = false) {
            if (!IsShowingLoadingValues) {
                StartCoroutine(ShowLoadingValuesCoroutine(showLoadingText, showSlider, showTextUpdates));
            } else {
                Debug.LogWarning("It is already showing loading values!");
            }
        }
        private IEnumerator ShowLoadingValuesCoroutine(bool showLoadingText, bool showSlider, bool showTextUpdates) {
            // Wait to Fade in finish first
            yield return new WaitUntil(() => !fadeView.IsFadingIn);
            // Check that fade out wasn't call
            if (!fadeView.IsFadingOut) {
                loadingValuesView.ShowLoadingValues(showLoadingText, showSlider, showTextUpdates);
            }
        }

        /// <summary>
        /// This method will hide all loading values with a coroutine.
        /// Alert: This method is desinged to be executed just by FadeOut (LoadingPanelController_Fade) method.
        /// </summary>
        private void HideLoadingValues() {
            if (IsShowingLoadingValues) {
                loadingValuesView.HideLoadingValues();
            } else {
                Debug.LogWarning("It isn't showing loading values!");
            }
        }
        #endregion

        /// <summary>
        /// Method to update progress bar of loading panel.
        /// This changes can only be seen if you call first method "ShowLoadingValues" and
        /// the parameter "showSlider" was true first.
        /// </summary>
        /// <param name="progress">New progress to show on progress bar (from 0% to 100%)</param>
        public void UpdateProgressBar(float progress) {
            if (progress >= 1f) progress /= 100f;
            loadingValuesView.UpdateProgressBar(progress);
        }

        /// <summary>
        /// Method to update the loading values text update.
        /// This changes can only be seen if you call first method "ShowLoadingValues" and
        /// the parameter "showTextUpdates" was true first.
        /// </summary>
        /// <param name="newUpdate">New text to show by display.</param>
        public void UpdateLoadingDataText(string newUpdate) {
            loadingValuesView.UpdateLoadingDataText(newUpdate);
        }



        #region EDITOR METHODS
#if UNITY_EDITOR
        private int progressIndex = 0;
        private List<float> progress = new List<float>() { 10, 25, 55, 75, 100 };

        [ContextMenu("Show Loading Values (All)")]
        public void SimulateShowLoadingValuesAll() {
            Debug.Log("Simulated Show Loading values (All)");
            ShowLoadingValues(true, true, true);
        }

        [ContextMenu("Show Loading Text")]
        public void SimulateShowLoadingText() {
            Debug.Log("Simulated Show Loading values (Loading Text)");
            ShowLoadingValues(true);
        }

        [ContextMenu("Show Progress Bar")]
        public void SimulateShowProgressBar() {
            Debug.Log("Simulated Show Progress Bar");
            ShowLoadingValues(false, true);
        }

        [ContextMenu("Show Text updates")]
        public void SimulateShowTextUpdates() {
            Debug.Log("Simulated Show Text Updates");
            ShowLoadingValues(false, false, true);
        }

        [ContextMenu("Update Progress Bar")]
        public void SimulateProgressBarUpdate() {
            Debug.Log("Simulated Progress Bar Update: " + progress[progressIndex] + "%");
            UpdateProgressBar(progress[progressIndex]);
            if (progressIndex + 1 >= progress.Count) {
                progressIndex = 0;
            } else progressIndex++;
        }

        [ContextMenu("Update Text updates")]
        public void SimulateNewTextUpdate() {
            string newUpdate = "New Update to show index: " + progressIndex;
            Debug.Log("Simulated New Text Update: " + progressIndex);
            UpdateLoadingDataText(newUpdate);
        }
        [ContextMenu("Hide Loading Values")]
        public void SimulateHideLoadingValues() {
            Debug.Log("Simulated Hide loading values");
            HideLoadingValues();
        }
#endif
        #endregion
    }
}
