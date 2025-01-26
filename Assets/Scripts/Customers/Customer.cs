using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Chair")]
    public Chair targetChair;

    [Header("Dishes")]
    [SerializeField] private AllDishes allDishes;
    public List<RestaurantMenuItem> orderList;

    [Header("AI Agent")]
    [SerializeField] private bool randomSpeed = false;
    [Range(1f, 10f)]
    [SerializeField] private float minSpeed = 1f;
    [Range(1f, 10f)]
    [SerializeField] private float maxSpeed = 1f;
    private float speed;

    [Header("Hunger")]
    [SerializeField] private float currentHunger = 100;
    [SerializeField] private float hungerDecreaseRate = 0.15f;
    [SerializeField] private int talkTier1 = 70;
    [SerializeField] private int secondTalkTier4 = 20;
    [SerializeField] private int attentionTier2 = 40;
    [SerializeField] private int orderTier3 = 10;
    [SerializeField] private int angerTier4 = 10;

    [SerializeField] private int hungerLevel = 0;
    [SerializeField] private AnimationCurve hungerCurve;
    [SerializeField] private int hungerState = 4;

    [Header("Jump Animation")]
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration = 1f;

    private bool isSeated = false;
    private float elapsedTime = 0f;
    private bool isHungerFrozen = false;

    [Header("SpeakBubble")]
    [SerializeField] private TextMeshProUGUI speakBubble;
    private TypingEffect typingEffect;

    // Add reference to CustomerSpawner and spawnPoint
    private Vector3 spawnPoint;
    private bool isMovingAway = false;

    private void Awake()
    {
        if (randomSpeed)
            speed = Random.Range(minSpeed, maxSpeed);
        else
            speed = minSpeed;

        typingEffect = GetComponent<TypingEffect>();

        // Find the CustomerSpawner in the scene and get the spawnPoint
        spawnPoint = transform.position;

        talkTier1 = AdjustTier(talkTier1);
        secondTalkTier4 = AdjustTier(secondTalkTier4);
        attentionTier2 = AdjustTier(attentionTier2);
        orderTier3 = AdjustTier(orderTier3);
        angerTier4 = AdjustTier(angerTier4);
    }

    private void Update()
    {
        if (isSeated && currentHunger > 0 && !isMovingAway && !isHungerFrozen)
            elapsedTime += Time.deltaTime;

        float hungerRate = hungerCurve.Evaluate(elapsedTime);

        if (isSeated && currentHunger > 0 && !isHungerFrozen)
            currentHunger -= hungerRate * Time.deltaTime;

        CheckHungerTiers();

        typingEffect.ChangeColorToRed(currentHunger);

        if (targetChair != null && !isSeated && !isMovingAway)
        {
            MoveTowardsTarget(targetChair.transform.position);
        }

        if (transform.position == targetChair.transform.position)
        {
            isSeated = true;
            targetChair.isChairOccupied = true;
        }

        if (currentHunger <= 0 && !isMovingAway)
        {
            isMovingAway = true;
        }

        if (isMovingAway)
        {
            Debug.Log("Attempting to move away");
            MoveTowardsTarget(spawnPoint);

            if (transform.position == spawnPoint)
            {
                typingEffect.PopBubble();
                targetChair.isChairOccupied = false;
                Destroy(gameObject);
            }
        }
    }

    private int AdjustTier(int tier)
    {
        float adjustment = Random.Range(-0.1f, 0.1f);
        return Mathf.RoundToInt(tier * (1 + adjustment));
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void CheckHungerTiers()
    {
        int newHungerState = hungerState;

        if (currentHunger <= angerTier4)
            newHungerState = 4;
        else if (currentHunger <= orderTier3)
            newHungerState = 3;
        else if (currentHunger <= secondTalkTier4)
            newHungerState = 1;
        else if (currentHunger <= attentionTier2)
            newHungerState = 2;
        else if (currentHunger <= talkTier1)
            newHungerState = 1;
        else if (currentHunger > talkTier1)
            newHungerState = 0;

        if (newHungerState != hungerState)
        {
            hungerState = newHungerState;
            typingEffect.StartTypingEffect(speakBubble, hungerState, allDishes);
        }
    }

    private IEnumerator FreezeHunger()
    {
        isHungerFrozen = true;
        yield return new WaitForSeconds(5f);
        isHungerFrozen = false;
    }

    // Call this method when the customer gets an item
    public void OnItemReceived()
    {
        StartCoroutine(FreezeHunger());
    }

    //private IEnumerator JumpOffChair()
    //{
    //    Vector3 startPosition = transform.position;
    //    Vector3 endPosition = startPosition + transform.forward * -1; // Move one unit forward

    //    float elapsedTime = 0f;

    //    while (elapsedTime < jumpDuration)
    //    {
    //        float t = elapsedTime / jumpDuration;
    //        float height = jumpCurve.Evaluate(t);

    //        transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * height;

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    transform.SetPositionAndRotation(endPosition, Quaternion.identity);
    //    isSeated = false;
    //    typingEffect.PopBubble();
    //    StopCoroutine(JumpOffChair());
    //    MoveTowardsTarget(spawnPoint);
    //}
}
