using System;

namespace Nonwindows.Speech
{
    public delegate void PhraseRecognized(PhraseRecognizedEventArgs args);

    public class PhraseRecognizedEventArgs
    {
        public string text;
    }

    public class KeywordRecognizer
    {
        public event PhraseRecognized OnPhraseRecognized;

        public KeywordRecognizer(string[] recognizedWords)
        {

        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}