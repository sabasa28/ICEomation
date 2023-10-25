using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IInteractable
{
    [SerializeField] bool startLevelAddedToGrid = false;
    [SerializeField] int currentWaterHeld = 0;
    [SerializeField] int maxWater = 4;
    [SerializeField] float timeToGrabWater = 2.0f;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (startLevelAddedToGrid)
        {
            InteractingGrid.Get().OccupyPosition(this, transform.position);
        }
    }

    public InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        InteractionResult result = new InteractionResult(false, 0.0f);
        switch (playerResources.GetBucketState())
        {
            case PlayerResources.BucketState.filledWater:
                if (AttemptModifyWaterHeld(true))
                {
                    playerResources.SetBucketState(PlayerResources.BucketState.empty);
                    result.interactionSuccessful = true;
                    return result;
                }
                break;
            case PlayerResources.BucketState.empty:
                if (AttemptModifyWaterHeld(false, timeToGrabWater))//cuando hagamos nullable playerresources por los drones se cambiaria esto dependiendo de si es null
                {
                    playerResources.SetBucketState(PlayerResources.BucketState.filledWater, timeToGrabWater);
                    result.interactionSuccessful = true;
                    result.interactionTime = timeToGrabWater;
                    return result;
                }
                break;
        }
        return result;
    }

    bool AttemptModifyWaterHeld(bool shouldAdd, float timeBeforeModifying = 0.0f)
    {
        if (shouldAdd)
        {
            if (currentWaterHeld >= maxWater)
            {
                return false;
            }
            currentWaterHeld++;
            animator.SetTrigger("Fill");
        }
        else
        {
            if (currentWaterHeld <= 0)
            {
                return false;
            }
            if (timeBeforeModifying > 0)
            {
                StartCoroutine(RemoveWaterAfterTime(timeBeforeModifying));
                return true;
            }
            currentWaterHeld--;
            animator.SetTrigger("Empty");
        }
        return true;
    }

    IEnumerator RemoveWaterAfterTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        currentWaterHeld--;
        animator.SetTrigger("Empty");
    }
}
