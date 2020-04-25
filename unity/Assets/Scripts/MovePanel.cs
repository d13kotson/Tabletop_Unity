﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePanel : MonoBehaviour
{
    void Start()
    {
        
    }

    internal void Set(Attack attack)
    {
        Transform content = this.gameObject.transform;
        content.Find("Name").gameObject.GetComponent<Text>().text = attack.name;
        content.Find("Type").gameObject.GetComponent<Text>().text = attack.type;
        content.Find("Frequency").gameObject.GetComponent<Text>().text = attack.frequency;
        content.Find("AC").gameObject.GetComponent<Text>().text = attack.ac.ToString();
        content.Find("DB").gameObject.GetComponent<Text>().text = attack.damage_base.ToString();
        content.Find("Class").gameObject.GetComponent<Text>().text = attack.attack_class;
        content.Find("Range").gameObject.GetComponent<Text>().text = attack.range;
        content.Find("Effect").gameObject.GetComponent<Text>().text = attack.effect;
    }
}
