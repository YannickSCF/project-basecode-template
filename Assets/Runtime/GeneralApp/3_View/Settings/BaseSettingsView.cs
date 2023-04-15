/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     04/03/2023
 **/

/// Dependencies
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// Custom dependencies
using YannickSCF.GeneralApp.View.Settings.Events;

namespace YannickSCF.GeneralApp.View.Settings {
    public class BaseSettingsView : MonoBehaviour {

        [SerializeField] private Button closeButton;

        [Header("GENERAL VOLUME")]
        [Header("General volume mute button")]
        [SerializeField] protected Button generalVolumeMuteButton;
        [SerializeField] protected Sprite generalVolumeSprite;
        [SerializeField] protected Sprite generalVolumeMutedSprite;
        [SerializeField] protected Slider generalVolumeSlider;

        [Space, Header("MUSIC VOLUME")]
        [SerializeField] protected bool musicVolumeActive = true;
        [SerializeField, ConditionalHide("musicVolumeActive", true)] protected Button musicVolumeMuteButton;
        [SerializeField, ConditionalHide("musicVolumeActive", true)] protected Sprite musicVolumeSprite;
        [SerializeField, ConditionalHide("musicVolumeActive", true)] protected Sprite musicVolumeMutedSprite;
        [SerializeField, ConditionalHide("musicVolumeActive", true)] protected Slider musicVolumeSlider;

        [Space, Header("SFX VOLUME")]
        [SerializeField] protected bool sfxVolumeActive = true;
        [SerializeField, ConditionalHide("sfxVolumeActive", true)] protected Button sfxVolumeMuteButton;
        [SerializeField, ConditionalHide("sfxVolumeActive", true)] protected Sprite sfxVolumeSprite;
        [SerializeField, ConditionalHide("sfxVolumeActive", true)] protected Sprite sfxVolumeMutedSprite;
        [SerializeField, ConditionalHide("sfxVolumeActive", true)] protected Slider sfxVolumeSlider;

        #region Mono
        protected virtual void OnEnable() {
            closeButton.onClick.AddListener(OnCloseButton);

            generalVolumeMuteButton.onClick.AddListener(OnGeneralVolumeMuteButtonPressed);
            generalVolumeSlider.onValueChanged.AddListener(OnGeneralVolumeSliderChanged);

            if (musicVolumeActive) {
                musicVolumeMuteButton.onClick.AddListener(OnMusicVolumeMuteButtonPressed);
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChanged);
            }

            if (sfxVolumeActive) {
                sfxVolumeMuteButton.onClick.AddListener(OnSFXVolumeMuteButtonPressed);
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeSliderChanged);
            }
        }

        protected virtual void OnDisable() {
            closeButton.onClick.RemoveAllListeners();

            generalVolumeMuteButton.onClick.RemoveAllListeners();
            generalVolumeSlider.onValueChanged.RemoveAllListeners();

            if (musicVolumeActive) {
                musicVolumeMuteButton.onClick.RemoveAllListeners();
                musicVolumeSlider.onValueChanged.RemoveAllListeners();
            }

            if (sfxVolumeActive) {
                sfxVolumeMuteButton.onClick.RemoveAllListeners();
                sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            }
        }
        #endregion

        #region Event listeners
        private void OnCloseButton() {
            BaseSettingsViewEvents.ThrowClosePressed();
        }

        private void OnGeneralVolumeMuteButtonPressed() {
            BaseSettingsViewEvents.ThrowOnSourceMuted(AudioSources.General, !generalVolumeSlider.interactable);
            MuteGeneralButtonPressed(!generalVolumeSlider.interactable);
        }

        private void OnGeneralVolumeSliderChanged(float sliderValue) {
            BaseSettingsViewEvents.ThrowOnVolumeChanged(AudioSources.General, sliderValue);
        }

        private void OnMusicVolumeMuteButtonPressed() {
            BaseSettingsViewEvents.ThrowOnSourceMuted(AudioSources.Music, !musicVolumeSlider.interactable);
            MuteMusicButtonPressed(!musicVolumeSlider.interactable);
        }

        private void OnMusicVolumeSliderChanged(float sliderValue) {
            BaseSettingsViewEvents.ThrowOnVolumeChanged(AudioSources.Music, sliderValue);
        }

        private void OnSFXVolumeMuteButtonPressed() {
            BaseSettingsViewEvents.ThrowOnSourceMuted(AudioSources.SFX, !sfxVolumeSlider.interactable);
            MuteSFXButtonPressed(!sfxVolumeSlider.interactable);
        }

        private void OnSFXVolumeSliderChanged(float sliderValue) {
            BaseSettingsViewEvents.ThrowOnVolumeChanged(AudioSources.SFX, sliderValue);
        }
        #endregion

        #region External setters
        public virtual void SetGeneralVolumeData(bool muted, float sliderValue) {
            MuteGeneralButtonPressed(muted);
            generalVolumeSlider.SetValueWithoutNotify(sliderValue);
        }

        public virtual void SetMusicVolumeData(bool muted, float sliderValue) {
            if (musicVolumeActive) {
                MuteMusicButtonPressed(muted);
                musicVolumeSlider.SetValueWithoutNotify(sliderValue);
            }
        }

        public virtual void SetSFXVolumeData(bool muted, float sliderValue) {
            if (sfxVolumeActive) {
                MuteSFXButtonPressed(muted);
                sfxVolumeSlider.SetValueWithoutNotify(sliderValue);
            }
        }

        public virtual void SetLanguageSelected(int languageIndex) { }
        #endregion

        #region Mute buttons actions
        public virtual void MuteGeneralButtonPressed(bool muted) {
            generalVolumeMuteButton.image.sprite = muted ? generalVolumeMutedSprite : generalVolumeSprite;
            generalVolumeSlider.interactable = muted;
        }

        public virtual void MuteMusicButtonPressed(bool muted) {
            if (musicVolumeActive) {
                musicVolumeMuteButton.image.sprite = muted ? musicVolumeMutedSprite : musicVolumeSprite;
                musicVolumeSlider.interactable = muted;
            }
        }

        public virtual void MuteSFXButtonPressed(bool muted) {
            if (sfxVolumeActive) {
                sfxVolumeMuteButton.image.sprite = muted ? sfxVolumeMutedSprite : sfxVolumeSprite;
                sfxVolumeSlider.interactable = muted;
            }
        }
        #endregion

        public virtual void Close() {
            Destroy(this.gameObject);
        }
    }
}
