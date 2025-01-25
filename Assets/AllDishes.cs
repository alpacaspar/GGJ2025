using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllDishes", menuName = "Scriptable Objects/AllDishes")]
public class AllDishes : ScriptableObject
{
    public List<RestaurantMenuItem> allMainDish;
    public List<RestaurantMenuItem> allSideDish;
    public List<RestaurantMenuItem> allDrink;
}
