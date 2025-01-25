using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer Group", menuName = "Scriptable Objects")]
public class CustomerGroup : ScriptableObject
{
    public IReadOnlyList<Customer> Customers => customers;
    [SerializeField] private List<Customer> customers;

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
        return availableChairs >= customers.Count;
    }
}
