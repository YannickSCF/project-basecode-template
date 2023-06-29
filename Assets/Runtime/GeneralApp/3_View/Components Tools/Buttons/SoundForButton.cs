/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     10/06/2023
 **/

/// Dependencies
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// Custom dependencies
using static YannickSCF.GeneralApp.CommonEventsDelegates;

namespace YannickSCF.GeneralApp.View.ComponentTools.SoundButton {
    public class SoundForButton : MonoBehaviour, ISerializationCallbackReceiver, IPointerClickHandler {

        public static event StringEventDelegate OnSoundButtonClicked;

        private static List<string> _availableSounds;
        [SerializeField, ListToPopup("_availableSounds")]
        private string soundToPlay;

        private Button _btn;

        #region Mono
        private void Awake() {
            _btn = GetComponent<Button>();
        }
        #endregion

        #region ISerializationCallbackReceiver methods
        public void OnBeforeSerialize() {
            _availableSounds = AudioConstants.soundsOptions;
        }
        public void OnAfterDeserialize() { }
        #endregion

        #region IPointerClickHandler methods
        public void OnPointerClick(PointerEventData eventData) {
            if (_btn != null && !_btn.interactable) {
                return;
            }

            OnSoundButtonClicked?.Invoke(soundToPlay);
        }
        #endregion
    }
}
