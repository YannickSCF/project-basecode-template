/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     04/03/2023
 **/

/// Dependencies
using System.Collections;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.View.Settings.Events;

namespace YannickSCF.GeneralApp.Controller.Audio {
    public class BaseAudioController : MonoBehaviour {

        [SerializeField] protected AudioSource musicAudioSource;
        [SerializeField] protected AudioSource sfxAudioSource;

        [SerializeField] private AnimationCurve backgroundVolumeDownCurve;
        [SerializeField] private AnimationCurve backgroundVolumeUpCurve;

        private bool backgroundVolumeIsChanging = false;
        private bool isBackgroundAlreadyChanging = false;

        private float backgroundVolume = 1f;

        #region Mono
        protected virtual void OnEnable() {
            BaseSettingsViewEvents.OnSourceMuted += MuteSource;
            BaseSettingsViewEvents.OnVolumeChanged += SetGeneralVolume;
        }

        protected virtual void OnDisable() {
            BaseSettingsViewEvents.OnSourceMuted -= MuteSource;
            BaseSettingsViewEvents.OnVolumeChanged -= SetGeneralVolume;
        }
        #endregion

        #region Base audio settings methods
        public virtual void MuteSource(AudioSources source, bool mute) {
            switch (source) {
                case AudioSources.Music:
                    musicAudioSource.mute = mute;
                    break;
                case AudioSources.SFX:
                    sfxAudioSource.mute = mute;
                    break;
                case AudioSources.General:
                default:
                    musicAudioSource.mute = mute;
                    sfxAudioSource.mute = mute;
                    break;
            }
        }

        public virtual void SetGeneralVolume(AudioSources source, float volumeValue) {
            switch (source) {
                case AudioSources.Music:
                    musicAudioSource.volume = volumeValue;
                    break;
                case AudioSources.SFX:
                    sfxAudioSource.volume = volumeValue;
                    break;
                case AudioSources.General:
                default:
                    musicAudioSource.volume *= volumeValue;
                    sfxAudioSource.volume *= volumeValue;
                    break;
            }
        }
        #endregion

        #region Common public AudioSources methods
        public void OnChangeGeneralVolume(float value) {
            AudioListener.volume = value;
        }
        #endregion

        #region Background music Methods
        public void PlayBackground(AudioClip clip) {
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        public void SoftPlayBackground(AudioClip clip) {
            StartCoroutine(SoftPlayBackgroundCoroutine(clip));
        }

        private IEnumerator SoftPlayBackgroundCoroutine(AudioClip clip) {
            if (!isBackgroundAlreadyChanging) {
                isBackgroundAlreadyChanging = true;

                if (musicAudioSource.isPlaying) {
                    backgroundVolume = musicAudioSource.volume;
                    StartCoroutine(ChangeBackgroundVolume(backgroundVolumeDownCurve));
                    yield return new WaitForEndOfFrame();
                    yield return new WaitUntil(() => !backgroundVolumeIsChanging);
                }

                musicAudioSource.volume = 0;
                PlayBackground(clip);

                StartCoroutine(ChangeBackgroundVolume(backgroundVolumeUpCurve));
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => !backgroundVolumeIsChanging);
                musicAudioSource.volume = backgroundVolume;

                isBackgroundAlreadyChanging = false;
                Debug.Log("Music Changed"); //Aadir evento
            } else {
                Debug.Log("No se puede cambiar"); // Aadir warning
            }
        }

        private IEnumerator ChangeBackgroundVolume(AnimationCurve volumeCurve) {
            backgroundVolumeIsChanging = true;

            float maxVolume = backgroundVolume;

            Keyframe[] volumeKeysCurve = volumeCurve.keys;
            float time = volumeKeysCurve[0].time;
            while (time <= volumeKeysCurve[volumeKeysCurve.Length - 1].time) {
                float curveValue = volumeCurve.Evaluate(time);
                musicAudioSource.volume = curveValue * maxVolume;

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            backgroundVolumeIsChanging = false;
        }
        #endregion

        #region SFX Methods
        public void SwitchMutesfxAudioSource() {
            sfxAudioSource.enabled = !sfxAudioSource.enabled;
        }

        public void OnSFXVolumeChanged(float value) {
            sfxAudioSource.volume = value;
        }

        public void PlaySFX(AudioClip clip) {
            sfxAudioSource.PlayOneShot(clip);
        }

        public void RandomSoundSFX(AudioClip[] clips) {
            int randomIndex = Random.Range(0, clips.Length);
            sfxAudioSource.PlayOneShot(clips[randomIndex]);
        }
        #endregion

        #region Dialog Methods
        //public void SwitchMuteDialogSource() {
        //    dialogSource.enabled = !dialogSource.enabled;
        //}

        //public void OnDialogVolumeChanged(float value) {
        //    dialogSource.volume = value;
        //}

        //public void PlayDialog(AudioClip clip) {
        //    dialogSource.clip = clip;
        //    dialogSource.Play();
        //}

        //public void PauseDialog() {
        //    dialogSource.Pause();
        //}

        //public void ResumeDialog() {
        //    dialogSource.UnPause();
        //}

        //public void StopDialog() {
        //    dialogSource.Stop();
        //    dialogSource.clip = null;
        //}
        #endregion
    }
}
