using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [Header("Chair")]
    public Chair targetChair;

    [Header("Dishes")]
    [SerializeField] private AllDishes allDishes;

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
    [SerializeField] private float hungerDecreaseRate = 0.15f;
    [SerializeField] private int talkTier = 70;
    [SerializeField] private int secondTalkTier = 20;
    [SerializeField] private int attentionTier = 40;
    [SerializeField] private int orderTier = 10;
    [SerializeField] private AnimationCurve hungerCurve;
    [SerializeField] private int hungerState = 4;

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

        allDishes = AllDishes.instance;
    }


    private void Update()
    {
        if (isSeated && currentHunger > 0)
            elapsedTime += Time.deltaTime;

        float hungerRate = hungerCurve.Evaluate(elapsedTime);

        if (isSeated && currentHunger > 0)
            currentHunger -= hungerRate * Time.deltaTime;

        int previousHungerState = hungerState;

        if (currentHunger > talkTier)
            hungerState = 4;
        else if (currentHunger > secondTalkTier)
            hungerState = 4;
        else if (currentHunger > attentionTier)
            hungerState = 3;
        else if (currentHunger > orderTier)
            hungerState = 2;
        else
            hungerState = 1;

        if (hungerState != previousHungerState)
        {
            GetComponent<TypingEffect>().StartTypingEffect(speakBubble, hungerState, allDishes);
        }

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
