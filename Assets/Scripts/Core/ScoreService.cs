using System;
using UnityEngine;

public class ScoreService : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private int baseMatchScore = 10;

    public int CurrentScore { get; private set; }
    public int CurrentCombo { get; private set; }

    // UI / other systems can listen to these
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

    // -------------------------
    // Event handling
    // -------------------------

    private void HandleMatchResolved(bool isMatch)
    {
        if (isMatch)
        {
            CurrentCombo++;
            int addedScore = baseMatchScore * CurrentCombo;
            CurrentScore += addedScore;

            OnComboChanged?.Invoke(CurrentCombo);
            OnScoreChanged?.Invoke(CurrentScore);

            Debug.Log($"[Score] Match +{addedScore} (Combo x{CurrentCombo})");
        }
        else
        {
            ResetCombo();
            Debug.Log("[Score] Mismatch – combo reset");
        }
    }

    // -------------------------
    // Public API
    // -------------------------

    public void RestoreScore(int score)
    {
        CurrentScore = score;
        CurrentCombo = 0;

        OnScoreChanged?.Invoke(CurrentScore);
        OnComboChanged?.Invoke(CurrentCombo);

        Debug.Log($"[Score] Restored score: {CurrentScore}");
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

    // -------------------------
    // Internals
    // -------------------------

    private void ResetCombo()
    {
        CurrentCombo = 0;
        OnComboChanged?.Invoke(CurrentCombo);
    }
}
