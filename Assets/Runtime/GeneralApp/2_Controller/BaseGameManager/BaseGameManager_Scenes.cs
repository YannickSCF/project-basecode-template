/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Controller.Scenes;
using YannickSCF.GeneralApp.View.UI.LoadingPanel.Events;

namespace YannickSCF.GeneralApp.GameManager {
    /// <summary>
    /// Base Game Manager (partial).
    /// This base game manager script refers only for SCENE functionalities
    /// </summary>
    public partial class BaseGameManager {

        [SerializeField] protected SceneController SceneController;

        protected int sceneToGo = 0;
        protected bool showProgress = false;

        #region Load single scenes methods
        public virtual void ChangeSingleScene(int c_sceneToGo, bool _showProgress = false) {
            sceneToGo = c_sceneToGo;
            showProgress = _showProgress;

            UIController.LoadingController.FadeIn();

            LoadingPanelViewEvents.OnFadeInFinished += ChangeSingleSceneOnFadeInFinished;
        }

        protected virtual void ChangeSingleSceneOnFadeInFinished() {
            UIController.LoadingController.ShowLoadingValues(true, showProgress);
            if (showProgress) SceneController.OnSceneLoadProgress += UIController.LoadingController.UpdateProgressBar;

            SceneController.LoadSceneByIndex(sceneToGo);

            LoadingPanelViewEvents.OnFadeInFinished -= ChangeSingleSceneOnFadeInFinished;
            SceneController.OnSceneLoaded += SceneLoaded;
        }

        protected virtual void SceneLoaded() {
            UIController.LoadingController.FadeOut();

            if (showProgress) SceneController.OnSceneLoadProgress -= UIController.LoadingController.UpdateProgressBar;
            SceneController.OnSceneLoaded -= SceneLoaded;
            sceneToGo = 0;
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
