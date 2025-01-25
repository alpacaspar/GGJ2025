using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Chair targetChair;

    [SerializeField] private NavMeshAgent agent;

    [Header("Patience")]
    [Range(1f, 20f)]
    [SerializeField] private int minPatience = 5;
    [Range(1f, 20f)]
    [SerializeField] private int maxPatience = 5;
    [SerializeField] private int patience = 5;

    [Header("Jump Animation")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration = 1f;

    private bool isSeated = false;

    private void Awake()
    {
        patience = Random.Range(minPatience, maxPatience);
    }

    private void Update()
    {
        if (isSeated)
            return;

        if (targetChair != null && agent.enabled == true)
        {
            agent.SetDestination(targetChair.transform.position);
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                StartCoroutine(JumpToChair());
        }
    }

    private IEnumerator JumpToChair()
    {
        agent.isStopped = true;
        agent.enabled = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetChair.transform.position;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = targetChair.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float height = jumpCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * height;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(endPosition, endRotation);
        isSeated = true;
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
