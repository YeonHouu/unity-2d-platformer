using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState 
{
    public bool hasPhysics;

    public abstract void Enter();
    public abstract void Update();
    public virtual void FixedUpdate() { }
    public abstract void Exit();
}
