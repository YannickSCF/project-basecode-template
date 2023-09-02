using UnityEngine;
using UnityEngine.UI;
using UnityEditor.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YannickSCF.GeneralApp.View.ComponentTools.ButtonComponents {
    public class MultiGraphicButton : Button {

        public bool UseAllGraphicChilds;

        public bool UseTargetGraphic;
        public bool UseExtraImageGraphic;
        public Image ExtraImage;

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

                if (UseExtraImageGraphic && ExtraImage != null) {
                    ExtraImage.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MultiGraphicButton))]
    public class MultiGraphicButtonEditor : ButtonEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            MultiGraphicButton targetButton = (MultiGraphicButton)target;

            if (targetButton.transition == Selectable.Transition.ColorTint) {
                EditorGUILayout.LabelField("Graphics to use on Color Tint", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                targetButton.UseAllGraphicChilds = EditorGUILayout.Toggle("All Graphics", targetButton.UseAllGraphicChilds);

                if (!targetButton.UseAllGraphicChilds) {
                    EditorGUI.indentLevel++;
                    targetButton.UseTargetGraphic = EditorGUILayout.Toggle("Target Graphic", targetButton.UseTargetGraphic);

                    EditorGUILayout.BeginHorizontal();
                    targetButton.UseExtraImageGraphic = EditorGUILayout.Toggle("Extra Image", targetButton.UseExtraImageGraphic);
                    if (targetButton.UseExtraImageGraphic) {
                        targetButton.ExtraImage = (Image)EditorGUILayout.ObjectField(targetButton.ExtraImage, typeof(Image), true, null);
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
