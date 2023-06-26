/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
using YannickSCF.GeneralApp.Controller.Audio;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Scenes;
using YannickSCF.GeneralApp.Controller.UI;
using YannickSCF.GeneralApp.View.UI.LoadingPanel.Events;

namespace YannickSCF.GeneralApp.Controller {
    /// <summary>
    /// Base Game Manager.
    /// This base game manager script has basic functionalities for scene management
    /// </summary>
    public abstract class GameManager : GlobalSingleton<GameManager> {

        [Header("Basic Game Manager Controllers")]
        [SerializeField] protected BaseUIController _baseUIController;
        [SerializeField] protected BaseAudioController _baseAudioController;
        [SerializeField] protected SceneController _sceneController;

        public virtual BaseUIController BaseUIController { get => _baseUIController; }
        public virtual BaseAudioController BaseAudioController { get => _baseAudioController; }
        public SceneController SceneController { get => _sceneController; }
    }
}
