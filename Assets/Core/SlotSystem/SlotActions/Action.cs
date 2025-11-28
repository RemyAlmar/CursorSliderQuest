using System;
using UnityEngine;

[Serializable]
public abstract class Action : IAction
{
    [SerializeField] protected bool canBeDeactivated;
    public ActionState state { get; set; } = ActionState.Neutral;
    public int activationsThisTurn { get; set; } = 0;
    public int activationsThisFight { get; set; } = 0;
    public bool CanBeDeactivated => canBeDeactivated;

    public abstract void Execute(IEntity executor, IEntity target);
}