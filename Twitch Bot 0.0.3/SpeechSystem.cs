using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace Twitch_Bot_0._0._3
{
    class SpeechSystem
    {
        public void SpeechReconizer()
        {
            SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();

            GrammarBuilder gb = new GrammarBuilder("test");
            Grammar gr = new Grammar(gb);
            gr.Name = "testGrammer";
            _recognizer.LoadGrammar(gr);

            _recognizer.SpeechRecognized += _recognizer_SpeechRecocognized;

            _recognizer.SetInputToDefaultAudioDevice(); // set the input of the speech recognizer to the default audio device
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech asynchronous

        }

         private void _recognizer_SpeechRecocognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "test") // e.Result.Text contains the recognized text
            {
                Console.WriteLine("The test was successful!");
            }
        }
    }
}
