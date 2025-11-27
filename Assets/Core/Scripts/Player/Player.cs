using UnityEngine;

public class Player : MonoBehaviour, IEntity
{
    public int MaxHealth => throw new System.NotImplementedException();

    public int CurrentHealth => throw new System.NotImplementedException();

    public void TakeDamage(int _damage)
    {
        throw new System.NotImplementedException();
    }

    public void Turn(IEntity _entity)
    {
        throw new System.NotImplementedException();
    }
}
