using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PokemonLevelUp : Window
{
    public RectTransform MoveContent = default;
    public InputField ConstitutionInput = default;
    public InputField AttackInput = default;
    public InputField DefenseInput = default;
    public InputField SpecialAttackInput = default;
    public InputField SpecialDefenseInput = default;
    public InputField SpeedInput = default;
    public GameObject MovePanel = default;

    private GameObject statsTab = default;
    private GameObject movesTab = default;
    private List<GameObject> moves = new List<GameObject>();
    private List<int> attacks = new List<int>();

    private Pokemon pokemon;

    void Awake()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Transform content = this.gameObject.transform;
        this.statsTab = content.Find("StatsPanel").gameObject;
        this.movesTab = content.Find("Moves").gameObject;

        this.Disable();
        this.ShowStats();
    }

    internal void Set(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        int position = -50;
        List<int> knownAttacks = new List<int>();

        this.statsTab.transform.Find("BaseConstitution").gameObject.GetComponent<Text>().text = this.pokemon.species.base_constitution.ToString();
        this.statsTab.transform.Find("NatConstitution").gameObject.GetComponent<Text>().text = this.pokemon.nature.constitution.ToString();
        this.statsTab.transform.Find("Constitution").gameObject.GetComponent<Text>().text = this.pokemon.constitution.ToString();
        this.statsTab.transform.Find("TotConstitution").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_constitution + this.pokemon.nature.constitution + this.pokemon.constitution).ToString();

        this.statsTab.transform.Find("BaseAttack").gameObject.GetComponent<Text>().text = this.pokemon.species.base_attack.ToString();
        this.statsTab.transform.Find("NatAttack").gameObject.GetComponent<Text>().text = this.pokemon.nature.attack.ToString();
        this.statsTab.transform.Find("Attack").gameObject.GetComponent<Text>().text = this.pokemon.attack.ToString();
        this.statsTab.transform.Find("TotAttack").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_attack + this.pokemon.nature.attack + this.pokemon.attack).ToString();

        this.statsTab.transform.Find("BaseDefense").gameObject.GetComponent<Text>().text = this.pokemon.species.base_defense.ToString();
        this.statsTab.transform.Find("NatDefense").gameObject.GetComponent<Text>().text = this.pokemon.nature.defense.ToString();
        this.statsTab.transform.Find("Defense").gameObject.GetComponent<Text>().text = this.pokemon.defense.ToString();
        this.statsTab.transform.Find("TotDefense").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_defense + this.pokemon.nature.defense + this.pokemon.defense).ToString();

        this.statsTab.transform.Find("BaseSpAttack").gameObject.GetComponent<Text>().text = this.pokemon.species.base_special_attack.ToString();
        this.statsTab.transform.Find("NatSpAttack").gameObject.GetComponent<Text>().text = this.pokemon.nature.special_attack.ToString();
        this.statsTab.transform.Find("SpAttack").gameObject.GetComponent<Text>().text = this.pokemon.special_attack.ToString();
        this.statsTab.transform.Find("TotSpAttack").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_special_attack + this.pokemon.nature.special_attack + this.pokemon.special_attack).ToString();

        this.statsTab.transform.Find("BaseSpDefense").gameObject.GetComponent<Text>().text = this.pokemon.species.base_special_defense.ToString();
        this.statsTab.transform.Find("NatSpDefense").gameObject.GetComponent<Text>().text = this.pokemon.nature.special_defense.ToString();
        this.statsTab.transform.Find("SpDefense").gameObject.GetComponent<Text>().text = this.pokemon.special_defense.ToString();
        this.statsTab.transform.Find("TotSpDefense").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_special_defense + this.pokemon.nature.special_defense + this.pokemon.special_defense).ToString();

        this.statsTab.transform.Find("BaseSpeed").gameObject.GetComponent<Text>().text = this.pokemon.species.base_speed.ToString();
        this.statsTab.transform.Find("NatSpeed").gameObject.GetComponent<Text>().text = this.pokemon.nature.speed.ToString();
        this.statsTab.transform.Find("Speed").gameObject.GetComponent<Text>().text = this.pokemon.speed.ToString();
        this.statsTab.transform.Find("TotSpeed").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_speed + this.pokemon.nature.speed + this.pokemon.speed).ToString();

        foreach (PokemonAttack attack in pokemon.pokemon_attack)
        {
            GameObject movePanel = Instantiate(this.MovePanel);
            movePanel.transform.SetParent(this.MoveContent);
            RectTransform rect = movePanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, position);
            rect.sizeDelta = new Vector2(0, 120);
            movePanel.GetComponentInChildren<Text>().text = name;
            movePanel.GetComponent<MovePanel>().Set(attack.attack, pokemon.id, TokenType.pokemon);
            position -= 120;
            knownAttacks.Add(attack.attack.id);
            this.moves.Add(movePanel);
            this.attacks.Add(attack.attack.id);
        }
        this.controller.SendGetRequest(string.Format("api/pokemon/{0}/learnable-moves", pokemon.id), (request) =>
        {
            SpeciesAttackList attacks = JsonUtility.FromJson<SpeciesAttackList>(string.Format("{{\"list\": {0}}}", request.downloadHandler.text));
            foreach (SpeciesAttack attack in attacks.list.Where((a) => !knownAttacks.Contains(a.attack.id)))
            {
                GameObject movePanel = Instantiate(this.MovePanel);
                movePanel.transform.SetParent(this.MoveContent);
                RectTransform rect = movePanel.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, position);
                rect.sizeDelta = new Vector2(0, 120);
                movePanel.GetComponentInChildren<Text>().text = name;
                movePanel.GetComponent<MovePanel>().Set(attack.attack, pokemon.id, TokenType.pokemon);
                position -= 120;
                this.moves.Add(movePanel);
                this.attacks.Add(attack.attack.id);
            }
        }, (request) =>
        {

        });
        this.Enable();
    }

    public void LevelUp()
    {
        this.controller.SendGetRequest(string.Format("api/pokemon/{0}", this.pokemon.id), (pokemonRequest) =>
        {
            PokemonSimple pokemon = JsonUtility.FromJson<PokemonSimple>(pokemonRequest.downloadHandler.text);
            pokemon.constitution += int.Parse(this.ConstitutionInput.text);
            pokemon.attack += int.Parse(this.AttackInput.text);
            pokemon.defense += int.Parse(this.DefenseInput.text);
            pokemon.special_attack += int.Parse(this.SpecialAttackInput.text);
            pokemon.special_defense += int.Parse(this.SpecialDefenseInput.text);
            pokemon.speed += int.Parse(this.SpeedInput.text);
            this.controller.SendPutRequest(string.Format("api/pokemon/{0}", pokemon.id), JsonUtility.ToJson(pokemon), (levelRequest) => {}, (levelRequest) => {});
            List<int> newAttacks = new List<int>();
            for (int i = 0; i < this.moves.Count; i++)
            {
                if(this.moves[i].GetComponentInChildren<Toggle>().isOn)
                {
                    newAttacks.Add(this.attacks[i]);
                }
            }
            string data = string.Format("{{\"pokemon\": {0}, \"attacks\": [{1}]}}", pokemon.id, string.Join(",", newAttacks));
            this.controller.SendPostRequest("api/addPokemonAttack", data, (levelRequest) => { this.controller.Reload(); }, (levelRequest) => { });
            this.controller.Reload();
            Destroy(this.gameObject);
        }, (pokemonRequest) =>
        {

        });
    }

    public void UpdateConstitution(InputField input)
    {
        this.statsTab.transform.Find("TotConstitution").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_constitution + this.pokemon.nature.constitution + this.pokemon.constitution + int.Parse(input.text)).ToString();
    }

    public void UpdateAttack(InputField input)
    {
        this.statsTab.transform.Find("TotAttack").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_attack + this.pokemon.nature.attack + this.pokemon.attack + int.Parse(input.text)).ToString();
    }

    public void UpdateDefense(InputField input)
    {
        this.statsTab.transform.Find("TotDefense").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_defense + this.pokemon.nature.defense + this.pokemon.defense + int.Parse(input.text)).ToString();
    }

    public void UpdateSpecialAttack(InputField input)
    {
        this.statsTab.transform.Find("TotSpAttack").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_special_attack + this.pokemon.nature.special_attack + this.pokemon.special_attack + int.Parse(input.text)).ToString();
    }

    public void UpdateSpecialDefense(InputField input)
    {
        this.statsTab.transform.Find("TotSpDefense").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_special_defense + this.pokemon.nature.special_defense + this.pokemon.special_defense + int.Parse(input.text)).ToString();
    }

    public void UpdateSpeed(InputField input)
    {
        this.statsTab.transform.Find("TotSpeed").gameObject.GetComponent<Text>().text = (this.pokemon.species.base_speed + this.pokemon.nature.speed + this.pokemon.speed + int.Parse(input.text)).ToString();
    }

    public void ShowStats()
    {
        this.HideTabs();
        this.statsTab.SetActive(true);
    }

    public void ShowMoves()
    {
        this.HideTabs();
        this.movesTab.SetActive(true);
    }

    private void HideTabs()
    {
        this.statsTab.SetActive(false);
        this.movesTab.SetActive(false);
    }
}
