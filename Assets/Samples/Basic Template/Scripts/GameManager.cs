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

namespace YannickSCF.GeneralApp.Samples.Controller {
    /// <summary>
    /// Base Game Manager.
    /// This base game manager script has basic functionalities for scene management
    /// </summary>
    public class GameManager : GlobalSingleton<GameManager> {

        [Header("Basic Game Manager Controllers")]
        [SerializeField] protected BaseUIController _baseUIController;
        [SerializeField] protected BaseAudioController _baseAudioController;
        [SerializeField] protected SceneController _sceneController;

        public virtual BaseUIController BaseUIController { get => _baseUIController; }
        public virtual BaseAudioController BaseAudioController { get => _baseAudioController; }
        public SceneController SceneController { get => _sceneController; }


        private int _sceneToGo = 0;
        private bool _showProgress = false;

        #region Load single scenes methods
        public void ChangeSingleScene(int sceneToGo, bool showProgress = false) {
            _sceneToGo = sceneToGo;
            _showProgress = showProgress;

            _baseUIController.LoadingController.FadeIn();

            LoadingPanelViewEvents.OnFadeInFinished += ChangeSingleSceneOnFadeInFinished;
        }

        private void ChangeSingleSceneOnFadeInFinished() {
            _baseUIController.LoadingController.ShowLoadingValues(true, _showProgress);
            if (_showProgress) {
                _sceneController.OnSceneLoadProgress +=
                    _baseUIController.LoadingController.UpdateProgressBar;
            }

            _sceneController.LoadSceneByIndex(_sceneToGo);

            LoadingPanelViewEvents.OnFadeInFinished -= ChangeSingleSceneOnFadeInFinished;
            _sceneController.OnSceneLoaded += SceneLoaded;
        }

        private void SceneLoaded() {
            _baseUIController.LoadingController.FadeOut();

            if (_showProgress) {
                _sceneController.OnSceneLoadProgress -=
                    _baseUIController.LoadingController.UpdateProgressBar;
            }

            _sceneController.OnSceneLoaded -= SceneLoaded;
            _sceneToGo = 0;
        }
        #endregion

        #region Load/Unload additive scenes
        public void AddAdditiveScene(int c_sceneToGo) {
            _sceneController.LoadSceneByIndex(c_sceneToGo, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        public void RemoveAdditiveScene(int c_sceneToGo) {
            _sceneController.UnloadSceneByIndex(c_sceneToGo);
        }
        #endregion
    }
}
