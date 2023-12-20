using UnityEngine;

[System.Serializable]
public class UIReference
{
    public string name;
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
    Unknown // for elements that don't fit any of the above categories
}

// You can expand this enum with more types as needed, depending on the UI components you use in your project.
