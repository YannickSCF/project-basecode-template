using UnityEngine;
using System;
using System.Collections;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
public class ConditionalHideAttribute : PropertyAttribute {
    public string ConditionalSourceField = "";
    public bool HideInInspector = false;
    public int ValueToComapre = -1;


    // Use this for initialization
    public ConditionalHideAttribute(string conditionalSourceField) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
    }

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
    }
    public ConditionalHideAttribute(string conditionalSourceField, int valueToCompare) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.ValueToComapre = valueToCompare;
    }
    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, int valueToCompare) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.ValueToComapre = valueToCompare;
    }
}

