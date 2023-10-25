using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Merchant : MonoBehaviour
{
    [SerializeField] MerchantObject[] ObjectsForSale;
    [SerializeField] int startingPrice = 5;
    [SerializeField] int inflation = 2;
    [SerializeField] TextMeshProUGUI currentPriceText;

    int currentPrice;
    private void OnEnable()
    {
        MerchantObject.SetUpdatedObjectPrice += UpdatePrices;
    }

    private void OnDisable()
    {
        MerchantObject.SetUpdatedObjectPrice -= UpdatePrices;
    }

    private void Start()
    {
        currentPrice = startingPrice;
        UpdateObjectsPrices();
    }

    public void UpdatePrices()
    {
        if (inflation == 0) return; 
        currentPrice += inflation;
        UpdateObjectsPrices();
    }

    public void UpdateObjectsPrices()
    {
        currentPriceText.text = currentPrice.ToString() + "X";
        foreach (MerchantObject objForSale in ObjectsForSale)
        {
            objForSale.SetPrice(currentPrice);
        }
    }

}
