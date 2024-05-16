using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardsAnimationType
{
    FadeIn, FadeOut
}

public class CameraMoveOnClick : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public Vector3 target;
    public Vector3 targetRotation;

    public GameObject hand;
    public CardsAnimationType cardsAnimation;

    private void OnMouseUp()
    {
        var targetQ = new Quaternion();
        targetQ.eulerAngles = targetRotation;

        if (cardsAnimation == CardsAnimationType.FadeOut)
        {
            hand.GetComponent<CardsLayout>().FadeOut(() =>
            {
                cameraMovement.MoveCamera(target, targetQ);
            });
        }

        if (cardsAnimation == CardsAnimationType.FadeIn)
        {
            cameraMovement.MoveCamera(target, targetQ, () =>
            {
                hand.GetComponent<CardsLayout>().FadeIn();
            });
        }
    }

}
