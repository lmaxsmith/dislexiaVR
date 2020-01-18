using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    [Tooltip("Number of strokes required to finish the letter.")]
    public int strokes = 1;
    [Tooltip("List of all letter bulbs contained within the letter.")]
    public LetterBulb[] letterBulbs;
    public GameObject letterBulbPrefab;
    public LetterConfiguration letterConfiguration;

    //relationships
    private void Awake()
    {
        letterBulbs = GetComponentsInChildren<LetterBulb>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Save Letter Configuration")]
    public void SaveConfiguration()
    {
        //setup bulbs
        letterBulbs = GetComponentsInChildren<LetterBulb>();
        letterConfiguration.bulbPositions = new Vector3[letterBulbs.Length];
        for (int i = 0; i < letterBulbs.Length; i++)
        {
            letterConfiguration.bulbPositions[i] = letterBulbs[i].transform.localPosition;
        }

        letterConfiguration.strokes = strokes;
    }

    [ContextMenu("Load Letter Configuration")]
    public void LoadConfiguration()
    {
        //setup bulbs
        foreach (var bulbPosition in letterConfiguration.bulbPositions)
        {
            GameObject bulbObject = Instantiate(letterBulbPrefab);
            bulbObject.transform.SetParent(transform);
            bulbObject.transform.localPosition = bulbPosition;
        }

        strokes = letterConfiguration.strokes;
    }

}
