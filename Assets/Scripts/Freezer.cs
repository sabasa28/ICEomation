using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour, IInteractable
{
    [SerializeField] bool startLevelAddedToGrid = false;
    [SerializeField] float timeToMakeIce = 10.0f;
    enum FreezerState
    { 
        empty,
        waterIn,
        iceIn
    }
    [SerializeField] FreezerState state;
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
        switch (state)
        {
            case FreezerState.empty:
                if (playerResources.GetBucketState() == PlayerResources.BucketState.filledWater)
                {
                    SetState(FreezerState.waterIn);
                    playerResources.SetBucketState(PlayerResources.BucketState.empty);
                    result.interactionSuccessful = true;
                }
                break;
            case FreezerState.waterIn:
                break;
            case FreezerState.iceIn:
                if (playerResources.GetBucketState() == PlayerResources.BucketState.empty)
                {
                    SetState(FreezerState.empty);
                    playerResources.SetBucketState(PlayerResources.BucketState.filledIce);
                    result.interactionSuccessful = true;
                }
                break;
        }
        return result;

    }

    void SetState(FreezerState newState)
    {
        state = newState;
        switch (state)
        {
            case FreezerState.empty:
                animator.SetTrigger("Empty");
                break;
            case FreezerState.waterIn:
                animator.SetTrigger("MakingIce");
                StartCoroutine(MakeIce());
                break;
            case FreezerState.iceIn:
                animator.SetTrigger("IceReady");
                break;
        }
    }

    IEnumerator MakeIce()
    {
        yield return new WaitForSeconds(timeToMakeIce);
        SetState(FreezerState.iceIn);
    }
}
