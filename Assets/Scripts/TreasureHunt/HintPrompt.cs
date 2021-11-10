using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintPrompt : MonoBehaviour {
    public delegate void ConfirmCallback();

    [SerializeField]
    private TextMeshProUGUI hintText;
    private ConfirmCallback callback;

    public void SetConfirmCallback(ConfirmCallback callback) {
        this.callback = callback;
    }

    public void ConfirmButton() {
        gameObject.SetActive(false);
        callback?.Invoke();
    }

    public void SetHintText(string hint) {
        hintText.text = hint;
        Debug.Log("Set hint text: " + hint);
    }
}
