using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterTrigger : MonoBehaviour
{
    public bool lookingAtSea = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sea"))
        {
            lookingAtSea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Sea"))
        {
            lookingAtSea = false;
        }
    }
}
