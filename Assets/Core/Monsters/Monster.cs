using UnityEngine;

public class Monster : MonoBehaviour, IEnemy
{
    public int health = 100;
    public int damage = 10;
    public void Turn(IPlayer player)
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

// /////////// A RETIRER ///////////
public class GameManager
{
    public static GameManager Instance;

    public void EnemyDefeated(Monster monster) { }
}

public interface IPlayer
{
    void TakeDamage(int amount);
}
// /////////// A RETIRER ///////////