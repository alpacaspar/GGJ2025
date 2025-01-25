using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Customer : MonoBehaviour
{
    [Header("Chair")]
    public Chair targetChair;

    [Header("AI Agent")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool randomSpeed = false;
    [Range(1f, 10f)]
    [SerializeField] private float minSpeed = 1f;
    [Range(1f, 10f)]
    [SerializeField] private float maxSpeed = 1f;

    [Header("Patience")]
    [Range(10f, 60f)]
    [SerializeField] private int minPatience = 5;
    [Range(10f, 60f)]
    [SerializeField] private int maxPatience = 5;
    [SerializeField] private int patience = 5;

    [Header("Jump Animation")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration = 1f;

    private bool isSeated = false;

    [Header("SpeakBubble")]
    [SerializeField] private TextMeshProUGUI speakBubble;

    private void Awake()
    {
        patience = Random.Range(minPatience, maxPatience);

        if (randomSpeed)
            agent.speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Start()
    {
        GetComponent<TypingEffect>().StartTypingEffect(speakBubble, 0);
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
