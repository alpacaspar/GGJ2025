public class GameOverMenu : Menu
{
    public void Btn_Retry()
    {
        GameManager.Instance.CurrentState = CurrentState.InGame;
    }

    public void Btn_BackToMenu()
    {
        GameManager.Instance.CurrentState = CurrentState.Menu;
    }
}
