using UnityEngine;

public enum MonsterEState
{
    Idle, Walk, Die
}

public class EnemyState : BaseState
{
    protected Enemy enemy;

    public EnemyState(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
