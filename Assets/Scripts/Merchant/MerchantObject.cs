using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//THIS IS A BASE CLASS, not to be added directly 
public class MerchantObject : MonoBehaviour, IInteractable
{
    public delegate void UpdateObjectsPrice(MerchantObject merchantObj);
    public static UpdateObjectsPrice OnSuccessfullyInteracted;
    [SerializeField] Sprite sprite;
    int fishesRequired;
    protected virtual void Start() { }

    public virtual InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        InteractionResult result = new InteractionResult(false, 0.0f);
        if (playerResources.ModifyFishAmount(-fishesRequired))
        {
            OnSuccessfullyInteracted.Invoke(this);
            GiveObjectToPlayer(playerResources);
            result.interactionSuccessful = true;
        }
        return result;
    }

    protected virtual void GiveObjectToPlayer(PlayerResources playerResources) {}

    public void SetPrice(int newFishesRequired)
    {
        fishesRequired = newFishesRequired;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
