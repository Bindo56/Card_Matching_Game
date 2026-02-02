using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private readonly List<CardView> revealedCards = new();

    private void OnEnable()
    {
        GameEvents.RequestCardFlip += HandleCardFlipRequest;
    }

    private void OnDisable()
    {
        GameEvents.RequestCardFlip -= HandleCardFlipRequest;
    }

    private void HandleCardFlipRequest(CardView card)
    {
        if (!CanReveal(card)) return;

        RevealCard(card);
        TryResolveMatch();
    }

    private bool CanReveal(CardView card)
    {
        return card.State == CardState.Hidden;
    }

    private void RevealCard(CardView card)
    {
        card.SetState(CardState.Revealed);
        revealedCards.Add(card);
        GameEvents.CardRevealed?.Invoke(card);
    }

    private void TryResolveMatch()
    {
        if (revealedCards.Count < 2) return;

        // match logic comes next
    }
}
