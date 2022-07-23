using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoger : MonoBehaviour
{
    static public UILoger instance;
    private void Awake() => instance = this;
    private void OnDestroy() => instance = null;

    TextMesh text;
    private void Start() => text = GetComponent<TextMesh>();
    static public void Log(string str) => instance.text.text = str;
}
