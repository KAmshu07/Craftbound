using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public void AddUIReference(string referenceName, GameObject uiElement)
    {
        UIElementType type = DetermineUIElementType(uiElement);
        string categoryName = type.ToString(); // Use element type as category name

        UICategory category = uiCategories.Find(cat => cat.name == categoryName);
        if (category == null)
        {
            category = new UICategory { name = categoryName };
            uiCategories.Add(category);
        }

        UIReference reference = new UIReference { name = referenceName, uiElement = uiElement, elementType = type };
        category.references.Add(reference);
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



    // Remove a UI reference by name and category
    public void RemoveUIReference(string categoryName, string referenceName)
    {
        UICategory category = uiCategories.Find(cat => cat.name == categoryName);
        if (category != null)
        {
            category.references.RemoveAll(reference => reference.name == referenceName);
        }
    }

    // Get a UI reference by name and category
    public GameObject GetUIReference(string categoryName, string referenceName)
    {
        UICategory category = uiCategories.Find(cat => cat.name == categoryName);
        if (category != null)
        {
            var reference = category.references.Find(uiRef => uiRef.name == referenceName);
            return reference != null ? reference.uiElement : null;
        }
        return null;
    }

    // Get all UI categories
    public List<UICategory> GetAllUICategories()
    {
        return uiCategories;
    }

    #region BUTTON GET and SET
    public Button GetButton(string categoryName, string referenceName)
    {
        GameObject uiObject = GetUIReference(categoryName, referenceName);
        if (uiObject != null)
        {
            Button buttonComponent = uiObject.GetComponent<Button>();
            if (buttonComponent != null)
            {
                return buttonComponent;
            }
            else
            {
                Debug.LogError("The UI element found does not have a Button component.");
            }
        }
        else
        {
            Debug.LogError("No UI element found with the specified reference name.");
        }

        return null; // Return null if the button couldn't be retrieved
    }

    public void SetButtonListener(string categoryName, string referenceName, UnityEngine.Events.UnityAction action)
    {
        Button button = GetButton(categoryName, referenceName);
        if (button != null)
        {
            button.onClick.RemoveAllListeners(); // Remove existing listeners to avoid duplication
            button.onClick.AddListener(action);
        }
    }

    #endregion BUTTON GET and SET

    #region TEXT GET and SET
    public string GetTextContent(string categoryName, string referenceName)
    {
        GameObject uiObject = GetUIReference(categoryName, referenceName);
        if (uiObject != null)
        {
            Text textComponent = uiObject.GetComponent<Text>();
            if (textComponent != null)
            {
                return textComponent.text;
            }
            else
            {
                Debug.LogError("The UI element found does not have a Text component.");
            }
        }
        else
        {
            Debug.LogError("No UI element found with the specified reference name.");
        }

        return null; // Return null if the text content couldn't be retrieved
    }


    public void SetTextContent(string categoryName, string referenceName, string content)
    {
        GameObject uiObject = GetUIReference(categoryName, referenceName);
        if (uiObject != null)
        {
            Text textComponent = uiObject.GetComponent<Text>();
            if (textComponent != null)
            {
                textComponent.text = content;
            }
            else
            {
                Debug.LogError("The UI element found does not have a Text component.");
            }
        }
        else
        {
            Debug.LogError("No UI element found with the specified reference name.");
        }
    }
    #endregion TEXT GET and SET

    #region TOGGLE GET and SET
    public Toggle GetToggle(string categoryName, string referenceName)
    {
        GameObject uiObject = GetUIReference(categoryName, referenceName);
        if (uiObject != null)
        {
            Toggle toggleComponent = uiObject.GetComponent<Toggle>();
            if (toggleComponent != null)
            {
                return toggleComponent;
            }
            else
            {
                Debug.LogError("The UI element found does not have a Toggle component.");
            }
        }
        else
        {
            Debug.LogError("No UI element found with the specified reference name.");
        }

        return null; // Return null if the toggle couldn't be retrieved
    }

    public void SetToggleListener(string categoryName, string referenceName, UnityEngine.Events.UnityAction<bool> action)
    {
        Toggle toggle = GetToggle(categoryName, referenceName);
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveAllListeners(); // Remove existing listeners to avoid duplication
            toggle.onValueChanged.AddListener(action);
        }
    }



    #endregion TOGGLE GET and SET

    #region INPUT FIELD GET and SET
    public InputField GetInputField(string categoryName, string referenceName)
    {
        GameObject uiObject = GetUIReference(categoryName, referenceName);
        if (uiObject != null)
        {
            InputField inputFieldComponent = uiObject.GetComponent<InputField>();
            if (inputFieldComponent != null)
            {
                return inputFieldComponent;
            }
            else
            {
                Debug.LogError("The UI element found does not have an InputField component.");
            }
        }
        else
        {
            Debug.LogError("No UI element found with the specified reference name.");
        }

        return null; // Return null if the InputField couldn't be retrieved
    }

    public void SetInputFieldListener(string categoryName, string referenceName, UnityEngine.Events.UnityAction<string> action)
    {
        InputField inputField = GetInputField(categoryName, referenceName);
        if (inputField != null)
        {
            inputField.onValueChanged.RemoveAllListeners(); // Remove existing listeners
            inputField.onValueChanged.AddListener(action);
        }
    }


    #endregion INPUT FIELD GET and SET


    #endregion ____________________UI Reference Management____________________

}