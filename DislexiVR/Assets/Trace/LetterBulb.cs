using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterBulb : MonoBehaviour
{
    Wand wand;
    Coroutine wandNearCoroutine;

    public bool isNear;
    public bool isFuckingLit;

    public float currentDistance;
    [Tooltip("A score for this letterbulb measuring the closest the wand gets to the bulb " +
        "while casting as a function of the course cast distance. Out of 1.")]
    public float bestDistance;

        //relationships
    private void Awake()
    {
        wand = FindObjectOfType<Wand>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && wand.isCasting && !isFuckingLit)
        {
            OnLight();
        }
    }

    public void OnWandEnter()
    {
        isNear = true;

        //for testing in the editor scene
        if (wand.debugMode && !isFuckingLit)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        wandNearCoroutine = StartCoroutine(wandNear());

    }
    public void OnWandExit()
    {
        isNear = false;

        if (wand.debugMode && !isFuckingLit)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        StopAllCoroutines();
    }

    IEnumerator wandNear()
    {
        while (true)
        {
            Debug.Log("in WandNear");
            if (wand.isCasting && !isFuckingLit)
            {
                OnLight();
            }

            currentDistance = Vector3.Distance(transform.position, wand.transform.position);
            if (currentDistance < bestDistance)
            {
                bestDistance = currentDistance;
            }

            yield return new WaitForSeconds(.5f);
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

}
