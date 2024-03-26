using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;


#region ____________________UICategory Class____________________

[System.Serializable]
public class UICategory
{
    public string name;
    public List<UIReference> references = new List<UIReference>();
}

#endregion ____________________UICategory Class____________________

public class UIManager : MonoBehaviour
{
    #region ____________________Serialized Fields____________________

    [SerializeField]
    private List<UICategory> uiCategories = new List<UICategory>();

    #endregion ____________________Serialized Fields____________________

    #region ____________________Singleton Pattern____________________

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UIManager");
                    instance = obj.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    #endregion ____________________Singleton Pattern____________________

    #region ____________________UI Reference Management____________________

    // Add a UI reference to a category
    public void AddUIReference(GameObject uiElement)
    {
        if (uiElement == null)
        {
            Debug.LogError("UIManager: Attempted to add a null UI element.");
            return;
        }

        UIElementType type = DetermineUIElementType(uiElement);
        string categoryName = type.ToString();
        string fullPath = GetFullPath(uiElement.transform);
        string instanceID = uiElement.GetInstanceID().ToString();

        if (IsReferenceAlreadyAdded(categoryName, fullPath, instanceID))
        {
            Debug.LogWarning("UIManager: Duplicate UI element attempted to be added. Skipping.");
            return;
        }

        UICategory category = uiCategories.Find(cat => cat.name == categoryName);
        if (category == null)
        {
            category = new UICategory { name = categoryName };
            uiCategories.Add(category);
        }

        UIReference reference = new UIReference
        {
            name = uiElement.name,
            uiElement = uiElement,
            elementType = type,
            fullPath = fullPath,
            instanceID = instanceID
        };
        category.references.Add(reference);
    }


    private bool IsReferenceAlreadyAdded(string categoryName, string fullPath, string instanceID)
    {
        UICategory category = uiCategories.Find(cat => cat.name == categoryName);
        return category?.references.Exists(uiRef => uiRef.fullPath == fullPath && uiRef.instanceID == instanceID) ?? false;
    }


    private string GetFullPath(Transform transform)
    {
        if (transform == null)
        {
            Debug.LogError("UIManager: Null transform provided for GetFullPath.");
            return string.Empty;
        }

        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }


    // Remove a UI reference by full hierarchical path
    public void RemoveUIReference(string elementType, string fullPath)
    {
        UICategory category = uiCategories.Find(cat => cat.name == elementType);
        if (category == null)
        {
            Debug.LogWarning($"UIManager: Category '{elementType}' not found for removal.");
            return;
        }

        bool removed = category.references.RemoveAll(reference => reference.fullPath == fullPath) > 0;
        if (!removed)
        {
            Debug.LogWarning($"UIManager: No reference found with path '{fullPath}' in '{elementType}' category.");
        }
    }


    // Remove a UI reference by instance ID
    public void RemoveUIReference(string elementType, string instanceID, bool isInstanceID /*This boolean exists only for the purpose of overloading the previous method */)
    {
        UICategory category = uiCategories.Find(cat => cat.name == elementType);
        if (category == null)
        {
            Debug.LogWarning($"UIManager: Category '{elementType}' not found for removal.");
            return;
        }

        bool removed = category.references.RemoveAll(reference => reference.instanceID == instanceID) > 0;
        if (!removed)
        {
            Debug.LogWarning($"UIManager: No reference found with instance ID '{instanceID}' in '{elementType}' category.");
        }
    }


    // Remove a UI reference by element type, full hierarchical path, and instance ID
    public void RemoveUIReference(string elementType, string fullPath, string instanceID)
    {
        UICategory category = uiCategories.Find(cat => cat.name == elementType);
        if (category == null)
        {
            Debug.LogWarning($"UIManager: Category '{elementType}' not found for removal.");
            return;
        }

        bool removed = category.references.RemoveAll(reference =>
            reference.fullPath == fullPath && reference.instanceID == instanceID) > 0;
        if (!removed)
        {
            Debug.LogWarning($"UIManager: No reference found with path '{fullPath}' and instance ID '{instanceID}' in '{elementType}' category.");
        }
    }


