/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Audio;
using YannickSCF.GeneralApp.Scriptables.Audio;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial).
    /// This base game manager script refers only for AUDIO functionalities
    /// </summary>
    public partial class BaseGameManager {
        
        [Header("Audio Game Management parameters")]
        [SerializeField] protected BaseAudioController audioController;
        [SerializeField] protected AudiosDatabase _audiosDatabase;
    }
}
