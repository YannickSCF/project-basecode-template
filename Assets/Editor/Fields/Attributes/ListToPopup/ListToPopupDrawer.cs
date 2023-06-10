/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/06/2023
 * Reference Link: https://www.youtube.com/watch?v=ThcSHbVh7xc&ab_channel=URocks%21
 **/

/// Dependencies
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        ListToPopupAttribute attribute = this.attribute as ListToPopupAttribute;
        Type type = property.serializedObject.targetObject.GetType();
        FieldInfo field = type.GetField(
            attribute.PropertyName,
            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        List<string> stringList = null;
        if (field != null) {
            stringList = field.GetValue(type) as List<string>;
        }

        if (stringList != null && stringList.Count > 0) {
            string labelName = property.name;
            if (field.FieldType.Equals(typeof(List<string>))) {
                labelName = property.displayName;
            }

            int selectedIndex = Mathf.Max(stringList.IndexOf(property.stringValue), 0);
            selectedIndex = EditorGUI.Popup(position, labelName, selectedIndex, stringList.ToArray());
            property.stringValue = stringList[selectedIndex];
        } else {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif
