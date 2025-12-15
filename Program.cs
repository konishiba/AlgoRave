using NAudio.Wave;
using System.Diagnostics;



class Program
{
    static bool exit = false;
    static Note wave = null;
    static Note secondWave = null;
    static WaveOutEvent firstOutput = null;
    static WaveOutEvent secondOutput = null;

    //For debug;
    static double debugTime = 0.0f;
    static double interval = 0.0f;

    static void Main()
    {
        logger.Init();
        AppDomain.CurrentDomain.ProcessExit += Exit;
        Sound();

        //wave.Patern.Add("C#6");
        //wave.Patern.Add("~");
        //wave.Patern.Add("F2");
        //Console.WriteLine("Wave");



        while (exit == false)
        {
            Time.UpdateDeltime();
            //Console.WriteLine(Time.Deltatime);
            if (wave == null) continue;
            wave.Update();
            //TODO Remove debug
            double _deltatime = Time.Deltatime;
            debugTime += _deltatime;
            interval += _deltatime;
            //debug 
            //TODO remove debug
            if (interval > 0.01f)
            {
                interval = 0.0f;
                logger.PrintLog(VerbosityType.Display, $"currentDuration : {wave.CurrentDuration} " +
                                                       $"| currentAmplitude : {wave.CurrentAmplitude}" +
                                                       $"| currentState : {wave.ADSR.currentState}", logger.GetDebugInfo());
            }
            //if (wave.ADSR.currentState == ADSR_STATE.RELEASE_STATE)
            //{
            //    //debugTime += (float)Time.Deltatime;
            //    //logger.PrintLog(VerbosityType.Display, "debugTime : " + debugTime, logger.GetDebugInfo());
            //}
            //debug

            //Console.WriteLine("Wave"); 
        }

        //LiveCoding();

        //Console.WriteLine(NoteUtilites.NoteToFrequence("C4"));

        //Sound();
    }


    static void Sound()
    {
        double _freq = 277.6;
        NoteUtilites.NoteToFrequence("C3", ref _freq);
        Console.WriteLine(_freq);
        wave = new Note(_freq,1.0f, 5.0f, new ADSR(0.5f,0.5f, 0.5f, 0.5f));
        firstOutput = new WaveOutEvent();
        firstOutput.Init(wave);
        firstOutput.Volume = 0.5f;
        wave.Start();
        firstOutput.Play(); 
        wave.ADSR.OnReleaseFinished += RemoveWAve;

        //NoteUtilites.NoteToFrequence("F#3", ref _freq);
        //secondWave = new Note(_freq,1.0f, 8.0f, new ADSR(1.0f,1.0f, 1.0f, 1.0f));
        //secondOutput = new WaveOutEvent();
        //secondOutput.Init(secondWave);
        //secondOutput.Volume = 0.1f;
        //secondWave.Start();
        //secondOutput.Play();
        //secondWave.ADSR.OnReleaseFinished += RemoveWAve;

        //BufferedWaveProvider _bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat());
        //_bufferedWaveProvider.

        // //wave = new WaveProvider(_freq);
        // WaveOutEvent _firstOutput = new WaveOutEvent();
        // _firstOutput.Init(wave);
        //// wave.SetAmplitude(0.2f);
        // _firstOutput.Play();


        // //TODO le mettre au moment ou l'on créé la note
        // wave.IsFinished += () => { _firstOutput.Stop(); _firstOutput.Dispose(); };

    }

    static void RemoveWAve()
    {
        firstOutput.Dispose();
        wave = null;
    }

    static void LiveCoding()
    {
        LiveContext _ctx = new LiveContext();

        LiveCodingSystem _liveCoding = new LiveCodingSystem(_ctx);
        _liveCoding.OnReloaded += _ctx.Refresh;
        _ctx.Start();
        while (exit == false)
        {
        }
        //_liveCoding.Execute();
        //while (exit == false)
        //{
        //    if (_ctx.MyObj != null)
        //        Console.WriteLine(_ctx.MyObj.ToString());
        //        //_ctx.MyObj.Update();

        //    else
        //        Console.WriteLine("Null");
        //    Thread.Sleep(1000);
        //}
    }

    static void Exit(object _sender, EventArgs _e)
    {
        exit = true;
    }
}
