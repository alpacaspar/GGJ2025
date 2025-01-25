using UnityEngine;

[CreateAssetMenu(fileName = "New Customer Group", menuName = "Scriptable Objects")]
public class CustomerGroup : ScriptableObject
{
    public Customer[] customers;

    public bool CanSitAtTable(Table table)
    {
        int availableChairs = 0;
        foreach (Chair chair in table.chairs)
        {
            if (!chair.isChairOccupied)
            {
                availableChairs++;
            }
        }
        return availableChairs >= customers.Length;
    }
}
