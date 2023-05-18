using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomUIToggle : MonoBehaviour
{
    [SerializeField] private RectTransform uiHandleRectTransform;
    [SerializeField] private Color backgroundActiveColor;
    [SerializeField] private Color handleActiveColor;

    private Toggle toggle;
    private Vector2 handlePosition;
    private Color handleDefaultColor, backgroundDefaultColor;
    private Image backgroundImage, handleImage;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = uiHandleRectTransform.anchoredPosition;
        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
        {
            OnSwitch(true);
        }
    }

    private void OnSwitch(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? -handlePosition : handlePosition;
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = on ? handleActiveColor : handleDefaultColor; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
