/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.UI;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial-base).
    /// </summary>
    public partial class BaseGameManager : GlobalSingleton<BaseGameManager> {

        [SerializeField] protected BaseUIController uiManager;

        public BaseUIController UiManager { get => uiManager; }

        #region Mono
        protected virtual void OnEnable() {
            AddAudioListeners();
        }

        protected virtual void OnDisable() {
            RemoveAudioListeners();
        }
        #endregion
    }
}
