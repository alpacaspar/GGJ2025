using UnityEngine;

public class Table : MonoBehaviour
{
    public bool IsTableOccupied { get; private set; }
    public Chair[] chairs;

    public void Occupy()
    {
        IsTableOccupied = true;
    }

    public void Leave()
    {
        IsTableOccupied = false;
    }
}
