using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreService : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private int baseMatchScore = 10;

    public int CurrentScore { get; private set; }
    public int CurrentCombo { get; private set; }

    public event Action<int> OnScoreChanged; 
    public event Action<int> OnComboChanged;

    private void OnEnable()
    {
        GameEvents.MatchResolved += HandleMatchResolved;
    }

    private void OnDisable()
    {
        GameEvents.MatchResolved -= HandleMatchResolved;
    }

    private void Start()
    {
        ResetScore();
    }

    private void HandleMatchResolved(bool isMatch)
    {
        if (isMatch)
        {
            CurrentCombo++;
            int addedScore = baseMatchScore * CurrentCombo;
            CurrentScore += addedScore;

            OnComboChanged?.Invoke(CurrentCombo);
            OnScoreChanged?.Invoke(CurrentScore);

           // Debug.Log($"[Score] Match +{addedScore} (Combo x{CurrentCombo})");
        }
        else
        {
            ResetCombo();
          //  Debug.Log("[Score] Mismatch – combo reset");
        }
    }


    public void Set(int value)
    {
        CurrentScore = value;
    }
    public void RestoreScore(int score)
    {
        CurrentScore = score;
        CurrentCombo = 0;

        OnScoreChanged?.Invoke(CurrentScore);
        OnComboChanged?.Invoke(CurrentCombo);
    }

    public int GetScore()
    {
        return CurrentScore;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        ResetCombo();
        OnScoreChanged?.Invoke(CurrentScore);
    }

    private void ResetCombo()
    {
        CurrentCombo = 0;
        OnComboChanged?.Invoke(CurrentCombo);
    }
}
