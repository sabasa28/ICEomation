using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour, IInteractable
{
    void Start()
    {
        
    }
    public InteractionResult AttemptInteract(PlayerResources playerResources)
    {
        return new InteractionResult(false, 0.0f);

    }
}
