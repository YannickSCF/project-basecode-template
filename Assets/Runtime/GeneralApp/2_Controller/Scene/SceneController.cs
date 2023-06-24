/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YannickSCF.GeneralApp.Controller.Scenes {
    public class SceneController : MonoBehaviour, ISerializationCallbackReceiver {

        private static List<string> _popupList;

        public delegate void SimpleEventDelegate();
        public event SimpleEventDelegate OnSceneLoaded;
        public event SimpleEventDelegate OnSceneUnloaded;

        public delegate void ProgressEventDelegate(float progress);
        public event ProgressEventDelegate OnSceneLoadProgress;

        [SerializeField, ListToPopup("_popupList")]
        private List<string> _allScenes;

        private int c_sceneIndex = 0;

        public string CurrentSceneName { get => _allScenes[c_sceneIndex]; }
        public int CurrentSceneIndex { get => c_sceneIndex; }

        #region Mono
        private void OnEnable()
        {
            SceneManager.sceneLoaded += TriggerOnSceneLoaded;
            SceneManager.sceneUnloaded += TriggerOnSceneUnloaded;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded += TriggerOnSceneLoaded;
            SceneManager.sceneUnloaded += TriggerOnSceneUnloaded;
        }
        #endregion

        #region Event listeners
        private void TriggerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            OnSceneLoaded?.Invoke();
        }
        private void TriggerOnSceneUnloaded(Scene arg0)
        {
            OnSceneUnloaded?.Invoke();
        }
        #endregion

        #region ISerializationCallbackReceiver methods
        public void OnBeforeSerialize() {
            _popupList = new List<string>();

            for(int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i) {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(
                    SceneUtility.GetScenePathByBuildIndex(i));
                _popupList.Add(sceneName);
            }
        }
        public void OnAfterDeserialize() { }
        #endregion

        public void LoadSceneByName(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) {
            for (int i = 0; i < _allScenes.Count; ++i)
            {
                if (_allScenes[i] == sceneName)
                {
                    StartCoroutine(LoadScene(i, mode));
                    break;
                }
            }
        }

        public void LoadSceneByIndex(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single) {
            StartCoroutine(LoadScene(sceneIndex, mode));
        }

        private IEnumerator LoadScene(int sceneIndex, LoadSceneMode mode) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, mode);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone) {
                OnSceneLoadProgress?.Invoke(asyncLoad.progress);
                yield return new WaitForEndOfFrame();
            }

            c_sceneIndex = sceneIndex;
            yield return new WaitForSeconds(1f);
        }

        public void UnloadSceneByIndex(int sceneIndex) {
            SceneManager.UnloadSceneAsync(sceneIndex);
        }
    }
}
