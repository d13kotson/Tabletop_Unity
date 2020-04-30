using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public GameObject SettingsPanel = default;
    private GameController controller = default;

    private void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void DisplaySettings()
    {
        this.controller.OpenScreens.Add(this.SettingsPanel);
        this.SettingsPanel.SetActive(true);
    }
}
