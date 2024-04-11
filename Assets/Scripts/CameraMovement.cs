using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 0.02f;

    public void MoveCamera(Vector3 target, Quaternion targetQ)
    {
        StartCoroutine(MoveToTarget(target, targetQ));
    }

    IEnumerator MoveToTarget(Vector3 target, Quaternion targetQ)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQ, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
