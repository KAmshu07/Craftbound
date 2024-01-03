using UnityEngine;
using UnityEngine.UI;

public class TextReplacer : MonoBehaviour
{
    [Tooltip("The Text component to display the text.")]
    public Text Text;

    [Tooltip("The StringVariable to get text from.")]
    public StringVariable Variable;

    [Tooltip("Automatically update the text when the Variable changes.")]
    public bool AutoUpdate = true;

    private void OnEnable()
    {
        UpdateText();
    }

    private void Update()
    {
        if (AutoUpdate)
        {
            UpdateText();
        }
    }

    public void UpdateText()
    {
        if (Text != null && Variable != null)
        {
            Text.text = Variable.Value;
        }
    }
}
