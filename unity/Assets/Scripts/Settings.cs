using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    GameController controller;

    private void Awake()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void LogOut()
    {
        this.controller.LogOut();
    }

    public void CharacterSelect()
    {
        this.controller.CharacterSelect();
    }

    public void Login()
    {
        this.controller.Login();
    }
}
