using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] Animator bucketAnimator;
    [SerializeField] TextMeshProUGUI woodText;
    [SerializeField] TextMeshProUGUI metalText;
    [SerializeField] TextMeshProUGUI fishText;
    [SerializeField] int wood;
    [SerializeField] int metal;
    [SerializeField] int fish;
    [SerializeField] float bucketTimeToGrabWater;

    public enum BucketState
    { 
        filledIce,
        filledWater,
        empty
    }
    [SerializeField] BucketState bucketState;

    private void Start()
    {
        woodText.text = wood.ToString();
        metalText.text = metal.ToString();
        fishText.text = fish.ToString();
    }

    public InteractionResult InteractBucketToWater()
    {
        InteractionResult result = new InteractionResult(true, 0.0f);
        switch (bucketState)
        {
            case BucketState.filledIce:
                IceIceBabyManager.Get().ChangeSize(true);
                SetBucketState(BucketState.empty);
                break;
            case BucketState.filledWater:
                SetBucketState(BucketState.empty);
                break;
            case BucketState.empty:
                SetBucketState(BucketState.filledWater, bucketTimeToGrabWater);
                result.interactionTime = bucketTimeToGrabWater;
                break;
        }
        return result;
    }
    public void SetBucketState(BucketState newBucketState, float timeBeforeStateChange = 0.0f)
    {
        bucketState = newBucketState;
        if (timeBeforeStateChange > 0.0f)
        {
            StartCoroutine(SetBucketStateAfterTime(newBucketState, timeBeforeStateChange));
            return;
        }
        InternalSetBucketState(newBucketState);
    }

    private void InternalSetBucketState(BucketState newBucketState)
    {
        switch (bucketState)
        {
            case BucketState.filledIce:
                bucketAnimator.SetTrigger("IceFilled");
                break;
            case BucketState.filledWater:
                bucketAnimator.SetTrigger("waterFilled");
                break;
            case BucketState.empty:
                bucketAnimator.SetTrigger("Empty");
                break;
        }
    }

    public BucketState GetBucketState()
    {
        return bucketState;
    }
    public bool ModifyWoodAmount(int AmountToAdd)
    {
        if ((wood + AmountToAdd) < 0)
        {
            return false;
        }
        else
        {
            wood += AmountToAdd;
            woodText.text = wood.ToString();
            return true;
        }
    }

    public bool ModifyMetalAmount(int AmountToAdd)
    {
        if ((metal + AmountToAdd) < 0)
        {
            return false;
        }
        else
        {
            metal += AmountToAdd;
            metalText.text = metal.ToString();
            return true;
        }
    }

    public bool ModifyWoodAndMetalAmounts(int AmountToAddMetal, int AmountToAddWood)
    {
        if (metal + AmountToAddMetal >= 0 && wood + AmountToAddWood >= 0)
        {
            ModifyMetalAmount(AmountToAddMetal);
            ModifyWoodAmount(AmountToAddWood);
            return true;
        }
        return false;
    }

    public bool ModifyFishAmount(int AmountToAdd)
    {
        if ((fish + AmountToAdd) < 0)
        {
            return false;
        }
        else
        {
            fish += AmountToAdd;
            fishText.text = fish.ToString();
            return true;
        }
    }

    IEnumerator SetBucketStateAfterTime(BucketState newBucketState, float timeBeforeStateSet)
    {
        yield return new WaitForSeconds(timeBeforeStateSet);
        InternalSetBucketState(newBucketState);
    }
}
