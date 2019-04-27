using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject OptionPanel = null;

    private void Awake()
    {
        Object.DontDestroyOnLoad(this);
    }

    public void ToggleOptionMenu()
    {
        OptionPanel.SetActive(!OptionPanel.activeSelf);
    }
}
