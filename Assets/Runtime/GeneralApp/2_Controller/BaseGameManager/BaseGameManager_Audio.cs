/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Audio;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial).
    /// This base game manager script refers only for AUDIO functionalities
    /// </summary>
    public partial class BaseGameManager {

        [SerializeField] protected BaseAudioController audioController;

        #region Basic settings methods
        public virtual void PlaySFX(AudioClip clip) {
            audioController.PlaySFX(clip);
        }
        #endregion
    }
}
