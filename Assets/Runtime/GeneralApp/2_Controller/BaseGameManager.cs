/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Scenes;
using YannickSCF.GeneralApp.Controller.UI;
using YannickSCF.GeneralApp.View.UI.LoadingPanel.Events;

namespace YannickSCF.GeneralApp.Controller {
    /// <summary>
    /// Base Game Manager.
    /// This base game manager script has basic functionalities for scene management
    /// </summary>
    public class BaseGameManager {

        [SerializeField] protected UIController UIController;
        [SerializeField] protected SceneController SceneController;

        protected int _sceneToGo = 0;
        protected bool _showProgress = false;

        #region Load single scenes methods
        public virtual void ChangeSingleScene(int sceneToGo, bool showProgress = false) {
            _sceneToGo = sceneToGo;
            _showProgress = showProgress;

            UIController.LoadingController.FadeIn();

            LoadingPanelViewEvents.OnFadeInFinished += ChangeSingleSceneOnFadeInFinished;
        }

        protected virtual void ChangeSingleSceneOnFadeInFinished() {
            UIController.LoadingController.ShowLoadingValues(true, _showProgress);
            if (_showProgress) SceneController.OnSceneLoadProgress += UIController.LoadingController.UpdateProgressBar;

            SceneController.LoadSceneByIndex(_sceneToGo);

            LoadingPanelViewEvents.OnFadeInFinished -= ChangeSingleSceneOnFadeInFinished;
            SceneController.OnSceneLoaded += SceneLoaded;
        }

        protected virtual void SceneLoaded() {
            UIController.LoadingController.FadeOut();

            if (_showProgress) SceneController.OnSceneLoadProgress -= UIController.LoadingController.UpdateProgressBar;
            SceneController.OnSceneLoaded -= SceneLoaded;
            _sceneToGo = 0;
        }
        #endregion

        #region Load/Unload additive scenes
        public virtual void AddAdditiveScene(int c_sceneToGo) {
            SceneController.LoadSceneByIndex(c_sceneToGo, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        public virtual void RemoveAdditiveScene(int c_sceneToGo) {
            SceneController.UnloadSceneByIndex(c_sceneToGo);
        }
        #endregion
    }
}