    // Get a UI reference by full hierarchical path
    public GameObject GetUIReference(string elementType, string fullPath)
    {
        UICategory category = uiCategories.Find(cat => cat.name == elementType);
        if (category == null)
        {
            Debug.LogWarning($"UIManager: Category '{elementType}' not found.");
            return null;
        }

        UIReference reference = category.references.Find(uiRef => uiRef.fullPath == fullPath);
        if (reference == null)
        {
            Debug.LogWarning($"UIManager: No reference found with path '{fullPath}' in '{elementType}' category.");
        }
        return reference?.uiElement;
    }


    // Get a UI reference by instance ID
    public GameObject GetUIReference(string elementType, string instanceID, bool isInstanceID /*This boolean exists only for the purpose of overloading the previous method */)
    {
        UICategory category = uiCategories.Find(cat => cat.name == elementType);
        if (category == null)
        {
            Debug.LogWarning($"UIManager: Category '{elementType}' not found.");
            return null;
        }

        UIReference reference = category.references.Find(uiRef => uiRef.instanceID == instanceID);
        if (reference == null)
        {
            Debug.LogWarning($"UIManager: No reference found with instance ID '{instanceID}' in '{elementType}' category.");
        }
        return reference?.uiElement;
    }


    // Get a UI reference by element type, full hierarchical path, and instance ID
    public GameObject GetUIReference(string elementType, string fullPath, string instanceID)
    {
        UICategory category = uiCategories.Find(cat => cat.name == elementType);
        if (category == null)
        {
            Debug.LogWarning($"UIManager: Category '{elementType}' not found.");
            return null;
        }

        UIReference reference = category.references.Find(uiRef =>
            uiRef.fullPath == fullPath && uiRef.instanceID == instanceID);
        if (reference == null)
        {
            Debug.LogWarning($"UIManager: No reference found with path '{fullPath}' and instance ID '{instanceID}' in '{elementType}' category.");
        }
        return reference?.uiElement;
    }



    private UIElementType DetermineUIElementType(GameObject uiElement)
    {
        if (uiElement.GetComponent<Button>() != null) return UIElementType.Button;
        if (uiElement.GetComponent<Text>() != null) return UIElementType.Text;
        if (uiElement.GetComponent<Toggle>() != null) return UIElementType.Toggle;
        if (uiElement.GetComponent<InputField>() != null) return UIElementType.InputField;
        if (uiElement.GetComponent<Slider>() != null) return UIElementType.Slider;
        if (uiElement.GetComponent<Dropdown>() != null) return UIElementType.Dropdown;
        if (uiElement.GetComponent<ScrollRect>() != null) return UIElementType.ScrollView;

        // Check for Image or Panel based on suffix naming convention
        if (uiElement.GetComponent<Image>() != null)
        {
            if (uiElement.name.EndsWith("_Panel")) return UIElementType.Panel; // Using _Panel as a suffix for panels
            return UIElementType.Image;
        }

        if (uiElement.GetComponent<RawImage>() != null) return UIElementType.RawImage;
        if (uiElement.GetComponent<Mask>() != null) return UIElementType.Mask;
        if (uiElement.GetComponent<Canvas>() != null) return UIElementType.Canvas;
        if (uiElement.GetComponent<CanvasGroup>() != null) return UIElementType.CanvasGroup;

        // Default to Unknown if no component matches
        return UIElementType.Unknown;
    }

    // Get all UI categories
    public List<UICategory> GetAllUICategories()
    {
        return uiCategories;
    }

    #endregion ____________________UI Reference Management____________________

    #region ____________________Getting and Setting Particular UI Element ____________________

    #region BUTTON
    public Button GetButton(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Button>();
    }

    public Button GetButton(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Button>();
    }

    public Button GetButton(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Button>();
    }

