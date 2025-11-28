using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour, IEntity
{
    public int health = 100;
    public int damage = 10;

    public int Health { get => health; }
    public int Damage { get => damage; }


    public AudioClip hitSound;
    public AudioClip attackSound;

    Coroutine dieRoutine;
    public void Initialize()
    {
        health = 100;
        damage = 10;
    }
    public void StartTurn()
    {
        // Placeholder for any start turn logic
    }

    public void Turn(IEntity player)
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