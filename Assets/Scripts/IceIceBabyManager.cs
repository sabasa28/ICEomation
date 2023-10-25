using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceIceBabyManager : MonoBehaviourSingleton<IceIceBabyManager>
{
    Vector3 regularScaleIncrease = new Vector3(2.0f, 2.0f, 0.0f);
    bool shouldMeltIce = true;
    [SerializeField] float initialMeltingSpeed = 20.0f;
    [SerializeField] float timeBetweenGlobalWarming = 20.0f;
    [SerializeField] float meltingSpeedMultiplier = 1.1f;
    [SerializeField] Image sunImage;
    [SerializeField] Vector2 scaleToLose = new Vector2(8.0f, 6.0f);
    [SerializeField] Color StartSunColor;
    [SerializeField] Color MaxSunColor;
    float meltingSpeed;
    int startingScaleX;
    int maxScaleX;

    void Start()
    {
        startingScaleX = (int)transform.localScale.x;
        maxScaleX = startingScaleX;
        meltingSpeed = initialMeltingSpeed;
        sunImage.color = StartSunColor;
        StartCoroutine(MeltIce());
        StartCoroutine(GlobalWarming());
    }

    public void ChangeSize(bool increase, float speed = 1)
    {
        int direction = increase ? 1 : -1;
        float multiplier = increase ? 1 : speed * -1;
        Vector3 newScale = transform.localScale + (regularScaleIncrease * multiplier);
        transform.localScale = newScale;
        if (transform.localScale.x > maxScaleX) maxScaleX = (int) transform.localScale.x;
        if (transform.localScale.x <= scaleToLose.x || transform.localScale.y <= scaleToLose.y)
        {
            GameManager.Get().GameOver((maxScaleX - startingScaleX)/2);
        }
    }

    IEnumerator MeltIce()
    {
        while (shouldMeltIce)
        { 
            ChangeSize(false, meltingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator GlobalWarming()
    {
        while (shouldMeltIce)
        {
            yield return new WaitForSeconds(timeBetweenGlobalWarming);
            Debug.Log("GLOBAL WARMING");
            meltingSpeed *= meltingSpeedMultiplier;
            sunImage.color = Color.Lerp(StartSunColor, MaxSunColor, Mathf.InverseLerp(initialMeltingSpeed, 20, meltingSpeed));
        }
    }
}
