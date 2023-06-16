/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.UIManager;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial-base).
    /// </summary>
    public partial class BaseGameManager : GlobalSingleton<BaseGameManager> {

        [SerializeField] protected BaseUIManager _uiManager;

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
