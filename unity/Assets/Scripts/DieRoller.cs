using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieRoller : Window
{
    public InputField NumDie = default;
    public InputField DieNum = default;
    public InputField Add = default;

    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.Disable();
    }

    public void Roll()
    {
        int numDie = int.Parse(this.NumDie.text);
        int dieNum = int.Parse(this.DieNum.text);
        int add = int.Parse(this.Add.text);
        string user = string.Empty;
        if (this.controller.isGM)
        {
            user = "GM";
        }
        else
        {
            user = this.controller.trainer.name;
        }
        this.controller.socket.Roll(user, numDie, dieNum, add);
    }
}
