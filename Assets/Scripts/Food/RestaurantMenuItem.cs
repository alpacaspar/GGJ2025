using UnityEngine;

[CreateAssetMenu(fileName = "MenuItem", menuName = "Scriptable Objects/MenuItem")]
public class RestaurantMenuItem : ScriptableObject
{
    [field: SerializeField] public MenuType MenuType { get; private set; }
    [field: SerializeField] public Sprite ItemSprite { get; private set; }
}
