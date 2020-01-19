using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRandom : MonoBehaviour
{
    public int letterIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int letterIndex = Mathf.RoundToInt(Random.Range(0, 4 - 1));
    }
}
