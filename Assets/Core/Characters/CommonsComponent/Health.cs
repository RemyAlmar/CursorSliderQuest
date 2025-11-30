using System;
using UnityEngine;

public class Health
{
    private int maxHealth;
    private int currentHealth;
    public bool IsDie => currentHealth <= 0;

    public Action<int> OnTakeDamage;
    public Action OnDie;

    public Action<int> OnHeal;
    public Action OnHealthComplete;

    public Action OnMaxHealthChanged;

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
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        OnTakeDamage?.Invoke(_damage);
        if (currentHealth <= 0) OnDie?.Invoke();
    }

    public void Heal(int _healValue)
    {
        currentHealth += _healValue;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        OnHeal?.Invoke(_healValue);
        if (currentHealth >= maxHealth) OnHealthComplete?.Invoke();
    }
}
