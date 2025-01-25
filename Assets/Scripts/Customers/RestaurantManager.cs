using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    #region Singleton
    public static RestaurantManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion


}