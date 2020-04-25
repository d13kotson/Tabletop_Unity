using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    private GameController controller;
    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.GetComponent<Button>().onClick.AddListener(delegate { this.controller.CloseScreens(); });
    }
}
