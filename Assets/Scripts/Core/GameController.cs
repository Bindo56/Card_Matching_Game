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
        Debug.Log($"Card {card.State} revealed.");
        card.SetState(CardState.Revealed);
        Debug.Log($"Card {card.State} revealed.");
        revealedCards.Add(card);
        GameEvents.CardRevealed?.Invoke(card);
    }

    private void TryResolveMatch()
    {
        if (revealedCards.Count < 2) return;


        CardView first = revealedCards[0];
        CardView second = revealedCards[1];

        Debug.Log($"Resolving match between Card {first.Definition.pairId} and Card {second.Definition.pairId}.");
        first.SetState(CardState.Resolving);
        second.SetState(CardState.Resolving);

        bool isMatch = first.Definition.pairId == second.Definition.pairId;

        if (isMatch)
        {
            first.SetState(CardState.Matched);
            second.SetState(CardState.Matched);

            GameEvents.MatchResolved?.Invoke(true);
        }
        else
        {
            first.SetState(CardState.Hidden);
            second.SetState(CardState.Hidden);

            GameEvents.MatchResolved?.Invoke(false);
        }

        revealedCards.Clear();
    }
}
