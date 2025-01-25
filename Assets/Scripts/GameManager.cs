using UnityEngine;

public enum CurrentState
{
    Menu,
    InGame,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public event System.Action<CurrentState> OnStateChanged;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
            }

            return instance;
        }
    }

    private CurrentState currentState;
    public CurrentState CurrentState
    {
        get => currentState;
        set
        {
            currentState = value;
            OnStateChanged?.Invoke(currentState);
        }
    }

    private void Start()
    {
        CurrentState = CurrentState.Menu;
    }
}
