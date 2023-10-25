using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MerchantObject : MonoBehaviour, IInteractable
{
    public delegate void UpdateObjectsPrice();
    public static UpdateObjectsPrice SetUpdatedObjectPrice;
    enum ResourceToGive
    {
        wood,
        metal
    }
    [SerializeField] ResourceToGive resourceToGive;
    [SerializeField] int amountOfResource;
    [SerializeField] TextMeshProUGUI resourceText;
    int fishesRequired;
    void Start()
    {
        InteractingGrid.Get().OccupyPosition(this, transform.position);
        resourceText.text = amountOfResource.ToString();
    }

    public InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        InteractionResult result = new InteractionResult(false, 0.0f);
        if (playerResources.ModifyFishAmount(-fishesRequired))
        {
            SetUpdatedObjectPrice.Invoke();
            switch (resourceToGive)
            {
                case ResourceToGive.wood:
                    playerResources.ModifyWoodAmount(amountOfResource);
                    break;
                case ResourceToGive.metal:
                    playerResources.ModifyMetalAmount(amountOfResource);
                    break;
            }
            result.interactionSuccessful = true;
        }
        return result;
    }

    public void SetPrice(int newFishesRequired)
    {
        fishesRequired = newFishesRequired;
    }
}
