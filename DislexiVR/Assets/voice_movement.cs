using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;

public class voice_movement : MonoBehaviour
{
    public AudioClip start, stop, correct;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private Coroutine coroutine;

    public AudioSource _source;

    bool answered;

    void Start()
    {
        actions.Add("dog", CorrectAnswer);
        actions.Add("cat", CorrectAnswer);
        actions.Add("bat", CorrectAnswer);

    }

    [ContextMenu("Start Listening")]
    public void StartVoiceCommandListen()
    {
        answered = false;

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
        _source.clip = start;
        _source.PlayOneShot(start);
        coroutine = StartCoroutine(FinishedRecognition());

    }

    IEnumerator FinishedRecognition()
    {
        while (!answered)
        {
            yield return new WaitForSeconds(10);
            _source.PlayOneShot(stop);
        }
        keywordRecognizer.Stop();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        actions[speech.text].Invoke();
    }

    private void CorrectAnswer()
    {
        answered = true;
        UnityEngine.Debug.Log("Hit");
        _source.PlayOneShot(correct);
        StopCoroutine(coroutine);
        StartCoroutine(waitForNewRound());

        FindObjectOfType<TraceGame>().CastSpell();
    }

    IEnumerator waitForNewRound()
    {
        yield return new WaitForSeconds(5);

        UnityEngine.Debug.Log("Starting new round");
        FindObjectOfType<TraceGame>().StartRound();

        yield break;
    }

}

