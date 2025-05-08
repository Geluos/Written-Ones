using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public static bool showShop = true;

    public GameObject hand;
    public CardsAnimationType cardsAnimation;

    private bool showHand = true;

    private void OnMouseUp()
    {
		//if (!FightController.main.AdventureScene.gameObject.active || MenuController.main.isMovement || MenuController.main.menu.activeInHierarchy)
		//	return;
        var targetQ = new Quaternion();
        targetQ.eulerAngles = targetRotation;
		MenuController.main.isMovement = true;
		cameraMovement.MoveCamera(target, targetQ);
        showHand = !showHand;

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
