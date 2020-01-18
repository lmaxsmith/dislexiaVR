using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    [Tooltip("Number of strokes required to finish the letter.")]
    public int strokesAllowed = 1;
    public int strokesUsed = 0;
    [Tooltip("List of all letter bulbs contained within the letter.")]
    public LetterBulb[] letterBulbs;
    public GameObject letterBulbPrefab;
    public LetterConfiguration letterConfiguration;


    public TextMeshProUGUI letterTextMesh;
    public TextMeshProUGUI wordTextMesh;
    


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

        letterConfiguration.strokes = strokesAllowed;
        letterConfiguration.letter = letterTextMesh.text;
        letterConfiguration.wordRemainder = wordTextMesh.text;
    }

    [ContextMenu("Load Letter Configuration")]
    public void LoadConfiguration()
    {
        LoadConfiguration(letterConfiguration);
    }
    public void LoadConfiguration(LetterConfiguration letterConfiguration)
    {
        this.letterConfiguration = letterConfiguration;

        letterBulbs = GetComponentsInChildren<LetterBulb>();
        foreach (var bulb in letterBulbs)
        {
            DestroyImmediate(bulb.gameObject);
        }
        letterBulbs = new LetterBulb[0];
        //setup bulbs
        foreach (var bulbPosition in letterConfiguration.bulbPositions)
        {
            GameObject bulbObject = Instantiate(letterBulbPrefab);
            bulbObject.transform.SetParent(transform);
            bulbObject.transform.localPosition = bulbPosition;
        }
        letterBulbs = GetComponentsInChildren<LetterBulb>();

        strokesAllowed = letterConfiguration.strokes;
        letterTextMesh.text = letterConfiguration.letter;
        wordTextMesh.text = letterConfiguration.wordRemainder;

    }

}
