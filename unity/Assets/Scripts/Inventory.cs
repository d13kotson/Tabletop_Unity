using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject Item = default;
    public Transform viewPort = default;

    private GameController controller = default;

    private void Start()
    {
        this.Disable();
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>();
        if (!this.controller.isGM)
        {
            int position = -25;
            foreach (TrainerItem item in this.controller.trainer.item)
            {
                position = this.addItem(position, item, delegate {  });
            }
        }
    }

    private int addItem(int position, TrainerItem item, UnityAction function)
    {
        GameObject button = Instantiate(this.Item);
        button.transform.SetParent(this.viewPort);
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = new Vector2(0, position);
        buttonRect.sizeDelta = new Vector2(0, 40);
        if (string.IsNullOrEmpty(item.item.name))
        {
            button.transform.Find("Name").gameObject.GetComponent<Text>().text = item.item_name;
        }
        else
        {
            button.transform.Find("Name").gameObject.GetComponent<Text>().text = item.item.name;
        }
        button.transform.Find("Number").gameObject.GetComponent<Text>().text = item.number.ToString();
        button.transform.Find("Add").gameObject.GetComponent<Button>().onClick.AddListener(delegate { this.addItem(item); });
        button.transform.Find("Sub").gameObject.GetComponent<Button>().onClick.AddListener(delegate { this.subItem(item); });
        return position - 50;
    }

    private void addItem(TrainerItem item)
    {

    }

    private void subItem(TrainerItem item)
    {

    }

    public void Enable()
    {
        bool value = !this.gameObject.activeSelf;
        if (value)
        {
            this.controller.OpenScreens.Add(this.gameObject);
        }
        else
        {
            this.controller.OpenScreens.Remove(this.gameObject);
        }
        this.gameObject.SetActive(value);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
