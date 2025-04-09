using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MerchantObjectMaterial : MerchantObject
{
    enum ResourceToGive
    {
        wood,
        metal,
        stone
    }
    [SerializeField] ResourceToGive resourceToGive;
    [SerializeField] int amountOfResource;
    [SerializeField] TextMeshProUGUI resourceText;
    protected override void Start()
    {
        InteractingGrid.Get().OccupyPosition(this, transform.position);
        resourceText.text = amountOfResource.ToString();
    }

    protected override void GiveObjectToPlayer(PlayerResources playerResources)
    {
        switch (resourceToGive)
        {
            case ResourceToGive.wood:
                playerResources.ModifyWoodAmount(amountOfResource);
                break;
            case ResourceToGive.metal:
                playerResources.ModifyMetalAmount(amountOfResource);
                break;
            case ResourceToGive.stone:
                playerResources.ModifyStoneAmount(amountOfResource);
                break;
        }
    }
}
