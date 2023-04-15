/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YannickSCF.GeneralApp.Editor.Fields.Scene {

    [System.Serializable]
    public class SceneField : ISerializationCallbackReceiver {

#if UNITY_EDITOR
        // What we use in editor to select the scene
        [SerializeField] private Object sceneAsset = null;
        bool IsValidSceneAsset {
            get {
                if (sceneAsset == null)
                    return false;
                return sceneAsset.GetType().Equals(typeof(SceneAsset));
            }
        }
#endif

        // This should only ever be set during serialization/deserialization!
        [SerializeField]
        private string scenePath = string.Empty;

        // Use this when you want to actually have the scene path
        public string ScenePath {
#if UNITY_EDITOR
            get {
                // In editor we always use the asset's path
                return scenePath;
            }
            set {
                scenePath = value;
                sceneAsset = GetSceneAssetFromPath;
            }
#else
            get {
                // At runtime we rely on the stored path value which we assume was serialized correctly at build time.
                // See OnBeforeSerialize and OnAfterDeserialize
                return scenePath;
            }
            set {
                scenePath = value;
            }
#endif
        }

        public static implicit operator string(SceneField sceneReference) {
            return sceneReference.ScenePath;
        }

#if UNITY_EDITOR
        // Called to prepare this data for serialization. Stubbed out when not in editor.
        public void OnBeforeSerialize() {
            HandleBeforeSerialize();
        }
        // Called to set up data for deserialization. Stubbed out when not in editor.
        public void OnAfterDeserialize() {
            // We sadly cannot touch assetdatabase during serialization, so defer by a bit.
            EditorApplication.update += HandleAfterDeserialize;
        }
#else
        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() { }
#endif



#if UNITY_EDITOR
        private SceneAsset GetSceneAssetFromPath {
            get {
                if (string.IsNullOrEmpty(scenePath))
                    return null;
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            }
        }

        private string GetScenePathFromAsset {
            get {
                if (sceneAsset == null)
                    return string.Empty;
                return AssetDatabase.GetAssetPath(sceneAsset);
            }
        }

        private void HandleBeforeSerialize() {
            // Asset is invalid but have Path to try and recover from
            if (IsValidSceneAsset == false && string.IsNullOrEmpty(scenePath) == false) {
                sceneAsset = GetSceneAssetFromPath;
                if (sceneAsset == null)
                    scenePath = string.Empty;

                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
            // Asset takes precendence and overwrites Path
            else {
                scenePath = GetScenePathFromAsset;
            }
        }

        private void HandleAfterDeserialize() {
            EditorApplication.update -= HandleAfterDeserialize;
            // Asset is valid, don't do anything - Path will always be set based on it when it matters
            if (IsValidSceneAsset)
                return;

            // Asset is invalid but have path to try and recover from
            if (string.IsNullOrEmpty(scenePath) == false) {
                sceneAsset = GetSceneAssetFromPath;
                // No asset found, path was invalid. Make sure we don't carry over the old invalid path
                if (sceneAsset == null)
                    scenePath = string.Empty;

                if (Application.isPlaying == false)
                    UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
        }
#endif
    }
}
