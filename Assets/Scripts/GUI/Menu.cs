using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button defaultSelectedButton;

    protected virtual void OnEnable()
    {
        if (defaultSelectedButton != null)
            defaultSelectedButton.Select();
    }
}
