using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveOnClick : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public Vector3 target;
    public Vector3 targetRotation;

    private void OnMouseUp()
    {
        Debug.Log("Click");
        var targetQ = new Quaternion();
        targetQ.eulerAngles = targetRotation;
        cameraMovement.MoveCamera(target, targetQ);
    }

}
