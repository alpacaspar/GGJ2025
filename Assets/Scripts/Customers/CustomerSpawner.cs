using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private CustomerGroup[] customerGroups;
    [SerializeField] private Table[] tables;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float spawnRate = 30f;

    private Queue<CustomerGroup> waitingQueue = new();
    private List<Customer> customerInstances = new();

    private IEnumerator spawnCustomersCoroutine;

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }
    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(CurrentState state)
    {
        switch (state)
        {
            case CurrentState.Menu:
            case CurrentState.GameOver:
                if (spawnCustomersCoroutine != null)
                {
                    StopCoroutine(spawnCustomersCoroutine);
                }

                for (int i = customerInstances.Count - 1; i >= 0; i--)
                {
                    Customer customer = customerInstances[i];
                    customerInstances.Remove(customer);
                    Destroy(customer.gameObject);
                }

                waitingQueue.Clear();
                break;

            case CurrentState.InGame:
                spawnCustomersCoroutine = Co_SpawnCustomers();
                StartCoroutine(spawnCustomersCoroutine);
                break;
        }
    }

    private IEnumerator Co_SpawnCustomers()
    {
        yield return new WaitForSeconds(10f);

        while (GameManager.Instance.CurrentState is CurrentState.InGame)
        {
            var newGroup = customerGroups[Random.Range(0, customerGroups.Length)];
            waitingQueue.Enqueue(newGroup);

            if (waitingQueue.Count > 0)
            {
                var vacantTables = tables.Where((t) => !t.IsTableOccupied);
                if (vacantTables.Any())
                {
                    var group = waitingQueue.Dequeue();
                    var table = vacantTables.First();

                    for (int i = 0; i < group.Customers.Count; i++)
                    {
                        foreach (Chair chair in table.chairs)
                        {
                            if (chair.isChairOccupied)
                                continue;

                            var instance = Instantiate(group.Customers[i], spawnPoint.position, Quaternion.identity);

                            instance.targetChair = chair;
                            chair.isChairOccupied = true;

                            customerInstances.Add(instance);

                            break;
                        }
                    }

                    table.Occupy();
                }
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
