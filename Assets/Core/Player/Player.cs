using System;
using NUnit.Framework.Internal;
using UnityEngine;

public class Player : MonoBehaviour, IEntity
{
    public int health = 100;
    public int damage = 15;
    public int Health { get => health; }
    public int Damage { get => damage; }

    public IAction[] actions;

    public DoActionButton doActionButton;

    public void Initialize()
    {
        health = 100;
        damage = 15;
        doActionButton.player = this;

        actions = new IAction[]
        {
            new Action_Slash()
        };
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Debug.Log("Player Defeated!");
        }
    }

    public void Turn(IEntity _entity)
    {

    }

    internal void DoAction()
    {
        GameManager.Instance.RegisterAction(actions[0]);
        EndTurn();
    }

    private void EndTurn()
    {
        GameManager.Instance.EndTurn();
    }
}
