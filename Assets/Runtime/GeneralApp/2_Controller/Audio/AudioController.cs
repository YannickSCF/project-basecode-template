/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     04/03/2023
 **/

/// Dependencies
using System.Collections;
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Scriptables.Audio;
using YannickSCF.GeneralApp.View.ComponentTools.SoundButton;

namespace YannickSCF.GeneralApp.Controller.Audio {
    public class AudioController : MonoBehaviour {

        [SerializeField] private AudiosDatabase _audiosDatabase;

        [SerializeField] protected AudioSource musicAudioSource;
        [SerializeField] protected AudioSource sfxAudioSource;

        [SerializeField] private AnimationCurve _backgroundVolumeDownCurve;
        [SerializeField] private AnimationCurve _backgroundVolumeUpCurve;

        private bool _backgroundVolumeIsChanging = false;
        private bool _isBackgroundAlreadyChanging = false;

        private float _backgroundVolume = 1f;

        #region Mono
        protected virtual void OnEnable() {
            SoundForButton.OnSoundButtonClicked += PlaySFX;
        }
        protected virtual void OnDisable() {
            SoundForButton.OnSoundButtonClicked -= PlaySFX;
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
        public void PlayBackground(string backgroundClipName) {
            AudioClip backgroundClip = _audiosDatabase.GetBackgroundMusic(backgroundClipName);
            if (backgroundClip != null) {
                musicAudioSource.clip = backgroundClip;
                musicAudioSource.Play();
            } else {
                Debug.LogWarning($"Clip '{backgroundClipName}' is not stored in database!");
            }
        }
        public void SoftPlayBackground(string backgroundClipName) {
            StartCoroutine(SoftPlayBackgroundCoroutine(backgroundClipName));
        }
        private IEnumerator SoftPlayBackgroundCoroutine(string backgroundClipName) {
            if (!_isBackgroundAlreadyChanging) {
                _isBackgroundAlreadyChanging = true;

                if (musicAudioSource.isPlaying) {
                    _backgroundVolume = musicAudioSource.volume;
                    StartCoroutine(ChangeBackgroundVolume(_backgroundVolumeDownCurve));
                    yield return new WaitForEndOfFrame();
                    yield return new WaitUntil(() => !_backgroundVolumeIsChanging);
                }

                musicAudioSource.volume = 0;
                PlayBackground(backgroundClipName);

                StartCoroutine(ChangeBackgroundVolume(_backgroundVolumeUpCurve));
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => !_backgroundVolumeIsChanging);
                musicAudioSource.volume = _backgroundVolume;

                _isBackgroundAlreadyChanging = false;
                Debug.Log("Music Changed"); //Aadir evento
            } else {
                Debug.Log("No se puede cambiar"); // Aadir warning
            }
        }

        public void StopBackground() {
            musicAudioSource.Stop();
        }
        public void SoftStopBackground() {
            StartCoroutine(SoftStopBackgroundCoroutine());
        }
        private IEnumerator SoftStopBackgroundCoroutine() {
            if (musicAudioSource.isPlaying) {
                _backgroundVolume = musicAudioSource.volume;
                StartCoroutine(ChangeBackgroundVolume(_backgroundVolumeDownCurve));
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => !_backgroundVolumeIsChanging);
            }

            musicAudioSource.volume = 0;
        }

        private IEnumerator ChangeBackgroundVolume(AnimationCurve volumeCurve) {
            _backgroundVolumeIsChanging = true;

            float maxVolume = _backgroundVolume;

            Keyframe[] volumeKeysCurve = volumeCurve.keys;
            float time = volumeKeysCurve[0].time;
            while (time <= volumeKeysCurve[volumeKeysCurve.Length - 1].time) {
                float curveValue = volumeCurve.Evaluate(time);
                musicAudioSource.volume = curveValue * maxVolume;

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _backgroundVolumeIsChanging = false;
        }
        #endregion

        #region SFX Methods
        public void SwitchMutesSFXAudioSource() {
            sfxAudioSource.enabled = !sfxAudioSource.enabled;
        }

        public void OnSFXVolumeChanged(float value) {
            sfxAudioSource.volume = value;
        }

        public void PlaySFX(string clipName) {
            AudioClip clip = _audiosDatabase.GetSFXSound(clipName);
            if (clip != null) {
                sfxAudioSource.PlayOneShot(clip);
            } else {
                Debug.LogWarning($"Clip '{clipName}' is not stored in database!");
            }
        }

        public void StopSFX() {
            sfxAudioSource.Stop();
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
