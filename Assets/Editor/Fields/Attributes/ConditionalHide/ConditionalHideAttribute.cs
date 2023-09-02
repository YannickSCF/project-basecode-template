using UnityEngine;
using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
public class ConditionalHideAttribute : PropertyAttribute {
    public string ConditionalSourceField = "";
    public bool HideInInspector = false;
    public List<int> ValuesToCompare = new List<int>();


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
        this.ValuesToCompare.Add(valueToCompare);
    }
    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, int valueToCompare) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.ValuesToCompare.Add(valueToCompare);
    }
    public ConditionalHideAttribute(string conditionalSourceField, int[] valuesToCompare) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = false;
        this.ValuesToCompare.AddRange(valuesToCompare);
    }
    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, int[] valuesToCompare) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideInInspector = hideInInspector;
        this.ValuesToCompare.AddRange(valuesToCompare);
    }
}

