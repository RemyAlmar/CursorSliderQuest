using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IEnemy
{
    public int health = 100;
    public int damage = 10;

    public AudioClip hitSound;
    public AudioClip attackSound;

    Coroutine dieRoutine;

    public void Turn(IPlayer player)
    {
        if (attackSound != null)
        {
            SoundManager.Instance.PlaySound(new SoundManager.SoundOptions
            {
                soundType = SoundManager.SoundType.SFX,
                volume = 1f,
                pitch = 1.2f,
                loop = false
            }, attackSound);
        }
        player.TakeDamage(damage);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (hitSound != null)
            {
                SoundManager.Instance.PlaySound(new SoundManager.SoundOptions
                {
                    soundType = SoundManager.SoundType.SFX,
                    volume = 1f,
                    pitch = 0.7f,
                    loop = false
                }, hitSound);
            }
        }
    }

    private void Die()
    {
        if (dieRoutine != null) StopCoroutine(dieRoutine);
        dieRoutine = StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        var option = new SoundManager.SoundOptions
        {
            soundType = SoundManager.SoundType.SFX,
            volume = 1f,
            pitch = 0.7f,
            loop = false
        };

        // Placeholder for death animation duration
        yield return new WaitForSeconds(0.5f);

        if (hitSound != null) SoundManager.Instance.PlaySound(option, hitSound);
        yield return new WaitForSeconds(0.1f);
        option.pitch = 0.5f;
        if (hitSound != null) SoundManager.Instance.PlaySound(option, hitSound);
        yield return new WaitForSeconds(0.1f);
        option.pitch = 0.3f;
        if (hitSound != null) SoundManager.Instance.PlaySound(option, hitSound);
        yield return new WaitForSeconds(0.1f);

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