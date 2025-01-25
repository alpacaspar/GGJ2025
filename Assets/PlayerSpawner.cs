using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private GameObject playerInstance;

    private void OnEnable()
    {
        GameManager.OnStateChanged += Instance_OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= Instance_OnStateChanged;
    }

    private void Instance_OnStateChanged(CurrentState state)
    {
        switch (state)
        {
            case CurrentState.Menu:
            case CurrentState.GameOver:
                if (playerInstance != null)
                {
                    Destroy(playerInstance);
                }
                break;

            case CurrentState.InGame:
                if (playerInstance == null)
                {
                    playerInstance = Instantiate(playerPrefab, transform.position, Quaternion.identity);
                }
                break;
        }
    }
}
