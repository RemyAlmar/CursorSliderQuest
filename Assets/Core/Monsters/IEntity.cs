
public interface IEntity
{
    int Health { get; }
    int Damage { get; }
    bool isMyTurn { get; }
    bool isOccupied { get; }
    
    void Initialize();
    void StartTurn();
    public void TakeDamage(int _damage);
    void Turn(IEntity _entity);
}
