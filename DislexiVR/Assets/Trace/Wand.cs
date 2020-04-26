using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wand : MonoBehaviour
{
    [Tooltip("The diameter of the larges collision sphere (in meters)")]
    public float roughPrecisionScale = .5f;


    //collection of trigger colliders at various Distances from the tip of the wand.
    Collider castingCollider;
    public Letter letter;

    public bool isCasting;

    [Tooltip("Turn this on in the unit test scene.")]
    public bool debugMode;

    public UnityEvent StartCastingEvent;
    public UnityEvent StopCastingEvent;
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
        if (debugMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCasting();
            }
            if (Input.GetMouseButtonUp(0))
            {
                StopCasting();
            }

            transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
        }


    }

    //Main point of entry into Logan's pieces. Call this from control connectors.
    public void StartCasting()
    {
        letter = FindObjectOfType<Letter>();
        isCasting = true;
        if (StartCastingEvent != null)
        {
            StartCastingEvent.Invoke();
        }
        StartCoroutine(letter.CastLoggingCoroutine());
    }
    public void StopCasting()
    {
        isCasting = false;
        if (StopCastingEvent != null)
        {
            StopCastingEvent.Invoke();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        LetterBulb letterBulb;
        if (letterBulb = other.gameObject.GetComponent<LetterBulb>())
        {
            // Debug.Log("letterbulb collision detected.");

            letterBulb.OnWandEnter();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        LetterBulb letterBulb;
        if (letterBulb = other.gameObject.GetComponent<LetterBulb>())
        {
            // Debug.Log("letterbulb collision exit.");
            letterBulb.OnWandExit();

        }

    }

    #region ======================  Debug =========================



    #endregion

}
