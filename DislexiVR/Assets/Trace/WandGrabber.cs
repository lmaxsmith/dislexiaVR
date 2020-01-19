using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandGrabber : MonoBehaviour
{
    public Wand wand;
    // Start is called before the first frame update
    void Start()
    {
        wand = FindObjectOfType<Wand>();
        wand.transform.SetParent(transform);
        wand.transform.localPosition = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
