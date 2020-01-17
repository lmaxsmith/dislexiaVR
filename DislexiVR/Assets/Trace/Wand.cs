using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [Tooltip("The diameter of the larges collision sphere (in meters)")]
    public float roughPrecisionScale = .3f;

    //collection of trigger colliders at various Distances from the tip of the wand. 
    Collider castingCollider;

    public bool isCasting;

    [Tooltip("Turn this on in the unit test scene.")]
    public bool debugMode;

    //relationships
    private void Awake()
    {
        castingCollider = gameObject.GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isCasting = false; //just in case we fuck it up in editor
    }

    // Update is called once per frame
    void Update()
    {
        //for debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCasting = !isCasting;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        LetterBulb letterBulb;
        if (letterBulb = other.gameObject.GetComponent<LetterBulb>())
        {
            Debug.Log("letterbulb collision detected.");
            
            letterBulb.OnWandEnter();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        LetterBulb letterBulb;
        if (letterBulb = other.gameObject.GetComponent<LetterBulb>())
        {
            Debug.Log("letterbulb collision exit.");
            letterBulb.OnWandExit();

        }

    }
}
