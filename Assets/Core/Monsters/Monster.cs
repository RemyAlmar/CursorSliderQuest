using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour, IEntity
{
    public int health = 100;
    public int damage = 10;

    public int Health { get => health; }
    public int Damage { get => damage; }
    public bool isMyTurn { get; set; } = false;
    public bool isOccupied { get; set; } = false;


    public AudioClip hitSound;
    public AudioClip attackSound;

    private Animator animator;
    private int animAttackHash = Animator.StringToHash("Attack");
    private int animHitHash = Animator.StringToHash("Hit");
    [SerializeField] private float hitAnimTime = 0.2f;
    private int animDeathHash = Animator.StringToHash("Death");
    [SerializeField] private float deathAnimTime = 1.5f;

    Coroutine dieRoutine;
    public void Initialize()
    {
        health = 100;
        damage = 10;

        // Initialize Animator
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.Rebind();
        isMyTurn = false;
    }
    public void StartTurn()
    {
        // Placeholder for any start turn logic
    }

    public void Turn(IEntity player)
    {
        StartCoroutine(TurnRoutine(player));
    }

    IEnumerator TurnRoutine(IEntity player)
    {
        isMyTurn = true;
        animator.SetTrigger(animAttackHash);

        yield return new WaitForSeconds(1f);
        
        player.TakeDamage(damage);
        if (!GameManager.Instance.inFight)
        {
            isMyTurn = false;
            yield break;
        }

        isMyTurn = false;
    }

    public void TakeDamage(int amount)
    {
        isOccupied = true;
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger(animHitHash);
            StartCoroutine(OccupiedRoutine(hitAnimTime));
        }
    }

    IEnumerator OccupiedRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isOccupied = false;
    }

    private void Die()
    {
        GameManager.Instance.StopFight();
        if (dieRoutine != null) StopCoroutine(dieRoutine);
        dieRoutine = StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        animator.SetTrigger(animDeathHash);

        // Placeholder for death animation duration
        yield return new WaitForSeconds(deathAnimTime);

        isOccupied = false;
        GameManager.Instance.EnemyDefeated(this);
    }

    // Sounds
    public void PlayAttackSound()
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
    }
    public void PlayHitSound()
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
    public void PlayDeathSound(float pitch)
    {
        if (hitSound != null)
        {
            SoundManager.Instance.PlaySound(new SoundManager.SoundOptions
            {
                soundType = SoundManager.SoundType.SFX,
                volume = 1f,
                pitch = pitch,
                loop = false
            }, hitSound);
        }
    }
}