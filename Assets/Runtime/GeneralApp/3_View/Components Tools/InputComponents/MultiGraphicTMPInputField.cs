using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using TMPro.EditorUtilities;
#endif

namespace YannickSCF.GeneralApp.View.ComponentTools.InputComponents {
    public class MultiGraphicTMPInputField : TMP_InputField {

        public bool UseAllGraphicChilds = true;

        public bool UseTargetGraphic = false;
        public bool UseTextComponent = false;
        public bool UsePlaceholder = false;

        protected override void DoStateTransition(SelectionState state, bool instant) {
            var targetColor =
                state == SelectionState.Disabled ? colors.disabledColor :
                state == SelectionState.Highlighted ? colors.highlightedColor :
                state == SelectionState.Normal ? colors.normalColor :
                state == SelectionState.Pressed ? colors.pressedColor :
                state == SelectionState.Selected ? colors.selectedColor : Color.white;

            if (UseAllGraphicChilds) {
                foreach (var graphic in GetComponentsInChildren<Graphic>()) {
                    graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            } else {
                if (UseTargetGraphic) {
                    image.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }

                if (UseTextComponent) {
                    textComponent.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }

                if (UsePlaceholder) {
                    placeholder.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MultiGraphicTMPInputField))]
    public class MultiGraphicTMPInputFieldEditor : TMP_InputFieldEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            MultiGraphicTMPInputField targetInputField = (MultiGraphicTMPInputField)target;

            if (targetInputField.transition == Selectable.Transition.ColorTint) {
                EditorGUILayout.LabelField("Graphics to use on Color Tint", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                targetInputField.UseAllGraphicChilds = EditorGUILayout.Toggle("All Graphics", targetInputField.UseAllGraphicChilds);

                if (!targetInputField.UseAllGraphicChilds) {
                    EditorGUI.indentLevel++;
                    targetInputField.UseTargetGraphic = EditorGUILayout.Toggle("Target Graphic", targetInputField.UseTargetGraphic);
                    targetInputField.UseTextComponent = EditorGUILayout.Toggle("Text Component", targetInputField.UseTextComponent);
                    targetInputField.UsePlaceholder = EditorGUILayout.Toggle("Placeholder", targetInputField.UsePlaceholder);
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }
    }
#endif
}
