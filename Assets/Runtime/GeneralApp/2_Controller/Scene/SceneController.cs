/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// Custom dependencies
using YannickSCF.GeneralApp.Editor.Fields.Scene;

namespace YannickSCF.GeneralApp.Controller.Scenes {
    public class SceneController : MonoBehaviour {

        public delegate void SimpleEventDelegate();
        public event SimpleEventDelegate OnSceneLoaded;
        public event SimpleEventDelegate OnSceneUnloaded;

        public delegate void ProgressEventDelegate(float progress);
        public event ProgressEventDelegate OnSceneLoadProgress;

        [SerializeField] private List<SceneField> allScenes;

        private int c_sceneIndex = 0;

        public string CurrentSceneName { get => allScenes[c_sceneIndex]; }
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

        public void LoadSceneByName(string sceneName, LoadSceneMode mode = LoadSceneMode.Single) {
            for (int i = 0; i < allScenes.Count; ++i)
            {
                if (allScenes[i] == sceneName)
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
