using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Synth
{
    List<string> patern = new List<string>();
    int currentIndexPatern = 0;

    double currentTime = 0.0f, cycleTime = 2.0f;

    bool playSilence = false;

    public List<string>Patern { get { return patern; } set { patern = value; } }


    public void SetVolume(float _volume, ref WaveOutEvent _output)
    {
        _output.Volume = _volume / 1000;
    }

    //public void Update()
    //{
    //    UpdateTimer();
    //}

    //public void ReadPatern()
    //{
    //    if (patern.Count < 1) return;
    //    string _note = patern[currentIndexPatern];
    //    if (_note == "~")
    //    {
    //        tempAmplitude = targetAmplitude;
    //        targetAmplitude = 0.0f;
    //        playSilence = true;
    //        return;
    //    }
    //    else if(_note != "~" && playSilence == true)
    //    {
    //        targetAmplitude = tempAmplitude;
    //        playSilence = false;
    //    }
    //        NoteUtilites.NoteToFrequence(patern[currentIndexPatern], ref targetFrequency);
    //    Console.WriteLine(patern[currentIndexPatern]);
    //}

    //void UpdateTimer()
    //{
    //    currentTime += Time.Deltatime;
    //    if (currentTime > cycleTime / patern.Count)
    //    {
    //        IncrementIndexPatern();
    //        ReadPatern();
    //        currentTime = 0.0f;
    //    }
    //    //Console.WriteLine(currentTime);

    //}

    void IncrementIndexPatern()
    {
        currentIndexPatern = currentIndexPatern + 1 > patern.Count - 1 ? 0 : currentIndexPatern + 1;
    }
}

