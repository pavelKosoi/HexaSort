using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Device;
using static MainUIManager;

public class CameraController : MonoBehaviour
{
    public enum CameraPointType
    {
        None,
        Default,
        StackPicking
    }

    [Serializable]
    public class CameraPoint
    {
        public CameraPointType type;
        public Transform point;
    }

    #region Fields
    [SerializeField] CameraPoint[] points;
    [SerializeField] float moveCamTime;

    Camera mainCamera;
    CameraPointType currentPointType;
    Color currentBackGroundColor;
    #endregion

    #region Getters
    public Camera MainCamera => mainCamera;

    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;
        currentBackGroundColor = mainCamera.backgroundColor;
    }

    public void MoveCamera(CameraPointType cameraPoint, Action onComplete = null)
    {
        if (currentPointType == cameraPoint) return;
        if (PointByType(cameraPoint, out var point))
        {
            currentPointType = cameraPoint;
            var seq = DOTween.Sequence();
            seq.Append(mainCamera.transform.DOMove(point.point.position, moveCamTime).SetEase(Ease.Linear));
            seq.Join(mainCamera.transform.DORotateQuaternion(point.point.rotation, moveCamTime).SetEase(Ease.Linear));
        }
    }
    bool PointByType(CameraPointType pointType, out CameraPoint point)
    {
        point = points.FirstOrDefault(p => p.type == pointType);
        return point != null;
    }

    public void SetBackgroundColor(Color color)
    {
        if (currentBackGroundColor == color) return;
        currentBackGroundColor = color;
        mainCamera.DOColor(color, moveCamTime).SetEase(Ease.Linear);
    }
}
