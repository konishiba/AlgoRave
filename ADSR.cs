using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ADSR
{
    public event Action OnTimerFinished = null;
    public event Action OnReleaseFinished = null;

    public ADSR_STATE currentState = ADSR_STATE.ATTACK_STATE;

    float attackRate = 0, decayRate = 0, releaseRate = 0;
    float attackTime = 0.0f, decayTime = 0.0f, sustainRate = 1f, releaseTime = 0.0f;
    int currentSampleStageIndex = 0;
    float sampleRate = 0;

    public ADSR(float _attackTime, float _decayTime, float _sustainRate, float _releaseTime)
    {
        attackTime = _attackTime;
        decayTime = _decayTime;
        sustainRate = _sustainRate;
        releaseTime = _releaseTime;
    }

    public ADSR()
    {
        attackTime = 0.1f;
        decayTime = 0.1f;
        sustainRate = 1.0f;
        releaseTime = 0.1f;
    }

    public void Start()
    {
        //OnTimerFinished += UpdateState;
    }

    public void Init(int _sampleRate)
    {
        sampleRate = (float)_sampleRate;
        attackRate = (int)attackTime * sampleRate;
        decayRate = (int)decayTime * sampleRate;
        releaseRate = (int)releaseTime * sampleRate;
    }

    public void Update(ref float _amplitude, float _maxAmplitude)
    {
        //Console.WriteLine(currentState.ToString());
        if(currentState == ADSR_STATE.ATTACK_STATE)
        {
            float _factor = Lineare(currentSampleStageIndex / attackRate);
            //Console.WriteLine(_factor);
            _amplitude = _maxAmplitude * _factor; //TODO Mettre la callback à la place
            UpdateSampleStageIndex(attackRate);
        }
        else if(currentState == ADSR_STATE.DECAY_STATE)
        {
            //_value = decayRate;
            //UpdateSampleStageIndex(_value);
            //_amplitude = _maxAmplitude * sustainRate * 1f - Lineare(currentTime / _value);
        }
        else if(currentState == ADSR_STATE.RELEASE_STATE)
        {
            //_value = releaseRate;
            _amplitude = Math.Clamp(_maxAmplitude * 1f - Lineare(currentSampleStageIndex / releaseRate), 0.001f, _maxAmplitude);
            UpdateSampleStageIndex(releaseRate);
        }
        else return;
        //Console.WriteLine(_amplitude.ToString());
        
    }

    void UpdateState()
    {
      
        List<ADSR_STATE> _allState = Enum.GetValues<ADSR_STATE>().ToList();
        int _index = _allState.IndexOf(currentState) + 1;
        Console.WriteLine("Update State :" + currentState);
        if(_index < _allState.Count)
        {
            currentState = _allState[_index];
            currentSampleStageIndex = 0;
        }
        else 
        {
            Console.WriteLine("Finished");
            OnReleaseFinished?.Invoke();
        }
    }

    void UpdateSampleStageIndex(float _max)
    {
        if (currentState == ADSR_STATE.SUSTAIN_STATE) return;
        currentSampleStageIndex++;
        //Console.WriteLine("currentSampleStageIndex : " + currentSampleStageIndex);
        if (currentSampleStageIndex > _max)
        {
            currentSampleStageIndex = 0;
            UpdateState();
            OnTimerFinished?.Invoke();
        }
    }

    float Lineare(float t)
    {
        return Math.Clamp(t, 0.0f, 1.0f);
    }
}

