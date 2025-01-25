using System.Collections;
using UnityEngine;
using UnityEditor;

public class Customer : MonoBehaviour
{
    public Chair targetChair;

    [Header("Patience")]
    [Range(1f, 20f)]
    [SerializeField] private int minPatience = 5;
    [Range(1f, 20f)]
    [SerializeField] private int maxPatience = 5;
    [SerializeField] private int patience = 5;

    private void Awake()
    {
        patience = Random.Range(minPatience, maxPatience);
        StartCoroutine(TimeAliveCoroutine(patience));
    }

    private IEnumerator TimeAliveCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Customer is waiting for " + time + " seconds.");
        Debug.Log("Customer is leaving.");
        targetChair.isChairOccupied = false;
        Destroy(gameObject);
    }
}
