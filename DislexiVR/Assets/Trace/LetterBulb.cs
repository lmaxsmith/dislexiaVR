using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterBulb : MonoBehaviour
{
    Wand wand;
    Coroutine wandNearCoroutine;

    public bool isNear;
    public bool isFuckingLit;

    public float currentDistance = 1;
    [Tooltip("A score for this letterbulb measuring the closest the wand gets to the bulb " +
        "while casting as a function of the course cast distance. Out of 1.")]
    public float bestDistance = 1;

        //relationships
    private void Awake()
    {
        wand = FindObjectOfType<Wand>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TryDebugColor();


    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnWandEnter()
    {
        isNear = true;

        currentDistance = 1;
        wandNearCoroutine = StartCoroutine(wandNear());

    }
    public void OnWandExit()
    {
        isNear = false;

        currentDistance = 1;

        TryDebugColor();

        StopAllCoroutines();
    }

    IEnumerator wandNear()
    {
        while (true)
        {
            Debug.Log("in WandNear");

            //caluculate distances
            currentDistance = Vector3.Distance(transform.position, wand.transform.position) / wand.roughPrecisionScale;
            if (currentDistance < bestDistance)
            {
                bestDistance = currentDistance;
            }

            //control lighting the bulb
            if (wand.isCasting && !isFuckingLit && currentDistance < .5f)
            {
                OnLight();
            }



            //color by distance
            TryDebugColor();

            yield return new WaitForEndOfFrame();
        }
    }

    public void OnLight()
    {
        isFuckingLit = true;

        if (wand.debugMode)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }



    #region ===================== debug and testing ==================================
    void TryDebugColor()
    {
        //color by distance
        if (wand.debugMode && !isFuckingLit)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1 - currentDistance, 0, 0, 1 - currentDistance);
        }
    }



    #endregion
}
