// Dependencies
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif

namespace YannickSCF.GeneralApp.View.ComponentTools.ToggleComponents {
    public class MultiGraphicToggle : Toggle {

        public bool UseAllGraphicChilds = true;

        public bool UseGraphic = false;
        public bool UseLabel = false;
        public Graphic Label;

        protected override void DoStateTransition(SelectionState state, bool instant) {
            var targetColor =
                state == SelectionState.Disabled ? colors.disabledColor :
                state == SelectionState.Highlighted ? colors.highlightedColor :
                state == SelectionState.Normal ? colors.normalColor :
                state == SelectionState.Pressed ? colors.pressedColor :
                state == SelectionState.Selected ? colors.selectedColor : Color.white;

            if (UseAllGraphicChilds) {
                foreach (var toggleGraphic in GetComponentsInChildren<Graphic>()) {
                    if (graphic != toggleGraphic || (graphic == toggleGraphic && isOn)) {
                        toggleGraphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                    }
                }
            } else {
                if (UseGraphic) {
                    graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MultiGraphicToggle))]
    public class MultiGraphicToggleEditor : ToggleEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            MultiGraphicToggle targetToggle = (MultiGraphicToggle)target;

            if (targetToggle.transition == Selectable.Transition.ColorTint) {
                EditorGUILayout.LabelField("Graphics to use on Color Tint", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                targetToggle.UseAllGraphicChilds = EditorGUILayout.Toggle("All Graphics", targetToggle.UseAllGraphicChilds);

                if (!targetToggle.UseAllGraphicChilds) {
                    EditorGUI.indentLevel++;
                    targetToggle.UseGraphic = EditorGUILayout.Toggle("Graphic", targetToggle.UseGraphic);

                    EditorGUILayout.BeginHorizontal();
                    targetToggle.UseLabel = EditorGUILayout.Toggle("Label", targetToggle.UseLabel);
                    if (targetToggle.UseLabel) {
                        targetToggle.Label = (Graphic)EditorGUILayout.ObjectField(targetToggle.Label, typeof(Graphic), true, null);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }
    }
#endif
}
