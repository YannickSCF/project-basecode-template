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
    public class BaseAudioController : MonoBehaviour {

        [SerializeField] private AudiosDatabase _audiosDatabase;

        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        [SerializeField] private AnimationCurve _backgroundVolumeDownCurve;
        [SerializeField] private AnimationCurve _backgroundVolumeUpCurve;

        private bool _backgroundVolumeIsChanging = false;
        private bool _isBackgroundAlreadyChanging = false;

        private float _backgroundVolume = 1f;

        private float _audioListenerVolume = 1f;
        private bool _audioListenerMuted = false;

        #region Mono
        protected virtual void OnEnable() {
            SoundForButton.OnSoundButtonClicked += PlaySFX;
        }
        protected virtual void OnDisable() {
            SoundForButton.OnSoundButtonClicked -= PlaySFX;
        }
        #endregion

        #region Base audio settings methods
        public void MuteSource(AudioSources source, bool mute) {
            switch (source) {
                case AudioSources.Music:
                    _musicAudioSource.mute = mute;
                    break;
                case AudioSources.SFX:
                    _sfxAudioSource.mute = mute;
                    break;
                case AudioSources.General:
                default:
                    _audioListenerMuted = mute;
                    if (mute) {
                        _audioListenerVolume = AudioListener.volume;
                    }
                    AudioListener.volume = mute ? 0 : _audioListenerVolume;
                    break;
            }
        }

        public void SetGeneralVolume(AudioSources source, float volumeValue) {
            switch (source) {
                case AudioSources.Music:
                    _musicAudioSource.volume = volumeValue;
                    break;
                case AudioSources.SFX:
                    _sfxAudioSource.volume = volumeValue;
                    break;
                case AudioSources.General:
                default:
                    if (_audioListenerMuted) {
                        _audioListenerVolume = volumeValue;
                    } else {
                        AudioListener.volume = volumeValue;
                    }
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
                _musicAudioSource.clip = backgroundClip;
                _musicAudioSource.Play();
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

                if (_musicAudioSource.isPlaying) {
                    _backgroundVolume = _musicAudioSource.volume;
                    StartCoroutine(ChangeBackgroundVolume(_backgroundVolumeDownCurve));
                    yield return new WaitForEndOfFrame();
                    yield return new WaitUntil(() => !_backgroundVolumeIsChanging);
                }

                _musicAudioSource.volume = 0;
                PlayBackground(backgroundClipName);

                StartCoroutine(ChangeBackgroundVolume(_backgroundVolumeUpCurve));
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => !_backgroundVolumeIsChanging);
                _musicAudioSource.volume = _backgroundVolume;

                _isBackgroundAlreadyChanging = false;
                Debug.Log("Music Changed"); //Aadir evento
            } else {
                Debug.Log("No se puede cambiar"); // Aadir warning
            }
        }

        public void StopBackground() {
            _musicAudioSource.Stop();
        }
        public void SoftStopBackground() {
            StartCoroutine(SoftStopBackgroundCoroutine());
        }
        private IEnumerator SoftStopBackgroundCoroutine() {
            if (_musicAudioSource.isPlaying) {
                _backgroundVolume = _musicAudioSource.volume;
                StartCoroutine(ChangeBackgroundVolume(_backgroundVolumeDownCurve));
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => !_backgroundVolumeIsChanging);
            }

            _musicAudioSource.volume = 0;
        }

        private IEnumerator ChangeBackgroundVolume(AnimationCurve volumeCurve) {
            _backgroundVolumeIsChanging = true;

            float maxVolume = _backgroundVolume;

            Keyframe[] volumeKeysCurve = volumeCurve.keys;
            float time = volumeKeysCurve[0].time;
            while (time <= volumeKeysCurve[volumeKeysCurve.Length - 1].time) {
                float curveValue = volumeCurve.Evaluate(time);
                _musicAudioSource.volume = curveValue * maxVolume;

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _backgroundVolumeIsChanging = false;
        }

        protected bool IsMusicPlaying() {
            return _musicAudioSource.isPlaying;
        }
        #endregion

        #region SFX Methods
        public void SwitchMutesSFXAudioSource() {
            _sfxAudioSource.enabled = !_sfxAudioSource.enabled;
        }

        public void OnSFXVolumeChanged(float value) {
            _sfxAudioSource.volume = value;
        }

        public void PlaySFX(string clipName) {
            AudioClip clip = _audiosDatabase.GetSFXSound(clipName);
            if (clip != null) {
                _sfxAudioSource.PlayOneShot(clip);
            } else {
                Debug.LogWarning($"Clip '{clipName}' is not stored in database!");
            }
        }

        public void StopSFX() {
            _sfxAudioSource.Stop();
        }

        public void RandomSoundSFX(AudioClip[] clips) {
            int randomIndex = Random.Range(0, clips.Length);
            _sfxAudioSource.PlayOneShot(clips[randomIndex]);
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
