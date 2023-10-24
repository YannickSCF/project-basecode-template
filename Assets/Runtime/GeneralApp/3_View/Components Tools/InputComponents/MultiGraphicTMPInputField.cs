using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
using TMPro.EditorUtilities;
#endif

namespace YannickSCF.GeneralApp.View.ComponentTools.InputComponents {
    public class MultiGraphicTMPInputField : TMP_InputField {

        public bool UseAllGraphicChilds = true;

        public bool UseTargetGraphic = false;
        public bool UseTextComponent = false;
        public ColorBlock TextComponentColors;
        public bool UsePlaceholder = false;
        public ColorBlock PlaceholderColors;

        protected override void DoStateTransition(SelectionState state, bool instant) {
            Color targetColor;

            if (UseAllGraphicChilds) {
                targetColor = GetColorFromBlock(state, colors);
                foreach (var graphic in GetComponentsInChildren<Graphic>()) {
                    graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            } else {
                if (UseTargetGraphic) {
                    targetColor = GetColorFromBlock(state, colors);
                    image.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }

                if (UseTextComponent) {
                    targetColor = GetColorFromBlock(state, TextComponentColors);
                    textComponent.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }

                if (UsePlaceholder) {
                    targetColor = GetColorFromBlock(state, PlaceholderColors);
                    placeholder.GetComponent<Graphic>().CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }
        }

        private Color GetColorFromBlock(SelectionState state, ColorBlock colorsBlock) {
            return state == SelectionState.Disabled ? colorsBlock.disabledColor :
                state == SelectionState.Highlighted ? colorsBlock.highlightedColor :
                state == SelectionState.Normal ? colorsBlock.normalColor :
                state == SelectionState.Pressed ? colorsBlock.pressedColor :
                state == SelectionState.Selected ? colors.selectedColor : Color.white;
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

                GUIContent useAllGraphicChildsGUI = new GUIContent("All Graphics", "All graphics will be tinted using TMP_InputField colors assigned above");
                targetInputField.UseAllGraphicChilds = EditorGUILayout.ToggleLeft(useAllGraphicChildsGUI, targetInputField.UseAllGraphicChilds);

                if (!targetInputField.UseAllGraphicChilds) {
                    EditorGUI.indentLevel++;
                    targetInputField.UseTargetGraphic = EditorGUILayout.ToggleLeft("Target Graphic", targetInputField.UseTargetGraphic);
                    targetInputField.TextComponentColors = PaintColorBlockSection(targetInputField.TextComponentColors, "Text Component", ref targetInputField.UseTextComponent);
                    targetInputField.PlaceholderColors = PaintColorBlockSection(targetInputField.PlaceholderColors, "Placeholder", ref targetInputField.UsePlaceholder);
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        private ColorBlock PaintColorBlockSection(ColorBlock colorBlock, string title, ref bool conditionalShow) {
            conditionalShow = EditorGUILayout.BeginToggleGroup(title, conditionalShow);
            if (conditionalShow) {
                EditorGUI.indentLevel++;
                colorBlock.normalColor = EditorGUILayout.ColorField("Normal Color", colorBlock.normalColor, null);
                colorBlock.highlightedColor = EditorGUILayout.ColorField("Highlighted Color", colorBlock.highlightedColor, null);
                colorBlock.pressedColor = EditorGUILayout.ColorField("Pressed Color", colorBlock.pressedColor, null);
                colorBlock.selectedColor = EditorGUILayout.ColorField("Selected Color", colorBlock.selectedColor, null);
                colorBlock.disabledColor = EditorGUILayout.ColorField("Disabled Color", colorBlock.disabledColor, null);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndToggleGroup();

            return colorBlock;
        }
    }
#endif
}
