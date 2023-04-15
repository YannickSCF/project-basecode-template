/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     13/02/2023
 **/

/// Dependencies
using UnityEngine;
/// Custom dependencies
using YannickSCF.GeneralApp.Editor.Fields.Scene;

#if UNITY_EDITOR
using UnityEditor;

namespace com.YannickSCF.General.Editor.ScenesManager {

    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldDrawer : PropertyDrawer {
        // The exact name of the asset Object variable in the SceneReference object
        const string sceneAssetPropertyString = "sceneAsset";

        /// <summary>
        /// Drawing the 'SceneField' property
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var sceneAssetProperty = GetSceneAssetProperty(property);

            // Draw the Box Background
            position.height = EditorGUIUtility.singleLineHeight;

            // Draw the main Object field
            label.tooltip = "The actual Scene Asset reference.\nOn serialize this is also stored as the asset's path.";

            EditorGUI.BeginProperty(position, GUIContent.none, property);
            int sceneControlID = GUIUtility.GetControlID(FocusType.Passive);
            var selectedObject = EditorGUI.ObjectField(position, label, sceneAssetProperty.objectReferenceValue, typeof(SceneAsset), false);

            BuildUtils.BuildScene buildscene = BuildUtils.GetBuildScene(selectedObject);
            if (buildscene.assetGUID.Empty() == false) {
                if (buildscene.buildIndex == -1) {
                    label = EditorGUIUtility.IconContent("d_winbtn_mac_close");
                } else if (buildscene.scene.enabled) {
                    label = EditorGUIUtility.IconContent("d_winbtn_mac_max");
                } else {
                    label = EditorGUIUtility.IconContent("d_winbtn_mac_min");
                }

                using (new EditorGUI.DisabledScope(BuildUtils.IsReadOnly())) {
                    position.x = position.xMax - 40;
                    EditorGUI.PrefixLabel(position, sceneControlID, label);
                }
            }

            EditorGUI.EndProperty();
        }

        static SerializedProperty GetSceneAssetProperty(SerializedProperty property) {
            return property.FindPropertyRelative(sceneAssetPropertyString);
        }

        /// <summary>
        /// Various BuildSettings interactions
        /// </summary>
        static private class BuildUtils {
            // time in seconds that we have to wait before we query again when IsReadOnly() is called.
            public static float minCheckWait = 3;

            static float lastTimeChecked = 0;
            static bool cachedReadonlyVal = true;

            /// <summary>
            /// A small container for tracking scene data BuildSettings
            /// </summary>
            public struct BuildScene {
                public int buildIndex;
                public GUID assetGUID;
                public string assetPath;
                public EditorBuildSettingsScene scene;
            }

            /// <summary>
            /// Check if the build settings asset is readonly.
            /// Caches value and only queries state a max of every 'minCheckWait' seconds.
            /// </summary>
            static public bool IsReadOnly() {
                float curTime = Time.realtimeSinceStartup;
                float timeSinceLastCheck = curTime - lastTimeChecked;

                if (timeSinceLastCheck > minCheckWait) {
                    lastTimeChecked = curTime;
                    cachedReadonlyVal = QueryBuildSettingsStatus();
                }

                return cachedReadonlyVal;
            }

            /// <summary>
            /// A blocking call to the Version Control system to see if the build settings asset is readonly.
            /// Use BuildSettingsIsReadOnly for version that caches the value for better responsivenes.
            /// </summary>
            static private bool QueryBuildSettingsStatus() {
                // If no version control provider, assume not readonly
                if (UnityEditor.VersionControl.Provider.enabled == false)
                    return false;

                // If we cannot checkout, then assume we are not readonly
                if (UnityEditor.VersionControl.Provider.hasCheckoutSupport == false)
                    return false;

                //// If offline (and are using a version control provider that requires checkout) we cannot edit.
                //if (UnityEditor.VersionControl.Provider.onlineState == UnityEditor.VersionControl.OnlineState.Offline)
                //    return true;

                // Try to get status for file
                var status = UnityEditor.VersionControl.Provider.Status("ProjectSettings/EditorBuildSettings.asset", false);
                status.Wait();

                // If no status listed we can edit
                if (status.assetList == null || status.assetList.Count != 1)
                    return true;

                // If is checked out, we can edit
                if (status.assetList[0].IsState(UnityEditor.VersionControl.Asset.States.CheckedOutLocal))
                    return false;

                return true;
            }

            /// <summary>
            /// For a given Scene Asset object reference, extract its build settings data, including buildIndex.
            /// </summary>
            static public BuildScene GetBuildScene(Object sceneObject) {
                BuildScene entry = new BuildScene() {
                    buildIndex = -1,
                    assetGUID = new GUID(string.Empty)
                };

                if (sceneObject as SceneAsset == null)
                    return entry;

                entry.assetPath = AssetDatabase.GetAssetPath(sceneObject);
                entry.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(entry.assetPath));

                for (int index = 0; index < EditorBuildSettings.scenes.Length; ++index) {
                    if (entry.assetGUID.Equals(EditorBuildSettings.scenes[index].guid)) {
                        entry.scene = EditorBuildSettings.scenes[index];
                        entry.buildIndex = index;
                        return entry;
                    }
                }

                return entry;
            }
        }
    }
}
#endif
