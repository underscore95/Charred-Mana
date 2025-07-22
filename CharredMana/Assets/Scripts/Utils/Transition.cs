
using System;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class Transition
{
    public enum State
    {
        TransitionIn, Stay, TransitionOut
    }

    [SerializeField] private float _transitionIn = 1.0f;
    public float TransitionIn { get { return _transitionIn; } }
    [SerializeField] private float _stay = 3.0f;
    public float Stay { get { return _stay; } }
    [SerializeField] private float _transitionOut = 1.0f;
    public float TransitionOut { get { return _transitionOut; } }

    public Transition(float transitionIn, float stay, float transitionOut)
    {
        _transitionIn = transitionIn;
        _stay = stay;
        _transitionOut = transitionOut;

        Assert.IsTrue(transitionIn >= 0, $"transitionIn must be >= 0 but it was {transitionIn}");
        Assert.IsTrue(stay >= 0, $"stay must be >= 0 but it was {stay}");
        Assert.IsTrue(transitionOut >= 0, $"transitionOut must be >= 0 but it was {transitionOut}");
        GetDuration(); // asserts that duration is valid
    }

    // returns value between 0 and 1
    // imagine it like something fading in, staying for a bit at full opacity, then fading out, where this returns the alpha and elapsed is time since started fading in
    public float GetTransition(float elapsed)
    {
        return GetTransition(elapsed, out var _);
    }

    public float GetTransition(float elapsed, out State state)
    {
        // After transition in
        if (elapsed >= GetDuration())
        {
            state = State.TransitionOut;
            return 0;
        }

        // Before transition in
        if (elapsed <= 0)
        {
            state = State.TransitionIn;
            return 0;
        }

        // Stay
        if (elapsed >= _transitionIn && elapsed <= _transitionIn + _stay)
        {
            state = State.Stay;
            return 1;
        }

        // Transition in
        if (elapsed < _transitionIn)
        {
            state = State.TransitionIn;
            return Mathf.InverseLerp(0, _transitionIn, elapsed);
        }

        // Transition out
        state = State.TransitionOut;
        return Mathf.InverseLerp(_transitionOut, 0, elapsed - _transitionIn - _stay);
    }

    public bool IsTransitionFinished(float elapsed)
    {
        Assert.IsTrue(_transitionIn + _stay + _transitionOut > 0, "Transition cannot be instant");
        return elapsed >= GetDuration();
    }

    public float GetDuration()
    {
        float duration = _transitionIn + _stay + _transitionOut;
        Assert.IsTrue(duration > 0, "Transition cannot be instant");
        return duration;
    }

    public State GetState(float secondsElapsed)
    {
        GetTransition(secondsElapsed, out State s);
        return s;
    }
}