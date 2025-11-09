using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Fields
    StateMachine stateMachine;

    public Action<Stack> OnStackPicked;
    #endregion

    #region Getters
    public StateMachine StateMachine => stateMachine;
    #endregion

    #region Properties
    public Camera MainCamera {  get; private set; }
    #endregion

    void Awake()
    {
        MainCamera = Camera.main;
        stateMachine = new StateMachine();

        stateMachine.RegisterState(new DefaultInputState(this));
        stateMachine.RegisterState(new StackPickingState(this));
    }
    private void Start()
    {
        stateMachine.ChangeState<DefaultInputState>();
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void SetInputMode<T>() where T : BaseInputState
    {
        stateMachine.ChangeState<T>();
    }
}
