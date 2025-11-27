using UnityEngine;

public interface IEntity
{
    public int MaxHealth { get; }
    public int CurrentHealth { get; }
    public void TakeDamage(int _damage);
    void Turn(IEntity _entity);
}
