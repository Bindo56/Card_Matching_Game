using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;

    [Header("References")]
    [SerializeField] private RectTransform boardContainer;
    [SerializeField] private CardView cardPrefab;
    [SerializeField] private List<CardDefinition> cardDefinitions;

    private readonly List<CardView> spawnedCards = new();

    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        ClearBoard();

        int totalSlots = rows * columns;
        int requiredPairs = totalSlots / 2;

        List<CardDefinition> selectedCards = BuildCardDeck(requiredPairs);

        Shuffle(selectedCards);

        for (int i = 0; i < totalSlots; i++)
        {
            CardView card = Instantiate(cardPrefab, boardContainer);
            card.Initialize(selectedCards[i]);
            spawnedCards.Add(card);
        }
    }

    private List<CardDefinition> BuildCardDeck(int pairCount)
    {
        List<CardDefinition> deck = new();

        for (int i = 0; i < pairCount; i++)
        {
            CardDefinition definition = cardDefinitions[i];
            deck.Add(definition);
            deck.Add(definition);
        }

        return deck;
    }

    private void ClearBoard()
    {
        foreach (Transform child in boardContainer)
        {
            Destroy(child.gameObject);
        }

        spawnedCards.Clear();
    }

    private void Shuffle(List<CardDefinition> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
