using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllDishes", menuName = "Scriptable Objects/AllDishes")]
public class AllDishes : ScriptableObject
{
    [SerializeField] private List<RestaurantMenuItem> list;

    public RestaurantMenuItem GetRandom()
    {
        return list[Random.Range(0, list.Count)];
    }
}
