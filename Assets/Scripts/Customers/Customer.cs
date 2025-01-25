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

    [Header("Hunger")]
    [SerializeField] private float maxHunger = 100;
    [SerializeField] private float currentHunger = 100;
    [SerializeField] private float hungerDecreaseRate = 1;
    [SerializeField] private float tier1Hunger = 70;
    [SerializeField] private float tier2Hunger = 40;
    [SerializeField] private float tier3Hunger = 10;
    [SerializeField] private AnimationCurve hungerCurve;
    [SerializeField] private int hungerState;

    [Header("Jump Animation")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration = 1f;

    private bool isSeated = false;
    private float elapsedTime = 0f;

    [Header("SpeakBubble")]
    [SerializeField] private TextMeshProUGUI speakBubble;

    private void Awake()
    {
        if (randomSpeed)
            agent.speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Start()
    {
        GetComponent<TypingEffect>().StartTypingEffect(speakBubble, hungerState);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float hungerRate = hungerCurve.Evaluate(elapsedTime);

        currentHunger -= hungerRate * Time.deltaTime;

        if (currentHunger <= tier3Hunger)
            hungerState = 3;
        else if (currentHunger <= tier2Hunger)
            hungerState = 2;
        else if (currentHunger <= tier1Hunger)
            hungerState = 1;

        if (targetChair != null && agent.enabled == true && !isSeated)
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
    }
}
