using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] public List<CustomerGroup> customerGroups;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Table[] tables;
    [SerializeField] private float spawnRate = 50f;
    [SerializeField] private int customerAmount;

    private Queue<CustomerGroup> waitingQueue = new Queue<CustomerGroup>();

    private void Start()
    {
        foreach (CustomerGroup customerGroup in customerGroups)
            customerAmount += customerGroup.customers.Length;

        StartCoroutine(SpawnCustomerCoroutine());
    }

    private void SpawnCustomerGroup(CustomerGroup customerGroup)
    {
        foreach (Table table in tables)
        {
            if (!table.isTableOccupied && customerGroup.CanSitAtTable(table))
            {
                table.isTableOccupied = true; // Mark the table as occupied
                foreach (Customer customer in customerGroup.customers)
                {
                    GameObject customerObject = Instantiate(customer, spawnPoint.position, Quaternion.identity).gameObject;
                    foreach (Chair chair in table.chairs)
                    {
                        if (!chair.isChairOccupied)
                        {
                            chair.isChairOccupied = true;
                            customerObject.GetComponent<Customer>().targetChair = chair;
                            break;
                        }
                    }
                }
                return;
            }
        }
        waitingQueue.Enqueue(customerGroup);
    }

    private IEnumerator SpawnCustomerCoroutine()
    {
        for (int i = 0; i < customerAmount; i++)
        {
            if (i < customerGroups.Count)
            {
                SpawnCustomerGroup(customerGroups[i]);
            }
            yield return new WaitForSeconds(spawnRate);
        }

        while (waitingQueue.Count > 0)
        {
            CustomerGroup customerGroup = waitingQueue.Dequeue();
            SpawnCustomerGroup(customerGroup);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
