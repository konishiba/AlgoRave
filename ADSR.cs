using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ADSR
{
    public event Action OnTimerFinished = null;
    public event Action OnReleaseFinished = null;

    public ADSR_STATE currentState = ADSR_STATE.ATTACK_STATE;

    float currentTime = 0.0f, attackTime = 0.0f, decayTime = 0.0f, sustainRate = 1f, releaseTime = 0.0f;

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
        OnTimerFinished += () => {UpdateState(); };
    }

    public void Update(ref float _amplitude, float _maxAmplitude)
    {
        float _value = 0.0f;
        if(currentState == ADSR_STATE.ATTACK_STATE)
        {
            _value = attackTime;
            _amplitude = _maxAmplitude * Lineare(currentTime / _value); //TODO Mettre la callback à la place
        }
        if(currentState == ADSR_STATE.DECAY_STATE)
        {
            _value = decayTime;
            _amplitude = _maxAmplitude * sustainRate * 1f - Lineare(currentTime / _value);
        }
        if(currentState == ADSR_STATE.RELEASE_STATE)
        {
            _value = releaseTime;
            _amplitude = _maxAmplitude * 1f - Lineare(currentTime / _value);
        }
        else return;
        UpdateTimer(_value);
    }

    void UpdateState()
    {
      
        List<ADSR_STATE> _allState = Enum.GetValues<ADSR_STATE>().ToList();
        int _index = _allState.IndexOf(currentState) + 1;
        if(_index < _allState.Count)
        {
             currentState = _allState[_allState.IndexOf(currentState) + 1];
        }
        else 
        {
            OnReleaseFinished?.Invoke();
        }
    }

    void UpdateTimer(float _max)
    {
        currentTime += (float)Time.Deltatime;
        if (currentTime > _max)
        {
            currentTime = _max;
            OnTimerFinished?.Invoke();
        }
    }

    float Lineare(float t)
    {
        return Math.Clamp(t, 0.0f, 1.0f);
    }
}

