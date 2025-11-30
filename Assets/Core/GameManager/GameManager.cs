using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform interfaceUI;
    [SerializeField] private IEntity enemyEntity;
    [SerializeField] private IEntity playerEntity;
    [SerializeField] private Player playerPrefab;
    private List<Slot> slots = new List<Slot>();
    public bool anySlotRegistered => slots.Count > 0;

    public List<Monster> enemyPrefabs;

    public static GameManager Instance { get; private set; }
    public static bool CanDoAction => Instance.enemyEntity != null && Instance.playerEntity != null && !Instance.enemyEntity.Health.IsDie && !Instance.playerEntity.Health.IsDie;

    // Inputs
    private RaycastHit hitInfo;
    private Camera mainCamera;
    private IClickable lastClickable;
    private IClickable currentClickable;
    private float clickThreshold = 0.3f;
    private float lastClickTime = 0f;
    private bool lastPressedOnUI = false;


    // Fight Management
    public int turnCountThisFight = 0;
    public float startTurnDelay = 0.5f;
    public bool inFight = false;

    // Out Fight Management
    public SlotVisual selectedSlotVisual;
    public Vector3 currentClickOffset;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
        interfaceUI.gameObject.SetActive(true);
    }

    void Update()
    {
        bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        bool isMouseButtonDown = Input.GetMouseButtonDown(0);
        bool isMouseButtonUp = Input.GetMouseButtonUp(0);
        bool isMouseButtonPressed = Input.GetMouseButton(0);
        bool isAnyMouseButtonActive = isMouseButtonDown || isMouseButtonUp || isMouseButtonPressed;

        if (isAnyMouseButtonActive)
        {
            bool hasHit = RaycastMouseToWorld(out hitInfo);
            IClickable hitClickable = hasHit ? hitInfo.collider.GetComponent<IClickable>() : null;

            if (isMouseButtonDown)
            {
                if (isPointerOverUI)
                {
                    lastPressedOnUI = true;
                }
                else
                {
                    lastPressedOnUI = false;
                    if (hitClickable != null && hitClickable != currentClickable)
                    {
                        hitClickable.OnCursorDown();
                        currentClickable = hitClickable;
                        lastClickTime = Time.time;
                    }
                }
            }

            if (!lastPressedOnUI && isMouseButtonPressed)
            {
                if (hitClickable != currentClickable)
                {
                    currentClickable?.OnCursorExit();

                    if (hitClickable != null)
                    {
                        hitClickable.OnCursorEnter();
                    }

                    currentClickable = hitClickable;
                }
            }

            if (isMouseButtonUp)
            {
                if (!lastPressedOnUI)
                {
                    if (lastClickable != null && currentClickable != lastClickable)
                    {
                        lastClickable?.OnClickOutside();
                    }

                    if (currentClickable != null)
                    {
                        currentClickable.OnCursorUp();

                        if (Time.time - lastClickTime <= clickThreshold)
                            currentClickable.OnClick();

                        lastClickable = currentClickable;
                        currentClickable = null;
                    }

                    if (selectedSlotVisual != null && selectedSlotVisual.TryGetComponent(out IClickable slotClickable) && slotClickable != currentClickable)
                    {
                        selectedSlotVisual.OnCursorUp();
                        selectedSlotVisual = null;
                    }
                }

                lastPressedOnUI = false;
            }
        }

        // Update Player
        if (playerEntity is Player player)
        {
            if (inFight)
                playerEntity.Turn(enemyEntity);
            else
                player.OutFightUpdate();
        }

    }

    #region Raycast Input Helpers
    private Ray ray;
    public bool RaycastMouseToWorld(out RaycastHit hitInfo)
    {
        if (Input.mousePosition.x is float.NegativeInfinity or float.PositiveInfinity ||
           Input.mousePosition.y is float.NegativeInfinity or float.PositiveInfinity)
        {
            hitInfo = new RaycastHit();
            return false;
        }
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hitInfo);
    }
    #endregion

    private void SpawnPlayer()
    {
        playerEntity = Instantiate(playerPrefab);
        playerEntity.Initialize();
    }

    private void SpawnEnemy()
    {
        enemyEntity = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)]);
        enemyEntity.Initialize();
    }

    public void RegisterSlot(Slot _slot)
    {
        if (slots == null)
            slots = new List<Slot>();
        slots.Add(_slot);
    }

    public void GenerateFight()
    {
        Debug.Log("Generating Fight...");
        if (playerEntity == null)
        {
            SpawnPlayer();
        }

        inFight = true;
        turnCountThisFight = 0;
        SpawnEnemy();
        playerEntity.StartTurn();
    }

    public void StopFight()
    {
        Debug.Log("Fight Stopped.");
        inFight = false;
        foreach (Slot slot in slots)
        {
            slot.ResetFight(playerEntity, enemyEntity);
        }
        slots.Clear();

        if (playerEntity != null && playerEntity.Health.IsDie)
        {
            Debug.Log("Player has been defeated. Restarting the game...");
        }
    }

    public void EnemyDefeated(IEntity _enemy)
    {
        turnCountThisFight = 0;

        Destroy(((MonoBehaviour)_enemy).gameObject);

        if (playerEntity is Player player)
        {
            Debug.Log("Fight Ended. Resetting Player.");
            player.OutFight();
        }

        LaunchInterface();
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnRoutine());
    }

    IEnumerator EndTurnRoutine()
    {
        Debug.Log("---- End of Turn ----");
        yield return new WaitForSeconds(0.5f);
        turnCountThisFight++;
        foreach (Slot slot in slots)
        {
            Debug.Log("Executed Slot: " + slot.action.GetType().Name);
            slot.ExecuteEndTurn(playerEntity, enemyEntity);

            if (!inFight)
            {
                yield break;
            }

            Debug.Log("Enemy is occupied, waiting...");
            yield return new WaitUntil(() => !enemyEntity.isOccupied);
            Debug.Log("Enemy is free to act now.");

            slot.ResetTurn(playerEntity, enemyEntity);
        }
        slots.Clear();

        Debug.Log("---- Enemy Turn ----");
        enemyEntity.Turn(playerEntity);

        Debug.Log("Waiting for enemy to finish turn...");
        yield return new WaitUntil(() => !enemyEntity.isMyTurn);
        Debug.Log("Enemy turn finished.");

        yield return new WaitForSeconds(startTurnDelay);
        playerEntity.StartTurn();
    }

    private void LaunchInterface()
    {
        interfaceUI.gameObject.SetActive(true);
    }

    // Out Fight Slot Selection
    internal void SelectSlot(SlotVisual slotVisual)
    {
        selectedSlotVisual = slotVisual;
        if (slotVisual != null)
        {
            currentClickOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - slotVisual.transform.position;
        }
    }
}
