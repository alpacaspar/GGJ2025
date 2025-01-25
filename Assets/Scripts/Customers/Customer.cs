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
    [SerializeField] private float currentHunger = 100;
    [SerializeField] private float hungerDecreaseRate = 0.15f;
    [SerializeField] private int talkTier = 70;
    [SerializeField] private int secondTalkTier = 20;
    [SerializeField] private int attentionTier = 40;
    [SerializeField] private int orderTier = 10;
    [SerializeField] private int angerTier = 10;

    [SerializeField] private int hungerLevel = 0;
    [SerializeField] private AnimationCurve hungerCurve;
    [SerializeField] private int hungerState = 4;

    [Header("Jump Animation")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration = 1f;

    private bool isSeated = false;
    private float elapsedTime = 0f;

    [Header("SpeakBubble")]
    [SerializeField] private TextMeshProUGUI speakBubble;
    private TypingEffect typingEffect;

    // Add reference to CustomerSpawner and spawnPoint
    public CustomerSpawner customerSpawner;
    private Transform spawnPoint;

    private void Awake()
    {
        if (randomSpeed)
            agent.speed = Random.Range(minSpeed, maxSpeed);

        allDishes = AllDishes.instance;

        typingEffect = GetComponent<TypingEffect>();

        // Find the CustomerSpawner in the scene and get the spawnPoint
        if (customerSpawner != null)
        {
            spawnPoint = customerSpawner.spawnPoint;
        }
    }

    private void Update()
    {
        if (isSeated && currentHunger > 0)
            elapsedTime += Time.deltaTime;

        float hungerRate = hungerCurve.Evaluate(elapsedTime);

        if (isSeated && currentHunger > 0)
            currentHunger -= hungerRate * Time.deltaTime;

        CheckHungerTiers();

        typingEffect.ChangeColorToRed(currentHunger);

        if (targetChair != null && agent.enabled == true && !isSeated)
        {
            agent.SetDestination(targetChair.transform.position);
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                StartCoroutine(JumpToChair());
        }

        // Check if currentHunger is less than or equal to 0
        if (currentHunger <= 0)
        {
            StartCoroutine(JumpOffChair());
        }
    }

    private void CheckHungerTiers()
    {
        int newHungerState = hungerState;

        if (currentHunger <= angerTier)
            newHungerState = 4;
        else if (currentHunger <= orderTier)
            newHungerState = 3;
        else if (currentHunger <= secondTalkTier)
            newHungerState = 1;
        else if (currentHunger <= attentionTier)
            newHungerState = 2;
        else if (currentHunger <= talkTier)
            newHungerState = 1;
        else if (currentHunger > talkTier)
            newHungerState = 0;

        if (newHungerState != hungerState)
        {
            hungerState = newHungerState;
            typingEffect.StartTypingEffect(speakBubble, hungerState, allDishes);
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

    private IEnumerator JumpOffChair()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.forward * -1; // Move one unit forward
        Quaternion startRotation = transform.rotation;

        // Find the nearest point on the NavMesh to the endPosition
        NavMeshHit hit;
        if (NavMesh.SamplePosition(endPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            endPosition = hit.position;
        }

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float height = jumpCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * height;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(endPosition, Quaternion.identity);
        isSeated = false;
        MoveBackToSpawnPoint();
    }

    private void MoveBackToSpawnPoint()
    {
        StopCoroutine(JumpOffChair());
        if (spawnPoint != null)
        {
            agent.enabled = true;
            agent.SetDestination(spawnPoint.position);
        }
    }
}
