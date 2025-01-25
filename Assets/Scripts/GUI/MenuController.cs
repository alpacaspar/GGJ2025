using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Menu mainMenu;
    [SerializeField] private Menu gameOverMenu;

    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStateChanged -= Instance_OnStateChanged;
    }
    
    private void Instance_OnStateChanged(CurrentState state)
    {
        mainMenu.gameObject.SetActive(state is CurrentState.Menu);
        gameOverMenu.gameObject.SetActive(state is CurrentState.GameOver);
    }
}
