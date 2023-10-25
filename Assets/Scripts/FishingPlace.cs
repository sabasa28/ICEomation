using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPlace : MonoBehaviour, IInteractable
{
    [SerializeField] bool startLevelAddedToGrid = false;
    [SerializeField] bool isFishReady = false;
    [SerializeField] int fishesPerCatch = 1;
    [SerializeField] float secondsToCatchFish = 10;
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
        StartCoroutine(StartFishing());
    }
    public InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        InteractionResult result = new InteractionResult(false, 0.0f);
        if (isFishReady)
        {
            playerResources.ModifyFishAmount(fishesPerCatch);
            SetIsFishReady(false);
            result.interactionSuccessful = true;
        }
        return result;
    }

    void SetIsFishReady(bool newIsFishReady)
    {
        isFishReady = newIsFishReady;
        animator.SetBool("IsFishReady", isFishReady);
        if (!isFishReady)
        {
            StartCoroutine(StartFishing());
        }
    }

    IEnumerator StartFishing()
    {
        if (isFishReady)
        {
            Debug.Log("No deberia estar corriendose esta corutina de fishing place");
            yield break;
        }
        yield return new WaitForSeconds(secondsToCatchFish);
        SetIsFishReady(true);
    }
}
