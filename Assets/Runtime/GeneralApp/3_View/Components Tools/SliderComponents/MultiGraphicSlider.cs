using UnityEngine;
using UnityEngine.UI;
using UnityEditor.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YannickSCF.GeneralApp.View.ComponentTools.SliderComponents {
    public class MultiGraphicSlider : Slider {

        public bool UseAllGraphicChilds;

        public bool UseHandleGraphic;
        public bool UseFillGraphic;
        public bool UseBackgroundGraphic;
        public Image Background;

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
                if (UseHandleGraphic) {
                    handleRect.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }

                if (UseFillGraphic) {
                    fillRect.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }

                if (UseBackgroundGraphic && Background != null) {
                    Background.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MultiGraphicSlider))]
    public class MultiGraphicSliderEditor : SliderEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            MultiGraphicSlider targetSlider = (MultiGraphicSlider)target;

            if (targetSlider.transition == Selectable.Transition.ColorTint) {
                EditorGUILayout.LabelField("Graphics to use on Color Tint", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                targetSlider.UseAllGraphicChilds = EditorGUILayout.Toggle("Use All Graphics", targetSlider.UseAllGraphicChilds);

                if (!targetSlider.UseAllGraphicChilds) {
                    EditorGUI.indentLevel++;
                    targetSlider.UseHandleGraphic = EditorGUILayout.Toggle("Use Handle", targetSlider.UseHandleGraphic);

                    targetSlider.UseFillGraphic = EditorGUILayout.Toggle("Use Fill", targetSlider.UseFillGraphic);

                    EditorGUILayout.BeginHorizontal();
                    targetSlider.UseBackgroundGraphic = EditorGUILayout.Toggle("Use Background", targetSlider.UseBackgroundGraphic);
                    if (targetSlider.UseBackgroundGraphic) {
                        targetSlider.Background = (Image)EditorGUILayout.ObjectField(targetSlider.Background, typeof(Image), true, null);
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
