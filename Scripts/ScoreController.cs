using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class ScoreController : MonoBehaviour
{
    public UnityEvent OnScoreChanged;

    public UnityEvent ScoreTresholdReached;

    [SerializeField]
    public int score = 0;

    [SerializeField]
    public int scoreTreshold = 1000;

    private void Update()
    {
        if (score >= scoreTreshold)
        {
            ScoreTresholdReached.Invoke();
        }
    } 

    public void AddScore(int valueToAdd)
    {
        score += valueToAdd;
        OnScoreChanged.Invoke();
    }

    public void SubtractScore(int valueToSubtract)
    {
        score -= valueToSubtract;
        OnScoreChanged.Invoke();
    }
}
