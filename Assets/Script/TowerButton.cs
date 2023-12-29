using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : singleTon<TowerButton> {
    [SerializeField]
    private GameObject towerPrefab;

    public Sprite Sprite;

    public int price;

    public Text priceText;

    public bool isTower;

    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    public void Start()
    {
        priceText.text = price + "$";

        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }

    private void PriceCheck()
    {
        if(price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceText.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceText.color = Color.grey;
        }
    }

    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;

        switch (type)
        {
            case "Fire":
                FireTower fire = towerPrefab.GetComponentInChildren<FireTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Fire</b></size></color>" +
                                        "\nDamage: {0} \nProc:{1}%\nDebuff duration: {2}sec \nTick time: {3}sec", fire.damage, fire.proc, fire.debuffDuration, fire.TickTime);
                break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Poison</b></size></color>" +
                    "\nDamage: {0} \nProc:{1}%\nDebuff duration: {2}sec \nTick time: {3}sec", poison.damage, poison.proc, poison.debuffDuration, poison.TickTime);
                break;
            case "Frost":
                FrostTower frost = towerPrefab.GetComponentInChildren<FrostTower>();
                tooltip = string.Format("<color=#00ff00ff><size=20><b>Frost</b></size></color>\nDamage: {0} \nProc:{1}%\nDebuff duration: {2}sec \nSlowing factor: {3}% \nHas change to slow the target", frost.damage, frost.proc, frost.debuffDuration, frost.slowingFactor);
                break;
            case "Storm":
                StormTower storm = towerPrefab.GetComponentInChildren<StormTower>();
                tooltip = string.Format("<color=#add8e6ff><size=20><b>Strom</b></size></color>" +
                    " \nDamage: {0} \nProc: {1}%\nDebuff duration: {2}sec", storm.damage, storm.proc, storm.debuffDuration);
                break;
            case "block":
                tooltip = string.Format("<color=#add8e6ff><size=20><b>Block</b></size></color>\nStop enemy to pass");
                break;

        }
        GameManager.Instance.SetTooltipText(tooltip);
        GameManager.Instance.ShowStats();
    }
}
