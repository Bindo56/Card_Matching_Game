using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    [Header("Grid Layout")]
    [SerializeField] private int rows = 3;
    [SerializeField] private int columns = 3;

    [Header("References")]
    [SerializeField] private RectTransform boardContainer;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private CardView cardPrefab;
    [SerializeField] private List<CardDefinition> cardDefinitions;

    private readonly List<CardView> spawnedCards = new();

    private void Awake()
    {
        if (gridLayout == null)
            gridLayout = boardContainer.GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        IsTallAspect();
        ApplyAspectAwareLayout();
        ConfigureGridLayout();
        CreateBoard();
    }


    private void ConfigureGridLayout()
    {
       
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        Vector2 cardSize = CalculateCardSize();
        gridLayout.cellSize = cardSize;
    }

    private Vector2 CalculateCardSize()
    {
        float availableWidth =
            boardContainer.rect.width
            - gridLayout.padding.left
            - gridLayout.padding.right
            - gridLayout.spacing.x * (columns - 1);

        float availableHeight =
            boardContainer.rect.height
            - gridLayout.padding.top
            - gridLayout.padding.bottom
            - gridLayout.spacing.y * (rows - 1);

        float cardWidth = availableWidth / columns;
        float cardHeight = availableHeight / rows;

        float size = Mathf.Min(cardWidth, cardHeight);
        return new Vector2(size, size);
    }


    private void CreateBoard()
    {
        ClearBoard();

        int totalSlots = rows * columns;

        // Ensure even number for pairs
        if (totalSlots % 2 != 0)
            totalSlots -= 1;

        int pairCount = totalSlots / 2;

        List<CardDefinition> deck = BuildDeck(pairCount);
        Shuffle(deck);

        for (int i = 0; i < totalSlots; i++)
        {
            CardView card = Instantiate(cardPrefab, boardContainer);
            card.Initialize(deck[i]);
            spawnedCards.Add(card);
        }
    }


    private void ApplyAspectAwareLayout()
    {
        if (IsTallAspect())
        {
            // Mobile / portrait
            gridLayout.spacing = new Vector2(12f, 18f);
            gridLayout.padding = new RectOffset(24, 24, 32, 32);
        }
        else
        {
            // Desktop / landscape
            gridLayout.spacing = new Vector2(10f, 10f);
            gridLayout.padding = new RectOffset(20, 20, 20, 20);
        }
    }


    private bool IsTallAspect()
    {
        
        float aspect = (float)Screen.height / Screen.width;
        return aspect > 1.7f; // catches 9:16, 9:18, most mobiles
    }

    private List<CardDefinition> BuildDeck(int pairCount)
    {
        List<CardDefinition> deck = new();

        for (int i = 0; i < pairCount; i++)
        {
            deck.Add(cardDefinitions[i]);
            deck.Add(cardDefinitions[i]);
        }

        return deck;
    }

    private void Shuffle(List<CardDefinition> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    private void ClearBoard()
    {
        foreach (Transform child in boardContainer)
            Destroy(child.gameObject);

        spawnedCards.Clear();
    }

}
