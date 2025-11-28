using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IEntity enemy;
    [SerializeField] private IEntity player;
    [SerializeField] private Player playerPrefab;
    private List<IAction> actions = new List<IAction>();
    private RaycastHit hitInfo;
    private Camera mainCamera;
    private IClickable lastClickable;
    private IClickable currentClickable;
    private float clickThreshold = 0.3f;
    private float lastClickTime = 0f;
    private bool lastPressedOnUI = false;

    public List<Monster> enemyPrefabs;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
        SpawnPlayer();
        SpawnEnemy();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab);
        player.Initialize();
    }

    private void SpawnEnemy()
    {
        enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)]);
        enemy.Initialize();
    }

    public void RegisterAction(IAction _action)
    {
        if (actions == null)
            actions = new List<IAction>();
        actions.Add(_action);
    }

    public void EnemyDefeated(IEntity _enemy)
    {
        Destroy(((MonoBehaviour)_enemy).gameObject);
        SpawnEnemy();

        player.Initialize();
    }

    public void EndTurn()
    {
        foreach (IAction action in actions)
        {
            action.Execute(player, enemy);
        }
        actions.Clear();

        enemy.Turn(player);
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
                }

                lastPressedOnUI = false;
            }
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
}
