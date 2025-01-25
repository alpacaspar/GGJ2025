using System.Collections.Generic;
using UnityEngine;

public class AllDishes : MonoBehaviour
{
    public List<RestaurantMenuItem> allMainDish;
    public List<RestaurantMenuItem> allSideDish;
    public List<RestaurantMenuItem> allDrink;

    public static AllDishes instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
