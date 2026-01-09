using CSCore;
using CSCore.XAudio2;
using System.Collections.Generic;

public class NoteReader : Singleton<NoteReader>, ISampleSource
{
    private readonly int sampleRate;

    List<Note> notes = new();
    Mutex notesLock = new Mutex();

    public List<Note> Notes => notes;

    public NoteReader()
    {
        sampleRate = (int)ESampleRate.MEDIAM_QUALITY;
    }

    public void AddNote(Note _note)
    {
        if (_note == null) return;
        _note.SampleRate = sampleRate;
        _note.Start();
        _note.IsDispose += () => { RemoveNote(_note); };
        notes.Add(_note);
    }
    public void RemoveNote(Note _note)
    {
        if (_note == null) return;
        notes.Remove(_note);
    }

    public bool CanSeek => false;
    public WaveFormat WaveFormat => new WaveFormat(sampleRate, 32, 1, AudioEncoding.IeeeFloat);
    public long Length => 0;
    public long Position { get => 0; set { } }

    public int Read(float[] buffer, int offset, int count)
    {
        Array.Clear(buffer, offset, count);
        float _sample = 0.0f;

        List<Note> notesCopy;
        lock (notesLock) // lock une fois
        {
            notesCopy = new List<Note>(notes); // copie safe
        }
        for (int i = 0; i < count; i++)
        {
            _sample = 0.0f;
            foreach (Note _note in notesCopy)
            {
                if(_note == null) continue;
                _sample += (float)_note.NextSample();
            }
            buffer[offset + i] = Math.Clamp(_sample * 0.2f, -1f, 1f);
        }
        //mutex.Dispose();
        return count;
    }

    public void PrintDebug()
    {
        int _count = notes.Count;
        for (int i = 0; i < _count; i++)
        {
            Note _note = notes[i];
            logger.PrintLog(VerbosityType.Display, $"currentDuration : {_note.CurrentDuration} " +
                                                            $"| currentAmplitude : {_note.CurrentAmplitude}" +
                                                            $"| currentState : {_note.ADSR.currentState}", logger.GetDebugInfo());

        }
    }

    public void Dispose() { }
}