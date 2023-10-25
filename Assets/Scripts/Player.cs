using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float interactionRange = 1.0f;
    [SerializeField] PlayerWaterTrigger seaCollider;
    [SerializeField] AudioSource deniedSound;
    [SerializeField] float deniedSoundVolume = 0.3f;
    bool canTakeInput = true;
    Vector2 lookingDirection = Vector2.down;
    Vector2 movement;
    Animator characterAnimator;
    SpriteRenderer spriteRenderer;
    PlayerResources playerResources;

    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerResources = GetComponent<PlayerResources>();
        deniedSound = GetComponent<AudioSource>();
    }
    void Update()
    {
        
        if (canTakeInput) ManageInput();
        else
        {
            movement = Vector2.zero;
            characterAnimator.SetFloat("WalkSpeed", 0);
        }
    }
    void ManageInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Interact((Vector2)transform.position + lookingDirection * interactionRange);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!AttemptToBuild(PosibleInteractables.Freezer, (Vector2)transform.position + lookingDirection * interactionRange))
            {
                deniedSound.PlayOneShot(deniedSound.clip, deniedSoundVolume);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!AttemptToBuild(PosibleInteractables.Barrel, (Vector2)transform.position + lookingDirection * interactionRange))
            {
                deniedSound.PlayOneShot(deniedSound.clip, deniedSoundVolume);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!AttemptToBuild(PosibleInteractables.FishinPlace, (Vector2)transform.position + lookingDirection * interactionRange))
            {
                deniedSound.PlayOneShot(deniedSound.clip, deniedSoundVolume);
            }
        }
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement.magnitude == 0)
        {
            characterAnimator.SetFloat("WalkSpeed", 0);
        }
        else if (Mathf.Abs(movement.normalized.x) > Mathf.Abs(movement.normalized.y))
        {
            characterAnimator.SetFloat("WalkSpeed", 1);

            if (movement.normalized.x >= 0)
            {
                spriteRenderer.flipX = true;
                lookingDirection = Vector2.right;
                seaCollider.transform.localPosition = new Vector3(1.0f, 0.0f, 0.0f);
            }
            else
            {
                spriteRenderer.flipX = false;
                lookingDirection = Vector2.left;
                seaCollider.transform.localPosition = new Vector3(-1.0f, 0.0f, 0.0f);
            }

            characterAnimator.SetTrigger("WalkingSide");
        }
        else
        {
            characterAnimator.SetFloat("WalkSpeed", 1);
            if (movement.normalized.y > 0)
            {
                characterAnimator.SetTrigger("WalkingBack");
                seaCollider.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
                lookingDirection = Vector2.up;
            }
            else
            {
                characterAnimator.SetTrigger("WalkingFront");
                seaCollider.transform.localPosition = new Vector3(0.0f, -1.0f, 0.0f);
                lookingDirection = Vector2.down;
            }
        }
    }

    public void Interact(Vector2 posToInteract)
    {
        IInteractable interactable = InteractingGrid.Get().InteractWithGridAtPos(posToInteract);
        InteractionResult interactionResult = new InteractionResult(false, 0.0f);
        if (interactable != null)
        {
            interactionResult = interactable.AttemptInteract(playerResources);
            if (!interactionResult.interactionSuccessful)
            {
                deniedSound.PlayOneShot(deniedSound.clip, deniedSoundVolume);
            }
        }
        else if (seaCollider.lookingAtSea)
        {
            interactionResult = playerResources.InteractBucketToWater();
        }
        if (interactionResult.interactionSuccessful && interactionResult.interactionTime > 0.0f) 
        {
            StartCoroutine(WaitToFinishInteraction(interactionResult.interactionTime));
        }
    }
    public bool AttemptToBuild(PosibleInteractables buildingToPlace, Vector2 posToBuild)
    {
        return InteractingGrid.Get().AttemptBuildAtPosition(posToBuild, buildingToPlace, playerResources);
    }

    private void FixedUpdate()
    {
        if (movement.magnitude != 0)
        {
            transform.Translate(movement.normalized * speed * Time.fixedDeltaTime);
        }
    }

    IEnumerator WaitToFinishInteraction(float interactionTime)
    {
        canTakeInput = false;
        yield return new WaitForSeconds(interactionTime);
        canTakeInput = true;
    }
}
