

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceGame : MonoBehaviour
{
    
    public int proficiency = 100;
    public LetterConfiguration[] availableLetters;
    public Letter letter;
    public TextMesh wordRemainder;

    TraceSession traceSession;

    // Start is called before the first frame update
    void Start()
    {
        StartSession();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSession(int startingProficiency)
    {
        traceSession = new TraceSession();

        
    }
    public void StartSession()
    {
        StartSession(100);
    }

    #region ============= Round region ============

    public void StartRound(LetterConfiguration letterConfiguration)
    {
        System.Random random = new System.Random();
        int letterIndex = Mathf.RoundToInt(random.Next() * availableLetters.Length);

        letter.LoadConfiguration(availableLetters[letterIndex]);
    }


    #endregion


}




public class TraceSession
{
    public string dateTime;
    public List<LetterRound> letterRounds;
    public int proficiency;

    public TraceSession()
    {
        dateTime = System.DateTime.Now.ToString();
    }

}

public class LetterRound
{
    public char letter;

    public int highScore;
    public int lowScore;
    public int averageScore;

    //TODO: add images?  
}
