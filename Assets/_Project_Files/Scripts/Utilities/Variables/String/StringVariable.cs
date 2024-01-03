using UnityEngine;

[CreateAssetMenu(menuName = "Custom/String Variable")]
public class StringVariable : ScriptableObject
{
    [SerializeField]
    [Tooltip("The current string value.")]
    [TextArea(3, 10)]
    private string value = "";

    public string Value
    {
        get { return value; }
        set { this.value = value; }
    }

    public void SetValue(string newValue)
    {
        value = newValue;
    }
}
