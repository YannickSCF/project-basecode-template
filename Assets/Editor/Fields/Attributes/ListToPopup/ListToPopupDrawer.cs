/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/06/2023
 * Reference Link: https://www.youtube.com/watch?v=ThcSHbVh7xc&ab_channel=URocks%21
 **/

/// Dependencies
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        ListToPopupAttribute atb = attribute as ListToPopupAttribute;
        List<string> stringList = null;

        if (atb.MyType.GetField(atb.PropertyName) != null) {
            stringList = atb.MyType.GetField(atb.PropertyName).GetValue(atb.MyType) as List<string>;
        }

        if (stringList != null && stringList.Count > 0) {
            int selectedIndex = Mathf.Max(stringList.IndexOf(property.stringValue), 0);
            selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, stringList.ToArray());
            property.stringValue = stringList[selectedIndex];
        } else {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif
