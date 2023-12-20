using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set the text of a UI element using UIManager
        UIManager.Instance.SetTextContent("Text", "GreetingText", "Hello");

        UIManager.Instance.SetButtonListener("Button", "MyButton", () =>
        {
            Debug.Log("Button clicked!");
        });

        UIManager.Instance.SetToggleListener("Toggle", "MyToggle", (isOn) =>
        {
            Debug.Log("Toggle state changed to: " + isOn);
        });

        UIManager.Instance.SetInputFieldListener("InputField", "MyInputField", (value) =>
        {
            Debug.Log("InputField value changed to: " + value);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
