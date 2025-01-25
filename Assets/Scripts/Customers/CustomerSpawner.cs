using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Table[] tables;

    [SerializeField] private float spawnRate = 5f;
    [SerializeField] private float customerAmount;

    private void Start()
    {
        StartCoroutine(SpawnCustomerCoroutine());
    }

    private void SpawnCustomer()
    {
        foreach (Table table in tables)
        {
            foreach (Chair chair in table.chairs)
            {
                if (!chair.isChairOccupied)
                {
                    GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);

                    chair.isChairOccupied = true;

                    customer.GetComponent<Customer>().targetChair = chair;

                    return;
                }
            }
        }
    }

    private IEnumerator SpawnCustomerCoroutine()
    {
        for (int i = 0; i < customerAmount; i++)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
