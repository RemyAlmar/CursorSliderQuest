using UnityEngine;

public interface IEntity
{
    int Health { get; }
    int Damage { get; }
    
    void Initialize();
    public void TakeDamage(int _damage);
    void Turn(IEntity _entity);
}
