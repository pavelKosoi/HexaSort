using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputState : IState
{
    protected InputManager inputManager;

    protected BaseInputState(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    public virtual void Enter() { }   

    public virtual void Exit() { }    

    public virtual void Tick() { }    
  
}
