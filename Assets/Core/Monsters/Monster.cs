using UnityEngine;

public class Monster : MonoBehaviour, IEntity
{
    public int health = 100;
    public int damage = 10;

    public int MaxHealth => throw new System.NotImplementedException();

    public int CurrentHealth => throw new System.NotImplementedException();

    public void Turn(IEntity player)
    {
        player.TakeDamage(damage);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TO DO: Add death animation or effects here
        GameManager.Instance.EnemyDefeated(this);
    }
}