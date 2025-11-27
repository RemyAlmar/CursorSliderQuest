using UnityEngine;

public interface IEntity
{
    public void TakeDamage(int _damage);
    void Turn(IEntity _entity);
}
