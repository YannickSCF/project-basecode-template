/**
 * Author:      Yannick Santa Cruz Feuillias
 * Created:     09/06/2023
 * Reference Link: https://www.youtube.com/watch?v=ThcSHbVh7xc&ab_channel=URocks%21
 **/

/// Dependencies
using System;
using UnityEngine;

public class ListToPopupAttribute : PropertyAttribute {

    public Type MyType;
    public string PropertyName;

    public ListToPopupAttribute(Type myType, string propertyName) {
        MyType = myType;
        PropertyName = propertyName;
    }
}
