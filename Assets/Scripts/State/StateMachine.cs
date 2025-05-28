using System.Collections.Generic;

public class StateMachine
{
    public Dictionary<PlayerEState, BaseState> playerStateDic;

    public BaseState CurState;

    public StateMachine()
    {
        playerStateDic = new Dictionary<PlayerEState, BaseState>();
    }

    // State ÀüÀÌ
    public void ChangeState(BaseState changedState)
    {
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
