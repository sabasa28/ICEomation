using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Merchant : MonoBehaviour, IInteractable
{
    [SerializeField] protected List<MerchantObject> ObjectsForSale = new List<MerchantObject>();
    [SerializeField] protected int startingPrice = 5;
    [SerializeField] protected int inflation = 2;
    [SerializeField] protected TextMeshProUGUI currentPriceText;

    protected int currentPrice;

    protected void OnEnable()
    {
        MerchantObject.OnSuccessfullyInteracted += OnPlayerInteractsWithObj; //if at some point we want the merchant to appear in runtime we change this to be dependant of a bool
    }

    protected void OnDisable()
    {
        MerchantObject.OnSuccessfullyInteracted -= OnPlayerInteractsWithObj;
    }

    protected virtual void Start()
    {
        InteractingGrid.Get().OccupyPosition(this, transform.position);
        currentPrice = startingPrice;
        UpdateObjectsPrices();
    }

    protected virtual void OnPlayerInteractsWithObj(MerchantObject merchantObj)
    {
        UpdatePrices(merchantObj);
    }

    private void UpdatePrices(MerchantObject merchantObj)
    {
        if (inflation == 0) return;
        bool objIsInMerchantsInventory = false;
        foreach (MerchantObject obj in ObjectsForSale)
        {
            if (obj == merchantObj)
            {
                objIsInMerchantsInventory = true;
                continue;
            }
        }
        if (!objIsInMerchantsInventory) return;
        currentPrice += inflation;
        UpdateObjectsPrices();
    }

    private void UpdateObjectsPrices()
    {
        currentPriceText.text = currentPrice.ToString() + "X";
        foreach (MerchantObject objForSale in ObjectsForSale)
        {
            objForSale.SetPrice(currentPrice);
        }
    }

    public virtual InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        InteractionResult result = new InteractionResult(false, 0.0f);
        return result;
    }
}