    public void SetButtonListener(string elementType, string fullPath, UnityAction action)
    {
        Button button = GetButton(elementType, fullPath);
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Button not found or is not a Button type.");
        }
    }

    public void SetButtonListener(string elementType, string instanceID, bool isInstanceID, UnityAction action)
    {
        Button button = GetButton(elementType, instanceID, isInstanceID);
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Button not found or is not a Button type.");
        }
    }

    public void SetButtonListener(string elementType, string fullPath, string instanceID, UnityAction action)
    {
        Button button = GetButton(elementType, fullPath, instanceID);
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Button not found or is not a Button type.");
        }
    }



    #endregion BUTTON

    #region TEXT

    public Text GetText(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Text>();
    }

    public Text GetText(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Text>();
    }

    public Text GetText(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Text>();
    }

    public void SetTextContent(string elementType, string fullPath, string content)
    {
        Text textComponent = GetText(elementType, fullPath);
        if (textComponent != null)
        {
            textComponent.text = content;
        }
        else
        {
            Debug.LogWarning("UIManager: Text component not found or is not a Text type.");
        }
    }

    public void SetTextContent(string elementType, string instanceID, bool isInstanceID, string content)
    {
        Text textComponent = GetText(elementType, instanceID, isInstanceID);
        if (textComponent != null)
        {
            textComponent.text = content;
        }
        else
        {
            Debug.LogWarning("UIManager: Text component not found or is not a Text type.");
        }
    }

    public void SetTextContent(string elementType, string fullPath, string instanceID, string content)
    {
        Text textComponent = GetText(elementType, fullPath, instanceID);
        if (textComponent != null)
        {
            textComponent.text = content;
        }
        else
        {
            Debug.LogWarning("UIManager: Text component not found or is not a Text type.");
        }
    }

    #endregion TEXT

    #region TOGGLE

    public Toggle GetToggle(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Toggle>();
    }

    public Toggle GetToggle(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Toggle>();
    }

    public Toggle GetToggle(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Toggle>();
    }

    public void SetToggleListener(string elementType, string fullPath, UnityAction<bool> action)
    {
        Toggle toggle = GetToggle(elementType, fullPath);
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Toggle not found or is not a Toggle type.");
        }
    }

    public void SetToggleListener(string elementType, string instanceID, bool isInstanceID, UnityAction<bool> action)
    {
        Toggle toggle = GetToggle(elementType, instanceID, isInstanceID);
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Toggle not found or is not a Toggle type.");
        }
    }

    public void SetToggleListener(string elementType, string fullPath, string instanceID, UnityAction<bool> action)
    {
        Toggle toggle = GetToggle(elementType, fullPath, instanceID);
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Toggle not found or is not a Toggle type.");
        }
    }

    #endregion TOGGLE

    #region INPUT FIELD

    public InputField GetInputField(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<InputField>();
    }

    public InputField GetInputField(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<InputField>();
    }

    public InputField GetInputField(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<InputField>();
    }

    public void SetInputFieldListener(string elementType, string fullPath, UnityAction<string> action)
    {
        InputField inputField = GetInputField(elementType, fullPath);
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: InputField not found or is not an InputField type.");
        }
    }

    public void SetInputFieldListener(string elementType, string instanceID, bool isInstanceID, UnityAction<string> action)
    {
        InputField inputField = GetInputField(elementType, instanceID, isInstanceID);
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: InputField not found or is not an InputField type.");
        }
    }

    public void SetInputFieldListener(string elementType, string fullPath, string instanceID, UnityAction<string> action)
    {
        InputField inputField = GetInputField(elementType, fullPath, instanceID);
        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: InputField not found or is not an InputField type.");
        }
    }


    #endregion INPUT FIELD

    #region SLIDER

    public Slider GetSlider(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Slider>();
    }

    public Slider GetSlider(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Slider>();
    }

    public Slider GetSlider(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Slider>();
    }

    public void SetSliderListener(string elementType, string fullPath, UnityAction<float> action)
    {
        Slider slider = GetSlider(elementType, fullPath);
        if (slider != null)
        {
            slider.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Slider not found or is not a Slider type.");
        }
    }

    public void SetSliderListener(string elementType, string instanceID, bool isInstanceID, UnityAction<float> action)
    {
        Slider slider = GetSlider(elementType, instanceID, isInstanceID);
        if (slider != null)
        {
            slider.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Slider not found or is not a Slider type.");
        }
    }

    public void SetSliderListener(string elementType, string fullPath, string instanceID, UnityAction<float> action)
    {
        Slider slider = GetSlider(elementType, fullPath, instanceID);
        if (slider != null)
        {
            slider.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Slider not found or is not a Slider type.");
        }
    }

    #endregion SLIDER

    #region DROP DOWN

    public Dropdown GetDropdown(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Dropdown>();
    }

    public Dropdown GetDropdown(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Dropdown>();
    }

    public Dropdown GetDropdown(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Dropdown>();
    }

    public void SetDropdownListener(string elementType, string fullPath, UnityAction<int> action)
    {
        Dropdown dropdown = GetDropdown(elementType, fullPath);
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Dropdown not found or is not a Dropdown type.");
        }
    }

    public void SetDropdownListener(string elementType, string instanceID, bool isInstanceID, UnityAction<int> action)
    {
        Dropdown dropdown = GetDropdown(elementType, instanceID, isInstanceID);
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Dropdown not found or is not a Dropdown type.");
        }
    }

    public void SetDropdownListener(string elementType, string fullPath, string instanceID, UnityAction<int> action)
    {
        Dropdown dropdown = GetDropdown(elementType, fullPath, instanceID);
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: Dropdown not found or is not a Dropdown type.");
        }
    }



    #endregion DROP DOWN

    #region SCROLL VIEW

    public ScrollRect GetScrollView(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<ScrollRect>();
    }

    public ScrollRect GetScrollView(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<ScrollRect>();
    }

    public ScrollRect GetScrollView(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<ScrollRect>();
    }

    public void SetScrollViewListener(string elementType, string fullPath, UnityAction<Vector2> action)
    {
        ScrollRect scrollRect = GetScrollView(elementType, fullPath);
        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: ScrollView not found or is not a ScrollRect type.");
        }
    }

    public void SetScrollViewListener(string elementType, string instanceID, bool isInstanceID, UnityAction<Vector2> action)
    {
        ScrollRect scrollRect = GetScrollView(elementType, instanceID, isInstanceID);
        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: ScrollView not found or is not a ScrollRect type.");
        }
    }

    public void SetScrollViewListener(string elementType, string fullPath, string instanceID, UnityAction<Vector2> action)
    {
        ScrollRect scrollRect = GetScrollView(elementType, fullPath, instanceID);
        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(action);
        }
        else
        {
            Debug.LogWarning("UIManager: ScrollView not found or is not a ScrollRect type.");
        }
    }

    #endregion SCROLL VIEW

    #region IMAGE

    public Image GetImage(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Image>();
    }

    public Image GetImage(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Image>();
    }

    public Image GetImage(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Image>();
    }

    public void SetImageSprite(string elementType, string fullPath, Sprite newSprite)
    {
        Image image = GetImage(elementType, fullPath);
        if (image != null)
        {
            image.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("UIManager: Image not found or is not an Image type.");
        }
    }

    public void SetImageSprite(string elementType, string instanceID, bool isInstanceID, Sprite newSprite)
    {
        Image image = GetImage(elementType, instanceID, isInstanceID);
        if (image != null)
        {
            image.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("UIManager: Image not found or is not an Image type.");
        }
    }

    public void SetImageSprite(string elementType, string fullPath, string instanceID, Sprite newSprite)
    {
        Image image = GetImage(elementType, fullPath, instanceID);
        if (image != null)
        {
            image.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("UIManager: Image not found or is not an Image type.");
        }
    }

    public void SetImageColor(string elementType, string fullPath, Color newColor)
    {
        Image image = GetImage(elementType, fullPath);
        if (image != null)
        {
            image.color = newColor;
        }
        else
        {
            Debug.LogWarning("UIManager: Image not found or is not an Image type.");
        }
    }

    // Set Image color using instance ID
    public void SetImageColor(string elementType, string instanceID, bool isInstanceID, Color newColor)
    {
        Image image = GetImage(elementType, instanceID, isInstanceID);
        if (image != null)
        {
            image.color = newColor;
        }
        else
        {
            Debug.LogWarning($"UIManager: Image not found or is not an Image type for Instance ID '{instanceID}'.");
        }
    }

    // Set Image color using full hierarchical path and instance ID
    public void SetImageColor(string elementType, string fullPath, string instanceID, Color newColor)
    {
        Image image = GetImage(elementType, fullPath, instanceID);
        if (image != null)
        {
            image.color = newColor;
        }
        else
        {
            Debug.LogWarning($"UIManager: Image not found or is not an Image type for path '{fullPath}' and Instance ID '{instanceID}'.");
        }
    }

    #endregion IMAGE

    #region RAW IMAGE

    public RawImage GetRawImage(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<RawImage>();
    }

    public RawImage GetRawImage(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<RawImage>();
    }

    public RawImage GetRawImage(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<RawImage>();
    }

    public void SetRawImageTexture(string elementType, string fullPath, Texture newTexture)
    {
        RawImage rawImage = GetRawImage(elementType, fullPath);
        if (rawImage != null)
        {
            rawImage.texture = newTexture;
        }
        else
        {
            Debug.LogWarning("UIManager: RawImage not found or is not a RawImage type.");
        }
    }

    public void SetRawImageTexture(string elementType, string instanceID, bool isInstanceID, Texture newTexture)
    {
        RawImage rawImage = GetRawImage(elementType, instanceID, isInstanceID);
        if (rawImage != null)
        {
            rawImage.texture = newTexture;
        }
        else
        {
            Debug.LogWarning("UIManager: RawImage not found or is not a RawImage type.");
        }
    }

    public void SetRawImageTexture(string elementType, string fullPath, string instanceID, Texture newTexture)
    {
        RawImage rawImage = GetRawImage(elementType, fullPath, instanceID);
        if (rawImage != null)
        {
            rawImage.texture = newTexture;
        }
        else
        {
            Debug.LogWarning("UIManager: RawImage not found or is not a RawImage type.");
        }
    }

    #endregion RAW IMAGE

    #region MASK

    public Mask GetMask(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Mask>();
    }

    public Mask GetMask(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Mask>();
    }

    public Mask GetMask(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Mask>();
    }

    public void SetMaskEnabled(string elementType, string fullPath, bool enabled)
    {
        Mask mask = GetMask(elementType, fullPath);
        if (mask != null)
        {
            mask.enabled = enabled;
        }
        else
        {
            Debug.LogWarning("UIManager: Mask not found or is not a Mask type.");
        }
    }

    public void SetMaskEnabled(string elementType, string instanceID, bool isInstanceID, bool enabled)
    {
        Mask mask = GetMask(elementType, instanceID, isInstanceID);
        if (mask != null)
        {
            mask.enabled = enabled;
        }
        else
        {
            Debug.LogWarning("UIManager: Mask not found or is not a Mask type.");
        }
    }

    public void SetMaskEnabled(string elementType, string fullPath, string instanceID, bool enabled)
    {
        Mask mask = GetMask(elementType, fullPath, instanceID);
        if (mask != null)
        {
            mask.enabled = enabled;
        }
        else
        {
            Debug.LogWarning("UIManager: Mask not found or is not a Mask type.");
        }
    }

    #endregion MASK

    #region CANVAS

    public Canvas GetCanvas(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<Canvas>();
    }

    public Canvas GetCanvas(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<Canvas>();
    }

    public Canvas GetCanvas(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<Canvas>();
    }

    public void SetCanvasRenderMode(string elementType, string fullPath, RenderMode renderMode)
    {
        Canvas canvas = GetCanvas(elementType, fullPath);
        if (canvas != null)
        {
            canvas.renderMode = renderMode;
        }
        else
        {
            Debug.LogWarning("UIManager: Canvas not found or is not a Canvas type.");
        }
    }

    public void SetCanvasRenderMode(string elementType, string instanceID, bool isInstanceID, RenderMode renderMode)
    {
        Canvas canvas = GetCanvas(elementType, instanceID, isInstanceID);
        if (canvas != null)
        {
            canvas.renderMode = renderMode;
        }
        else
        {
            Debug.LogWarning("UIManager: Canvas not found or is not a Canvas type.");
        }
    }

    public void SetCanvasRenderMode(string elementType, string fullPath, string instanceID, RenderMode renderMode)
    {
        Canvas canvas = GetCanvas(elementType, fullPath, instanceID);
        if (canvas != null)
        {
            canvas.renderMode = renderMode;
        }
        else
        {
            Debug.LogWarning("UIManager: Canvas not found or is not a Canvas type.");
        }
    }



    #endregion CANVAS

    #region CANVAS GROUP 

    public CanvasGroup GetCanvasGroup(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        return uiElement?.GetComponent<CanvasGroup>();
    }

    public CanvasGroup GetCanvasGroup(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        return uiElement?.GetComponent<CanvasGroup>();
    }

    public CanvasGroup GetCanvasGroup(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        return uiElement?.GetComponent<CanvasGroup>();
    }

    public void SetCanvasGroupAlpha(string elementType, string fullPath, float alpha)
    {
        CanvasGroup canvasGroup = GetCanvasGroup(elementType, fullPath);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            Debug.LogWarning("UIManager: CanvasGroup not found or is not a CanvasGroup type.");
        }
    }

    public void SetCanvasGroupAlpha(string elementType, string instanceID, bool isInstanceID, float alpha)
    {
        CanvasGroup canvasGroup = GetCanvasGroup(elementType, instanceID, isInstanceID);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            Debug.LogWarning("UIManager: CanvasGroup not found or is not a CanvasGroup type.");
        }
    }

    public void SetCanvasGroupAlpha(string elementType, string fullPath, string instanceID, float alpha)
    {
        CanvasGroup canvasGroup = GetCanvasGroup(elementType, fullPath, instanceID);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            Debug.LogWarning("UIManager: CanvasGroup not found or is not a CanvasGroup type.");
        }
    }

    #endregion CANVAS GROUP

    #region PANEL

    public GameObject GetPanel(string elementType, string fullPath)
    {
        var uiElement = GetUIReference(elementType, fullPath);
        if (uiElement != null && uiElement.GetComponent<Image>() != null)
        {
            return uiElement;
        }
        Debug.LogWarning("UIManager: Panel not found or does not have an Image component.");
        return null;
    }

    public GameObject GetPanel(string elementType, string instanceID, bool isInstanceID)
    {
        var uiElement = GetUIReference(elementType, instanceID, isInstanceID);
        if (uiElement != null && uiElement.GetComponent<Image>() != null)
        {
            return uiElement;
        }
        Debug.LogWarning("UIManager: Panel not found or does not have an Image component.");
        return null;
    }

    public GameObject GetPanel(string elementType, string fullPath, string instanceID)
    {
        var uiElement = GetUIReference(elementType, fullPath, instanceID);
        if (uiElement != null && uiElement.GetComponent<Image>() != null)
        {
            return uiElement;
        }
        Debug.LogWarning("UIManager: Panel not found or does not have an Image component.");
        return null;
    }

    public void SetPanelColor(string elementType, string fullPath, Color color)
    {
        var panel = GetPanel(elementType, fullPath);
        if (panel != null)
        {
            var image = panel.GetComponent<Image>();
            if (image != null) image.color = color;
        }
    }

    public void SetPanelColor(string elementType, string instanceID, bool isInstanceID, Color color)
    {
        var panel = GetPanel(elementType, instanceID, isInstanceID);
        if (panel != null)
        {
            var image = panel.GetComponent<Image>();
            if (image != null) image.color = color;
        }
    }

    public void SetPanelColor(string elementType, string fullPath, string instanceID, Color color)
    {
        var panel = GetPanel(elementType, fullPath, instanceID);
        if (panel != null)
        {
            var image = panel.GetComponent<Image>();
            if (image != null) image.color = color;
        }
    }

    #endregion PANEL

    #endregion ____________________Getting and Setting Particular UI Element ____________________

    #region ____________________ PANEL Activation and Deactivation ____________________

    #region ACTIVATE PANEL


    private GameObject lastActivePanel = null;
    private List<GameObject> activePanels = new List<GameObject>();

    public void ActivatePanel(string elementType, string fullPath, bool deactivateOthers = false, bool keepLastPanel = false)
    {
        GameObject panelToActivate = GetPanel(elementType, fullPath);
        if (panelToActivate == null)
        {
            Debug.LogWarning("UIManager: Panel not found for activation.");
            return;
        }

        HandlePanelActivation(panelToActivate, deactivateOthers, keepLastPanel);
    }

    public void ActivatePanel(string elementType, string instanceID, bool isInstanceID, bool deactivateOthers = false, bool keepLastPanel = false)
    {
        var panel = GetPanel(elementType, instanceID, isInstanceID);
        if (panel != null)
        {
            HandlePanelActivation(panel, deactivateOthers, keepLastPanel);
        }
        else
        {
            Debug.LogWarning("UIManager: Panel not found for activation.");
        }
    }

    public void ActivatePanel(string elementType, string fullPath, string instanceID, bool deactivateOthers = false, bool keepLastPanel = false)
    {
        var panel = GetPanel(elementType, fullPath, instanceID);
        if (panel != null)
        {
            HandlePanelActivation(panel, deactivateOthers, keepLastPanel);
        }
        else
        {
            Debug.LogWarning("UIManager: Panel not found for activation.");
        }
    }

    private void HandlePanelActivation(GameObject panel, bool deactivateOthers, bool keepLastPanel)
    {
        if (deactivateOthers)
        {
            foreach (var activePanel in activePanels)
            {
                if (activePanel != panel && (!keepLastPanel || activePanel != lastActivePanel))
                {
                    activePanel.SetActive(false);
                }
            }
            activePanels.Clear();
            if (keepLastPanel && lastActivePanel != null && lastActivePanel != panel)
            {
                activePanels.Add(lastActivePanel);
            }
        }

        panel.SetActive(true);
        activePanels.Add(panel);
        lastActivePanel = panel;
    }

    #endregion ACTIVATE PANEL

    #region DEAVTIVATE PANEL

    public void DeactivatePanel(string elementType, string fullPath)
    {
        GameObject panel = GetPanel(elementType, fullPath);
        if (panel != null)
        {
            panel.SetActive(false);
            UpdateActivePanelsList(panel, false);
        }
        else
        {
            Debug.LogWarning("UIManager: Panel not found for deactivation.");
        }
    }

    public void DeactivatePanel(string elementType, string instanceID, bool isInstanceID)
    {
        GameObject panel = GetPanel(elementType, instanceID, isInstanceID);
        if (panel != null)
        {
            panel.SetActive(false);
            UpdateActivePanelsList(panel, false);
        }
        else
        {
            Debug.LogWarning("UIManager: Panel not found for deactivation.");
        }
    }

    public void DeactivatePanel(string elementType, string fullPath, string instanceID)
    {
        GameObject panel = GetPanel(elementType, fullPath, instanceID);
        if (panel != null)
        {
            panel.SetActive(false);
            UpdateActivePanelsList(panel, false);
        }
        else
        {
            Debug.LogWarning("UIManager: Panel not found for deactivation.");
        }
    }

    private void UpdateActivePanelsList(GameObject panel, bool isActive)
    {
        if (isActive)
        {
            if (!activePanels.Contains(panel))
            {
                activePanels.Add(panel);
            }
        }
        else
        {
            if (activePanels.Contains(panel))
            {
                activePanels.Remove(panel);
            }
        }

        // Update last active panel if necessary
        if (!isActive && lastActivePanel == panel)
        {
            lastActivePanel = null;
        }
    }


    #endregion DEACTIVATE PANEL

    #endregion ____________________ PANEL Activation and Deactivation ____________________

}
