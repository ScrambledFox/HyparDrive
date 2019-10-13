using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileNameSanitiser : MonoBehaviour {

    TMP_InputField inputField;

    private void Awake () {
        inputField = GetComponent<TMP_InputField>();
    }

    public void CheckFileNameInput () {
        string text = inputField.text;

        text = text.Replace(" ", "");
        text = text.Replace(".", "");
        text = text.Replace("/", "");
        text = text.Replace("\\", "");
        text = text.Replace(" ", "");

        inputField.text = text;
    }

}