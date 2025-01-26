#connector
 ... ... ... 

#banter
$$void Start(): {print(idleBanter);}
$$void Update(): {TapLeg();}
public static event Action OnSmokeBreakStarted;
public bool isChairOccupied = true;
if (isSeated && currentHunger > 0) elapsedTime += Time.deltaTime;
isSeated = true;

#calling
if (!isFood): CallWaiter();
if (hunger >= hunger_target) GetFood();
await Interact();
public bool CanInteract(IInteractable interactor);
isWaiting = true;
$$void QueueDesiredFood(order): { foodQueue.clear(); foodQueue.Append(order); }

#orders
var newOrder = {00};
await firstOrder == {00} && secondOrder == {01};
$$public String order: set(menuItem) { if (!order): return;  order = menuItem; pickMenuItem(order); } get: return {{00}};
QueueDesiredFood({00});

#impatient
$$private void Update() { UpdateTime(); }
$$while (currentPenalty >= maxPenalty) { yield return new WaitForSeconds(scoreUpdateTimeValue); currentScore += (int)scoreUpdateValue; if(scoreText != null) scoreText.text = currentScore.ToString(); }
currentPenalty++;
$$public void GetPenalty() { currentPenalty++; if(penaltyText != null) penaltyText.text = currentPenalty.ToString(); }
currentTime += Time.deltaTime;
currentPenalty = maxPenalty;
isHungry = true;