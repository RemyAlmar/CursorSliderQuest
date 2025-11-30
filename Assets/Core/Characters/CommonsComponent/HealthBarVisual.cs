using System;
using UnityEngine;


public class HealthBarVisual : MonoBehaviour
{
    public Vector2 size = new(5, 1);
    public float speedLerp = .5f;
    public Transform currentHealthContainer;

    public Gradient gradientColorHealth;
    [Range(0, 1)] public float targetScaleCurrentHealth = 1;

    private SpriteRenderer spriteCurrentHealth;

    public void Awake()
    {
        spriteCurrentHealth = currentHealthContainer.GetComponentInChildren<SpriteRenderer>();
    }

    public void Update() => UpdateBar();
    public void SetSizeBar() => transform.localScale = size;
    public void SetSize(float _value) => targetScaleCurrentHealth = Mathf.Clamp01(_value);
    public void UpdateBar()
    {
        Vector2 _target = new(targetScaleCurrentHealth, currentHealthContainer.localScale.y);
        currentHealthContainer.localScale = Vector2.Lerp(currentHealthContainer.localScale, _target, speedLerp * Time.deltaTime);
        spriteCurrentHealth.color = gradientColorHealth.Evaluate(currentHealthContainer.localScale.x);
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        SetSizeBar();
        if (spriteCurrentHealth == null)
            spriteCurrentHealth = currentHealthContainer.GetComponentInChildren<SpriteRenderer>();

        spriteCurrentHealth.color = gradientColorHealth.Evaluate(targetScaleCurrentHealth);
    }
#endif
}
