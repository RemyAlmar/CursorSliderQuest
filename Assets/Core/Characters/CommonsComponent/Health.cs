using System;
using UnityEngine;

public class Health
{
    private int maxHealth;
    public int currentHealth;
    public bool IsDead => currentHealth <= 0;

    public Action<int> OnTakeDamage;
    public Action OnDie;

    public Action<int> OnHeal;
    public Action OnHealthComplete;

    public Action OnCurrentHealthChanged;
    public Action OnMaxHealthChanged;
    public float HealthNormalized => Mathf.Clamp01((float)currentHealth / maxHealth);
    public void SetMaxHealth(Func<int> _newValue)
    {
        int _previousValue = maxHealth;
        maxHealth = _newValue();
        if (_previousValue != maxHealth)
            OnMaxHealthChanged();
    }

    public Health(int _maxHealth)
    {
        maxHealth = _maxHealth;
        currentHealth = maxHealth;
    }
    public void Reset()
    {
        currentHealth = maxHealth;
        OnCurrentHealthChanged();
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        OnTakeDamage?.Invoke(_damage);
        OnCurrentHealthChanged?.Invoke();
        if (currentHealth <= 0) OnDie?.Invoke();
    }

    public void Heal(int _healValue)
    {
        currentHealth += _healValue;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        OnHeal?.Invoke(_healValue);
        OnCurrentHealthChanged?.Invoke();
        if (currentHealth >= maxHealth) OnHealthComplete?.Invoke();
    }
}
