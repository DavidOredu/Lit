using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// for displaying error messages
/// Just add the ErrorMessage prefab to the scene
/// and call ErrorDisplayer.DIsplayMessage(msg) from any script in case of an error
/// </summary>
public class ErrorDisplayer : MonoBehaviour
{
    public TextMeshProUGUI errorMessage;
    private bool active = false;

    public static ErrorDisplayer instance;

    private void Awake() => instance = this;

    /// <summary>
    /// displays error message
    /// </summary>
    public void DisplayMessage(string message)
    {
        StartCoroutine(displayErrorMessage(message));
    }

    private IEnumerator displayErrorMessage(string msg)
    {
        if (active) yield break;
        active = true;
        errorMessage.text = msg;
        yield return new WaitForSeconds(2);
        errorMessage.text = "";
        active = false;
    }
}