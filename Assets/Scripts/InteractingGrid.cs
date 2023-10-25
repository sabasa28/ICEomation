using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PosibleInteractables
{ 
    Barrel,
    FishinPlace,
    Freezer,
    MerchantObject,
    Sea
}

public struct InteractionResult
{
    public InteractionResult(bool inInteractionSuccesful, float inInteractionTime)
    {
        interactionSuccessful = inInteractionSuccesful;
        interactionTime = inInteractionTime;
    }
    public bool interactionSuccessful;
    public float interactionTime;
}
public interface IInteractable
{
    public InteractionResult AttemptInteract(PlayerResources playerResources);

}

public class InteractingGrid : MonoBehaviourSingleton<InteractingGrid>
{
    struct OccupiedPosition
    {
        public Vector2 gridPosition;
        public IInteractable interactable;
    }

    [SerializeField] int gridMaxIndex;
    [SerializeField] Vector2 gridMaxPos;
    [SerializeField] Vector2 gridMinPos;
    [SerializeField] Vector2 buildingZoneMinIndex;
    [SerializeField] Vector2 buildingZoneMaxIndex;

    [SerializeField] Barrel barrelPrefab;
    [SerializeField] int barrelWoodCost;
    [SerializeField] int barrelMetalCost;
    [SerializeField] FishingPlace fishingPlacePrefab;
    [SerializeField] int fishingPlaceWoodCost;
    [SerializeField] int fishingPlaceMetalCost;
    [SerializeField] Freezer freezerPrefab;
    [SerializeField] int freezerWoodCost;
    [SerializeField] int freezerMetalCost;

    Vector2 spawningOffset = new Vector2(0.5f, 0.5f);

    List<OccupiedPosition?> occupiedPositions = new List<OccupiedPosition?>();


    Vector2 GetGridPosFromWorldPos(Vector2 worldPos)
    {
        Vector2 gridUnrondedPos = (worldPos - gridMinPos);
        return new Vector2(Mathf.Floor(gridUnrondedPos.x), Mathf.Floor(gridUnrondedPos.y));
        
        //Vector2 gridRoundedPos = new Vector2(Mathf.Round(gridUnrondedPos.x * 2.0f), Mathf.Round(gridUnrondedPos.y * 2.0f)) / 2.0f; //merca rarisima que invente para sacar el centro del tile pero que al final no necesite
    }
    Vector2 GetWorldPosFromGridPos(Vector2 gridPos)
    {
        return (gridPos + gridMinPos + new Vector2(0.5f, 0.5f));
    }
    OccupiedPosition? GetOccupiedPosition(Vector2 gridPos)
    {
        foreach (OccupiedPosition occupiedPosition in occupiedPositions)
        {
            if (occupiedPosition.gridPosition == gridPos)
            {
                return occupiedPosition;
            }
        }
        return null;
    }

    public IInteractable InteractWithGridAtPos(Vector2 worldPosition)
    {
        OccupiedPosition? occupiedPosition = GetOccupiedPosition(GetGridPosFromWorldPos(worldPosition));
        if (occupiedPosition != null)
        { 
            IInteractable interactableAtPos = occupiedPosition.GetValueOrDefault().interactable;
            return interactableAtPos;
        }
        return null;
    }

    public bool AttemptBuildAtPosition(Vector2 worldPosition, PosibleInteractables construction, PlayerResources playerResources)
    {
        Vector2 gridPos = GetGridPosFromWorldPos(worldPosition);
        OccupiedPosition? ocupiedPosition = GetOccupiedPosition(gridPos);
        if (!IsGridPositionInBuildingGrid(gridPos)) return false;
        if (ocupiedPosition == null)
        {
            IInteractable interactable;
            switch (construction)
            {
                case PosibleInteractables.Barrel:
                    if (playerResources.ModifyWoodAndMetalAmounts(-barrelMetalCost, -barrelWoodCost))
                    {
                        interactable = Instantiate(barrelPrefab, GetWorldPosFromGridPos(gridPos), Quaternion.identity);
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case PosibleInteractables.FishinPlace:
                    if (playerResources.ModifyWoodAndMetalAmounts(-fishingPlaceMetalCost, -fishingPlaceWoodCost))
                    {
                        interactable = Instantiate(fishingPlacePrefab, GetWorldPosFromGridPos(gridPos), Quaternion.identity);
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case PosibleInteractables.Freezer:

                    if (playerResources.ModifyWoodAndMetalAmounts(-freezerMetalCost, -freezerWoodCost))
                    {
                        interactable = Instantiate(freezerPrefab, GetWorldPosFromGridPos(gridPos), Quaternion.identity);
                    }
                    else
                    {
                        return false;
                    }
                    break;
                default:
                    interactable = null;
                    Debug.Log("What are you trying to build???!?!?!??!?");
                    return false;
            }
            OccupyPosition(gridPos, interactable);
            //IInteractable interactableAtPos = ocupiedPosition.GetValueOrDefault().interactable; //checkear que esto funque
            return true;
        }
        return false;
    }

    void OccupyPosition(Vector2 gridPos, IInteractable interactable)
    {
        OccupiedPosition occupiedPosition;
        occupiedPosition.gridPosition = gridPos;
        occupiedPosition.interactable = interactable;
        occupiedPositions.Add(occupiedPosition);
    }

    public void OccupyPosition(IInteractable interactable, Vector2 worldpos) //por favor no me juzgues
    {
        OccupyPosition(GetGridPosFromWorldPos(worldpos), interactable);
    }

    bool IsGridPositionInBuildingGrid(Vector2 gridPosition)
    {
        return (gridPosition.x >= buildingZoneMinIndex.x && gridPosition.y >= buildingZoneMinIndex.y && gridPosition.x <= buildingZoneMaxIndex.x && gridPosition.y <= buildingZoneMaxIndex.y);
    }
}
