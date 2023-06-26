/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     26/06/2023
 **/

/// Custom dependencies
using YannickSCF.GeneralApp.View.UI.LoadingPanel.Events;

namespace YannickSCF.GeneralApp.Controller.Scenes {
    public static class SceneUtils {
        private static int _sceneToGo = 0;
        private static bool _showProgress = false;

        #region Load single scenes methods
        public static void ChangeSingleScene(int sceneToGo, bool showProgress = false) {
            _sceneToGo = sceneToGo;
            _showProgress = showProgress;

            GameManager.Instance.BaseUIController.LoadingController.FadeIn();

            LoadingPanelViewEvents.OnFadeInFinished += ChangeSingleSceneOnFadeInFinished;
        }

        private static void ChangeSingleSceneOnFadeInFinished() {
            GameManager.Instance.BaseUIController.LoadingController.ShowLoadingValues(true, _showProgress);
            if (_showProgress) {
                GameManager.Instance.SceneController.OnSceneLoadProgress +=
                    GameManager.Instance.BaseUIController.LoadingController.UpdateProgressBar;
            }

            GameManager.Instance.SceneController.LoadSceneByIndex(_sceneToGo);

            LoadingPanelViewEvents.OnFadeInFinished -= ChangeSingleSceneOnFadeInFinished;
            GameManager.Instance.SceneController.OnSceneLoaded += SceneLoaded;
        }

        private static void SceneLoaded() {
            GameManager.Instance.BaseUIController.LoadingController.FadeOut();

            if (_showProgress) {
                GameManager.Instance.SceneController.OnSceneLoadProgress -=
                    GameManager.Instance.BaseUIController.LoadingController.UpdateProgressBar;
            }

            GameManager.Instance.SceneController.OnSceneLoaded -= SceneLoaded;
            _sceneToGo = 0;
        }
        #endregion

        #region Load/Unload additive scenes
        public static void AddAdditiveScene(int c_sceneToGo) {
            GameManager.Instance.SceneController.LoadSceneByIndex(c_sceneToGo, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        public static void RemoveAdditiveScene(int c_sceneToGo) {
            GameManager.Instance.SceneController.UnloadSceneByIndex(c_sceneToGo);
        }
        #endregion
    }
}
