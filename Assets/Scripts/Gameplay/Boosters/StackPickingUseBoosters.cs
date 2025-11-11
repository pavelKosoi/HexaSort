using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoostersManager;

public abstract class StackPickingUseBoosters : BoosterBase
{
    protected StackPickingUseBoosters(BoosterConfig config) : base(config) { }

    public override void OnActivate()
    {
        GameManager.Instance.InputManager.OnStackPicked = null;

        GameManager.Instance.InputManager.OnStackPicked += OnStackPicked;

        GameManager.Instance.CameraController.MoveCamera(CameraController.CameraPointType.StackPicking);
        GameManager.Instance.CameraController.SetBackgroundColor(ConfigsManager.Instance.ColorsConfig.BoosterCameraBackgroundColor);
        GameManager.Instance.InputManager.SetInputMode<StackPickingState>();
    }

    public override void OnCancel()
    {
        GameManager.Instance.InputManager.OnStackPicked -= OnStackPicked;             
    }

    public override void OnDeactivate()
    {
        GameManager.Instance.CameraController.MoveCamera(CameraController.CameraPointType.Default);
        GameManager.Instance.CameraController.SetBackgroundColor(ConfigsManager.Instance.ColorsConfig.DefaultCameraBackgroundColor);
        GameManager.Instance.InputManager.SetInputMode<DefaultInputState>();

        GameManager.Instance.InputManager.OnStackPicked = null;
    }
}
