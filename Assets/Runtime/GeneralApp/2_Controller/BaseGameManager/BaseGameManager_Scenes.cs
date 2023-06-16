/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Scenes;
using YannickSCF.GeneralApp.Controller.LoadingPanel;
using YannickSCF.GeneralApp.View.LoadingPanel.Events;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial).
    /// This base game manager script refers only for SCENE functionalities
    /// </summary>
    public partial class BaseGameManager {

        [SerializeField] protected SceneController sceneController;

        protected int sceneToGo = 0;
        protected bool showProgress = false;

        #region Load single scenes methods
        public virtual void ChangeSingleScene(int c_sceneToGo, bool _showProgress = false) {
            sceneToGo = c_sceneToGo;
            showProgress = _showProgress;

            _uiManager.LoadingController.FadeIn();

            LoadingPanelViewEvents.OnFadeInFinished += ChangeSingleSceneOnFadeInFinished;
        }

        protected virtual void ChangeSingleSceneOnFadeInFinished() {
            _uiManager.LoadingController.ShowLoadingValues(true, showProgress);
            if (showProgress) sceneController.OnSceneLoadProgress += _uiManager.LoadingController.UpdateProgressBar;

            sceneController.LoadSceneByIndex(sceneToGo);

            LoadingPanelViewEvents.OnFadeInFinished -= ChangeSingleSceneOnFadeInFinished;
            sceneController.OnSceneLoaded += SceneLoaded;
        }

        protected virtual void SceneLoaded() {
            _uiManager.LoadingController.FadeOut();

            if (showProgress) sceneController.OnSceneLoadProgress -= _uiManager.LoadingController.UpdateProgressBar;
            sceneController.OnSceneLoaded -= SceneLoaded;
            sceneToGo = 0;
        }
        #endregion

        #region Load/Unload additive scenes
        public virtual void AddAdditiveScene(int c_sceneToGo) {
            sceneController.LoadSceneByIndex(c_sceneToGo, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        public virtual void RemoveAdditiveScene(int c_sceneToGo) {
            sceneController.UnloadSceneByIndex(c_sceneToGo);
        }
        #endregion
    }
}
