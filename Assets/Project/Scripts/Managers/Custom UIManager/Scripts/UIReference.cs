using UnityEngine;

[System.Serializable]
public class UIReference
{
    public string name;
    public string fullPath;
    public string instanceID;
    public GameObject uiElement;
    public UIElementType elementType;
}

public enum UIElementType
{
    Panel,
    Button,
    Text,
    Toggle,
    InputField,
    Slider,
    Dropdown,
    ScrollView,
    Image,
    RawImage,
    Mask,
    Canvas,
    CanvasGroup,
    Unknown
}