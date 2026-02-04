using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public UnityEvent<int, int> OnNewGame = new UnityEvent<int, int>();
    public UnityEvent OnContinue = new UnityEvent();

    [Header("UI Component & Panel")]

    [Tooltip("Menu Panel UI")]
    [SerializeField] Transform menuPanel;
    [SerializeField] Transform wonPanel;
    [Tooltip("Continie Btn in Menu Panel")]
    [SerializeField] Button continueLastGameBtn;

    SaveData saveData;

    public void NewGame_2x2() => OnNewGame.Invoke(2, 2);
    public void NewGame_3x3() => OnNewGame.Invoke(3, 3);
    public void NewGame_5x6() => OnNewGame.Invoke(5, 6);
    public void Back() => GameEvents.GameOver.Invoke();
    public void ContinueLastGame() => OnContinue.Invoke();

    private void OnEnable()
    {
        GameEvents.GameOver += ShowGameWon;
        GameEvents.HideMenu += HideMainMenu;
        GameEvents.ShowMenu += ShowMainMenu;

    }

    private void OnDisable()
    {
        GameEvents.GameOver -= ShowGameWon;
        GameEvents.HideMenu -= HideMainMenu;
        GameEvents.ShowMenu -= ShowMainMenu;
    }

    void ShowGameWon()
    {
        wonPanel.gameObject.SetActive(true);
    }

    void ShowMainMenu()
    {
        wonPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);
    }
    void HideMainMenu()
    {
        wonPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(false);
    }

}
