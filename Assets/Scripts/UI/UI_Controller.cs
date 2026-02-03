using UnityEngine;
using UnityEngine.Events;

public class UI_Controller : MonoBehaviour
{
    public UnityEvent<int, int> OnNewGame = new UnityEvent<int, int>();
    public UnityEvent OnContinue = new UnityEvent();

    public void NewGame_2x2() => OnNewGame.Invoke(2, 2);
    public void NewGame_3x3() => OnNewGame.Invoke(3, 3);
    public void NewGame_5x6() => OnNewGame.Invoke(5, 6);
    public void ContinueLastGame() => OnContinue.Invoke();
}
