using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Inventory : Window
{
    public GameObject Item = default;
    public Transform viewPort = default;
    public Dropdown ItemSelect = default;
    public InputField ItemName = default;
    public InputField ItemNumber = default;
    private List<int> itemIDs = new List<int>();
    private List<GameObject> itemPanels = new List<GameObject>();

    private void Awake()
    {
        this.Disable();
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.Set();
        this.controller.RefreshActions.Add(() => this.Set());

        this.controller.SendGetRequest("api/items", (request) =>
        {
            ItemList list = JsonUtility.FromJson<ItemList>(string.Format("{{\"list\": {0}}}", request.downloadHandler.text));
            foreach (ItemStruct item in list.list)
            {
                this.itemIDs.Add(item.id);
                this.ItemSelect.options.Add(new Dropdown.OptionData() { text = item.name });
            }
        }, (request) =>
        {

        });
    }

    private void Set()
    {
        foreach(GameObject item in this.itemPanels)
        {
            Destroy(item);
        }
        this.itemPanels = new List<GameObject>();
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>();
        if (!this.controller.isGM)
        {
            int position = -25;
            foreach (TrainerItem item in this.controller.trainer.item)
            {
                position = this.addItem(position, item, delegate { });
            }
        }
    }

    private int addItem(int position, TrainerItem item, UnityAction function)
    {
        GameObject button = Instantiate(this.Item);
        this.itemPanels.Add(button);
        button.transform.SetParent(this.viewPort);
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = new Vector2(0, position);
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

    public void ItemSelection()
    {
        if(this.ItemSelect.value == 0)
        {
            this.ItemName.gameObject.SetActive(true);
        }
        else
        {
            this.ItemName.gameObject.SetActive(false);
        }
    }

    public void NewItem()
    {
        int inputID = this.ItemSelect.value;
        string itemName = this.ItemName.text;
        int itemNumber = int.Parse(this.ItemNumber.text);
        string data = "";
        if(inputID == 0)
        {
            data = string.Format("{{\"trainer\": {0}, \"item_name\": \"{1}\", \"number\": {2}}}", this.controller.trainer.id, itemName, itemNumber);
        }
        else
        {
            int itemID = this.itemIDs[inputID - 1];
            data = string.Format("{{\"trainer\": {0}, \"item\": {1}, \"number\": {2}}}", this.controller.trainer.id, itemID, itemNumber);
        }

        this.controller.SendPostRequest("api/addItem", data, (request) =>
        {
            this.controller.Reload();
        }, (request) =>
        {

        });
    }

    private void addItem(TrainerItem item)
    {
        this.controller.SendPostRequest(string.Format("api/item-add/{0}", item.id), " ", (request) =>
        {
            this.controller.Reload();
        }, (request) =>
        {

        });
    }

    private void subItem(TrainerItem item)
    {
        this.controller.SendPostRequest(string.Format("api/item-sub/{0}", item.id), " ", (request) =>
        {
            this.controller.Reload();
        }, (request) =>
        {

        });
    }
}
