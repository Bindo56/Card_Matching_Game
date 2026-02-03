using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private readonly List<CardView> revealedCards = new();


    [SerializeField] private BoardController boardController;
    [SerializeField] private ScoreService scoreService;

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
            first.HideCard();
            second.HideCard();

            GameEvents.MatchResolved?.Invoke(true);
            Debug.Log("Matched");
        }
        else
        {
            first.SetState(CardState.Hidden);
            second.SetState(CardState.Hidden);

            GameEvents.MatchResolved?.Invoke(false);
            Debug.Log(" Not Matched");
        }

        revealedCards.Clear();
    }

    private void Start()
    {
        /* if (SaveService.HasSave())
         {
             ContinueGame();
         }
         else
        {
            NewGame(
                boardController.GetRow(),
                boardController.GetColumn()
            );
        }*/
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public void SaveGame()
    {
        if (boardController == null || scoreService == null)
            return;

        SaveData data = boardController.GetSaveData(scoreService.GetScore());
        SaveService.Save(data);

    }

    private void TryRecoverGame()
    {
        if (!SaveService.TryLoad(out SaveData data))
        {
            Debug.Log("No saved game to recover.");
            return;
        }

        //  scoreService.Set(data.score);
        boardController.RestoreFromSave(data);
    }

    // -------------------- GAME FLOW --------------------

    public void NewGame(int rows, int columns)
    {
        SaveService.Delete();
        scoreService.ResetScore();
        boardController.Setup(rows, columns);
    }

    private void LoadGame()
    {
        if (!SaveService.TryLoad(out SaveData data))
        {
            NewGame(5, 6);
            return;
        }

        scoreService.Set(data.score);
        boardController.RestoreFromSave(data);
    }

    public void ContinueGame()
    {
        if (SaveService.HasSave())
        {
            LoadGame();
            //ContinueGame();
        }
    }
}
