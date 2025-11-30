
public interface IEntity
{
    Health Health { get; }
    bool isMyTurn { get; }
    bool isOccupied { get; }

    void Initialize();
    void StartTurn();
    void Turn(IEntity _entity);
}
