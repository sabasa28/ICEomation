using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantObjectUpgrade : MerchantObject
{

    public void SelectUpgrade()
    {
        OnSuccessfullyInteracted.Invoke(this);
    }
    public override InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        InteractionResult result = new InteractionResult(false, 0.0f);
        if (true) //will be a check to see if he already has this upgrade
        {
            result.interactionSuccessful = true;
            GiveObjectToPlayer(playerResources);
            OnSuccessfullyInteracted.Invoke(this);
        }
        return result;
    }

    protected override void GiveObjectToPlayer(PlayerResources playerResources) 
    {
        //aca le daria el upgrade al player, si dicho upgrade existiese
    }
}
