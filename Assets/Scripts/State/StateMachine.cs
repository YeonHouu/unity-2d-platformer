using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Dictionary<PlayerEState, BaseState> playerStateDic;
    public Dictionary<EnemyEState, BaseState> enemyStateDic;

    public BaseState CurState;

    public StateMachine()
    {
        playerStateDic = new Dictionary<PlayerEState, BaseState>();
        enemyStateDic = new Dictionary<EnemyEState, BaseState>();
    }

    // State ÀüÀÌ
    public void ChangeState(BaseState changedState)
    {
        //Debug.Log($"Changing state to {CurState}");
        if (CurState == changedState)
        {
            return;
        }

        CurState.Exit();

        CurState = changedState;

        CurState.Enter();
    }

    public void Update() => CurState.Update();

    public void FixedUpdate()
    {
        if (CurState.hasPhysics)
        {
            CurState.FixedUpdate();
        }
    }
}
