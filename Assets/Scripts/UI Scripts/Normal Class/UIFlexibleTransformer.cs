using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlexibleTransformer
{
    private float uiPosYOriginalValue;
    private float uiPosYChangedValue;

    public UIFlexibleTransformer(float originalValue, float changedValue)
    {
        UiPosYOriginalValue = originalValue;
        UiPosYChangedValue = changedValue;
    }

    public float UiPosYOriginalValue { get => uiPosYOriginalValue; private set => uiPosYOriginalValue = value; }
    public float UiPosYChangedValue { get => uiPosYChangedValue; private set => uiPosYChangedValue = value; }  
}
