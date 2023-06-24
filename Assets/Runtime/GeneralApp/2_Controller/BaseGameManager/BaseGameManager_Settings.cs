/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Settings;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial).
    /// This base game manager script refers only for SETTINGS functionalities
    /// </summary>
    public partial class BaseGameManager {

        [SerializeField] protected BaseSettingsController settingsController;

        #region Basic settings methods
        public void OpenSettingsView() {
            settingsController.OpenSettingsView(uiManager.UIDisplay);
        }

        public void CloseSettingsView() {
            settingsController.CloseSettingsView();
        }
        #endregion
    }
}
