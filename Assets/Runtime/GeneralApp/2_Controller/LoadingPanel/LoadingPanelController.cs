/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     11/02/2023
 **/

/// Dependencies
using UnityEngine;

namespace YannickSCF.GeneralApp.Controller.LoadingPanel {
    /// <summary>
    /// Loading Panel controller (partial-base).
    /// </summary>
    public partial class LoadingPanelController : MonoBehaviour {

        #region Mono
        private void Awake() {
            // Check all views needed
            CheckFadeViewInstantiate();
            CheckLoadingValuesViewInstantiate();
            // Initialize all view needed
            InitializeFadeController();
            InitializeLoadingValuesController();
        }
        #endregion

        // Method to check mandatory components
        private partial void CheckFadeViewInstantiate();
        private partial void CheckLoadingValuesViewInstantiate();

        // Method to initialize each partial class
        private partial void InitializeFadeController();
        private partial void InitializeLoadingValuesController();
    }
}
