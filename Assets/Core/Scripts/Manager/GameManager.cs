using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private Player player;
    private List<IAction> actions;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new();
            return instance;
        }
    }

    public void EnemyDefeated(IEntity _enemy)
    {

    }
    public void EndTurn()
    {
        foreach (IAction action in actions) { action.Execute(); }
        monster.Turn(player);
    }

}
