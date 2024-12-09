using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreAllocator : MonoBehaviour
{
    [SerializeField]
    private int deathPenalty;

    private ScoreController _scoreController;

    void Awake()
    {
        _scoreController = FindObjectOfType<ScoreController>();
    }

    public void AllocateScore()
    {
        _scoreController.SubtractScore(deathPenalty);
    }
}
