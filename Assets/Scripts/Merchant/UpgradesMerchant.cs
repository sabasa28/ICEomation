using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradesMerchant : Merchant
{
    [SerializeField] GameObject upgradesMenu;
    bool showingUpgrades = false;
    List<int> buyableUpgrades = new List<int>();

    public Action OnUpgradeSelected;
    

    protected override void Start()
    {
        base.Start();
        currentPrice = startingPrice;
        SetBuyableUpgrades();
    }
    public override InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        InteractionResult result = new InteractionResult(false, 0.0f);
        if (playerResources.ModifyFishAmount(currentPrice))
        {
            UpdatePrices();
            ChangeUpgradesMenuVisibility(true);
            OnUpgradeSelected = playerResources.OnUpgradeAdded;
        }
        return result;
    }

    protected override void OnPlayerInteractsWithObj(MerchantObject merchantObj)
    {
        for (int i = 0; i < buyableUpgrades.Count; i++)
        {
            ObjectsForSale[buyableUpgrades[i]].gameObject.SetActive(false);
            ObjectsForSale[buyableUpgrades[i]].GetComponent<RectTransform>().SetParent(transform);
        }
        ObjectsForSale.Remove(merchantObj);
        SetBuyableUpgrades();
        ChangeUpgradesMenuVisibility(false);
        OnUpgradeSelected.Invoke();
    }

    private void UpdatePrices()
    {
        currentPrice += inflation;
        currentPriceText.text = currentPrice.ToString() + "X";
    }

    void ChangeUpgradesMenuVisibility(bool visible)
    {
        Time.timeScale = visible ? 0 : 1;
        showingUpgrades = visible;

        upgradesMenu.SetActive(visible);
        EventSystem.current.SetSelectedGameObject(ObjectsForSale[buyableUpgrades[0]].gameObject);
    }

    private void Update()
    {
        if (showingUpgrades && !EventSystem.current.currentSelectedGameObject)
            EventSystem.current.SetSelectedGameObject(ObjectsForSale[buyableUpgrades[0]].gameObject);
    }

    void SetBuyableUpgrades()
    {
        UpdateBuyableUpgradesIndices();

        for (int i = 0; i < buyableUpgrades.Count; i++)
        {
            ObjectsForSale[buyableUpgrades[i]].GetComponent<RectTransform>().SetParent(upgradesMenu.GetComponent<RectTransform>());
            ObjectsForSale[buyableUpgrades[i]].gameObject.SetActive(true);
        }
    }

    void UpdateBuyableUpgradesIndices()
    {
        List<int> randomNums = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int aux = UnityEngine.Random.Range(0, ObjectsForSale.Count);
            
            int antiLoopHero = 0;
            while (randomNums.Contains(aux))
            {
                antiLoopHero++;
                if (antiLoopHero > 50)
                {
                    Debug.Log("There no more upgrades available");
                    break;
                }
                aux++;
                if (aux >= ObjectsForSale.Count) aux = 0;
            }
            randomNums.Add(aux);
        }
        buyableUpgrades = randomNums;
    }

}
