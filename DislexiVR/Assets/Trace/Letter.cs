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

    [Tooltip("Portion of casting time within the letter boundaries.")]
    public float inBoundsTime = 1;
    [Tooltip("InBoundsTime that allows pass.")]
    public float inBoundsThreshold = .75f;

    public GameObject summonedObject;

    public TextMeshProUGUI letterTextMesh;
    public TextMeshProUGUI wordTextMesh;

    public int ignitedBulbs;

    public Wand wand;


    //relationships
    private void Awake()
    {
        letterBulbs = GetComponentsInChildren<LetterBulb>(true);
        wand = FindObjectOfType<Wand>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CleanUpBulbs();
        ResetLetter();
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void CleanUpBulbs()
    {
        letterBulbs = GetComponentsInChildren<LetterBulb>();

        for (int i = 0; i < letterBulbs.Length; i++)
        {
            DestroyImmediate(letterBulbs[i].gameObject);
        }
        letterBulbs = GetComponentsInChildren<LetterBulb>();
    }

    public void ResetLetter()
    {
        letterBulbs = GetComponentsInChildren<LetterBulb>();
        foreach (var bulb in letterBulbs)
        {
            bulb.ResetBulb();
        }
        strokesUsed = 0;
        ignitedBulbs = 0;
    }

    public IEnumerator CastLoggingCoroutine()
    {
        int passingMoments = 0;
        int totalMoments = 0;

        while (wand.isCasting)
        {
            foreach (var bulb in letterBulbs)
            {
                if (bulb.currentDistance < .5)
                {
                    passingMoments++;
                    break;
                }
            }
            totalMoments++;

            yield return new WaitForEndOfFrame();
        }
        inBoundsTime = passingMoments / totalMoments;
        yield break;
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

        summonedObject = letterConfiguration.summonPrefab;

    }

}
