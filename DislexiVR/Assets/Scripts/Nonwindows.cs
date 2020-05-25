using System;
using System.Threading.Tasks;

namespace Nonwindows.Speech
{
    public delegate void PhraseRecognized(PhraseRecognizedEventArgs args);

    public class PhraseRecognizedEventArgs
    {
        public string text;

        public PhraseRecognizedEventArgs(string _text)
        {
            text = _text;
        }
    }

    public class KeywordRecognizer
    {
        public event PhraseRecognized OnPhraseRecognized;

        public KeywordRecognizer(string[] recognizedWords)
        {

        }

        public async void Start()
        {
            UnityEngine.Microphone.Start(UnityEngine.Microphone.devices[0], false, 3, 16000);
            await Task.Delay(4000);
            OnPhraseRecognized?.Invoke(new PhraseRecognizedEventArgs("bat"));
        }

        public void Stop()
        {

        }
    }
}