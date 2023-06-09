/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/06/2023
 **/

/// Dependencies
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// Custom Dependencies

namespace YannickSCF.GeneralApp.Scriptables.Audio {
    [CreateAssetMenu(fileName = "Audio Database", menuName = "Scriptable Objects/YannickSCF/Audio Database")]
    public class AudiosDatabase : ScriptableObject {

        [Header("Background music list")]
        [SerializeField] private List<AudiosData> _allBackgroundMusic;

        [Header("SFX sounds list")]
        [SerializeField] private List<AudiosData> _allSFXSounds;

        public AudioClip GetBackgroundMusic(string backgroundMusicName) {
            return _allBackgroundMusic.FirstOrDefault(x => x.ClipName.Equals(backgroundMusicName)).Clip;
        }

        public List<string> GetAllBackgroundMusicNames() {
            List<string> res = new List<string>();
            foreach(AudiosData data in _allBackgroundMusic) {
                res.Add(data.ClipName);
            }
            return res;
        }

        public AudioClip GetSFXSound(string sfxSoundName) {
            return _allSFXSounds.FirstOrDefault(x => x.ClipName.Equals(sfxSoundName)).Clip;
        }

        public List<string> GetAllSFXSoundNames() {
            List<string> res = new List<string>();
            foreach (AudiosData data in _allSFXSounds) {
                res.Add(data.ClipName);
            }
            return res;
        }

        [Serializable]
        private class AudiosData {
            [SerializeField] private string _clipName;
            [SerializeField] private AudioClip _clip;

            public string ClipName { get => _clipName; }
            public AudioClip Clip { get => _clip; }
        }
    }
}
