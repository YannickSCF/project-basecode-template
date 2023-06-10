/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Audio;
using YannickSCF.GeneralApp.Scriptables.Audio;
using YannickSCF.GeneralApp.View.ComponentTools.SoundButton;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial).
    /// This base game manager script refers only for AUDIO functionalities
    /// </summary>
    public partial class BaseGameManager {
        
        [Header("Audio Game Management parameters")]
        [SerializeField] protected BaseAudioController audioController;
        [SerializeField] protected AudiosDatabase _audiosDatabase;

        #region Methods for Mono (in BaseGameManager)
        protected void AddAudioListeners() {
            SoundForButton.OnSoundButtonClicked += PlayButtonSound;
        }

        protected void RemoveAudioListeners() {
            SoundForButton.OnSoundButtonClicked -= PlayButtonSound;
        }
        #endregion

        #region Event listeners methods
        private void PlayButtonSound(string clipName) {
            AudioClip clip = _audiosDatabase.GetSFXSound(clipName);
            if (clip != null) {
                audioController.PlaySFX(clip);
            } else {
                Debug.LogWarning($"Clip '{clipName}' is not stored in database!");
            }
        }
        #endregion
    }
}
