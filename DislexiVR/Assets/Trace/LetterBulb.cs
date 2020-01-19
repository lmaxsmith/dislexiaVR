using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterBulb : MonoBehaviour
{
    public  Wand wand;
    Letter letter;
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
        letter = FindObjectOfType<Letter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TryChangeColor();


    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ResetBulb()
    {
        isFuckingLit = false;
        bestDistance = 1;
        TryChangeColor();
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

        TryChangeColor();

        StopAllCoroutines();
    }

    IEnumerator wandNear()
    {
        while (true)
        {

            //caluculate distances
            currentDistance = Vector3.Distance(transform.position, wand.transform.position) / (wand.roughPrecisionScale * transform.parent.localScale.x);
            if (currentDistance < bestDistance)
            {
                bestDistance = currentDistance;
            }

            //control lighting the bulb
            if (wand.isCasting && !isFuckingLit && currentDistance < .4f)
            {
                OnLight();
            }



            //color by distance
            TryChangeColor();

            yield return new WaitForEndOfFrame();
        }
    }

    public void OnLight()
    {
        isFuckingLit = true;
        letter.ignitedBulbs++;

            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }



    #region ===================== debug and testing ==================================
    void TryChangeColor()
    {
        
        //color by distance
        if (!isFuckingLit)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1 - currentDistance, 0, 0, 1 - currentDistance);
        }
    }



    #endregion
}
