using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button defaultSelectedButton;

    private void OnEnable()
    {
        if (defaultSelectedButton != null)
            defaultSelectedButton.Select();
    }
}
