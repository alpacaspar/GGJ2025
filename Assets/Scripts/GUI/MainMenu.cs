using UnityEngine;

public class MainMenu : Menu
{
    public void Btn_Start()
    {
        GameManager.Instance.CurrentState = CurrentState.InGame;
    }

    public void Btn_Quit()
    {
        Application.Quit();
    }
}
