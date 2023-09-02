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

namespace YannickSCF.GeneralApp.View.ComponentTools.ButtonComponents {
    public class SoundForButton : MonoBehaviour, ISerializationCallbackReceiver, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

        public static event StringEventDelegate SoundButtonEvent;

        private enum ButtonSoundPlayType { Click, ClickDown, ClickUp, ClickDownAndUp }
        [SerializeField] private ButtonSoundPlayType _soundType;

        private static List<string> _availableSounds;
        [SerializeField, ConditionalHide("_soundType", true, 0), ListToPopup("_availableSounds")]
        private string soundToPlayOnClick;

        [SerializeField, ConditionalHide("_soundType", true, new int[] { 1, 3 }), ListToPopup("_availableSounds")]
        private string soundToPlayOnClickDown;

        [SerializeField, ConditionalHide("_soundType", true, new int[] { 2, 3 }), ListToPopup("_availableSounds")]
        private string soundToPlayOnClickUp;

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
            if (_soundType == ButtonSoundPlayType.Click) {
                if (_btn != null && !_btn.interactable) {
                    return;
                }

                SoundButtonEvent?.Invoke(soundToPlayOnClick);
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (_soundType == ButtonSoundPlayType.ClickDown ||
                _soundType == ButtonSoundPlayType.ClickDownAndUp) {
                if (_btn != null && !_btn.interactable) {
                    return;
                }

                SoundButtonEvent?.Invoke(soundToPlayOnClickDown);
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (_soundType == ButtonSoundPlayType.ClickUp ||
                _soundType == ButtonSoundPlayType.ClickDownAndUp) {
                if (_btn != null && !_btn.interactable) {
                    return;
                }

                SoundButtonEvent?.Invoke(soundToPlayOnClickUp);
            }
        }
        #endregion
    }
}
