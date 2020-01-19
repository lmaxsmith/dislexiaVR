

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceGame : MonoBehaviour
{
    
    public float generalProficiency = 0;
    public LetterConfiguration[] availableLetters;
    public Letter letter;
    public UnityEngine.UI.Text monitor;

    TraceSession currentSession;

    // Start is called before the first frame update
    void Start()
    {
        StartSession();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region =================Session Region====================

    public void StartSession(int startingProficiency)
    {
        currentSession = new TraceSession();

        StartRound();
    }
    public void StartSession()
    {
        StartSession(0); //TODO 
    }

    #endregion



    #region ============= Round region ============

    LetterRound currentRound;

    public void StartRound()
    {
        currentRound = new LetterRound();
        currentSession.letterRounds.Add(currentRound);

        System.Random random = new System.Random();
        int letterIndex = Random.Range(0, availableLetters.Length - 1);

       

        letter.LoadConfiguration(availableLetters[letterIndex]);

        StartAttempt(true);

    }

    IEnumerator RoundCoroutine()
    {
        while (true)
        {







            yield return new WaitForEndOfFrame();
        }
    }

    public void CastSpell()
    {
        Debug.Log("Expelliarmus!");
    }

    #endregion




    #region ================= Attempt Region ====================

    Attempt currentAttempt;

    public void StartAttempt(bool isFirstAttempt)
    {
        

        if (isFirstAttempt)
        {
            currentAttempt = new Attempt(1, 2);
        }
        else //not our first rodeo
        {
            int hintLevel = currentAttempt.hintLevel;
            int sizeLevel = currentAttempt.sizeLevel;

            if (currentAttempt.passed)
            {
                if (hintLevel <= 0)
                {
                    sizeLevel--;
                }
                else
                {
                    hintLevel--;
                }
            }
            else //dind't pass
            {
                if (hintLevel == 0)
                {
                    hintLevel++;
                }
                else if (sizeLevel <2)
                {
                    sizeLevel++;
                }
                
            }

            currentAttempt = new Attempt(hintLevel, sizeLevel);
            Debug.Log(string.Format("New attempt with hint level {0} and size level {1}", hintLevel, sizeLevel));

        }
        monitor.text = string.Format("Hint Level: {0},  Size Level: {1}", currentAttempt.hintLevel, currentAttempt.sizeLevel);

        currentRound.attempts.Add(currentAttempt);

        //setup letter
        letter.enabled = true;
        letter.ResetLetter();

        //StartCoroutine(AttemptCoroutine());
        FindObjectOfType<Wand>().StopCastingEvent.AddListener(CheckLetter);
    }

    public void CheckLetter()
    {
        letter.strokesUsed++;
         if (letter.inBoundsTime < letter.inBoundsThreshold)
        {
            StopAttempt(false);
        }
        else if (letter.ignitedBulbs >= letter.letterBulbs.Length)
        {
            StopAttempt(true);
            //break;
        }
        else if (letter.strokesUsed >= letter.strokesAllowed)
        {
            StopAttempt(false);
        }
        

    }

    IEnumerator AttemptCoroutine() //replace with check result
    {
        while (true)
        {
            Debug.Log(letter.ignitedBulbs + " bulbs ignited out of " + letter.letterBulbs.Length);
            if (letter.ignitedBulbs >= letter.letterBulbs.Length)
            {
                StopAttempt(true);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void StopAttempt(bool passed)
    {
        letter.enabled = false;
        FindObjectOfType<Wand>().StopCastingEvent.RemoveListener(CheckLetter);

        // decide result and next steps
        if (passed)
        {
            Debug.Log("Passed!");
            currentAttempt.passed = true;
            if (currentAttempt.sizeLevel == 0 && currentAttempt.hintLevel == 0)
            {
                CastSpell();
                return;
            }


        }
        else
        {
            currentAttempt.passed = false;
            Debug.Log("Faaaaaailed.");
        }

        StartCoroutine(AttemptPauseCoroutine());
    }

    IEnumerator AttemptPauseCoroutine()
    {
        yield return new WaitForSeconds(3);

        StartAttempt(false);

        yield break;
    }

    public void StepByResult()
    {

    }

    #endregion

}


#region ==================== classes region =========================

public class TraceSession
{
    public string dateTime;
    public List<LetterRound> letterRounds;
    public int proficiency;

    public TraceSession()
    {
        dateTime = System.DateTime.Now.ToString();
        letterRounds = new List<LetterRound>();
    }

}

public class LetterRound
{
    [SerializeField]
    public char letter;

    public int highScore;
    public int lowScore;
    public int averageScore;

    public List<Attempt> attempts = new List<Attempt>();

    public LetterRound()
    {
        attempts = new List<Attempt>();
    }

    //TODO: add images?  
}

public class Attempt
{
    public int sizeLevel = 2;
    public int hintLevel = 1;

    public bool passed;
    public float score;

    public Attempt(int hint, int size)
    {
        sizeLevel = size;
        hintLevel = hint;
    }
}

#endregion
