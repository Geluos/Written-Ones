using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AdventureController : Controller<AdventureController>
{
	public Deck pathDeck;
	public GameObject hand;
	public GameObject pathCard;
    public GameObject notificationDialog;
    public GameObject chooseDialog;

    public void Start()
	{
		LoadPathDeck();
	}

	public void LoadPathDeck()
	{
        foreach (var card in pathDeck.cards)
        {
            var cardObject = Instantiate(pathCard, hand.transform);
            cardObject.GetComponent<PathCardScript>().card = card.copy();
            cardObject.GetComponent<PathCardScript>().UpdateView();
        }
    }

    private void MoveToMarket()
    {
        var cameraPosition = Camera.main.transform.position;
        var cameraRotation = Camera.main.transform.rotation;
/*        if (Vector3.Distance(cameraPosition, marketCameraPosition) < moveEps && Quaternion.Angle(cameraRotation, Quaternion.Euler(marketCameraRotation)) < rotateEps)
        {
            moveToMarket = false;
            moveToPath = true;
            return;
        }*/
        Camera.main.transform.SetPositionAndRotation(
            Vector3.MoveTowards(Camera.main.transform.position, marketCameraPosition, moveSpeed * Time.fixedDeltaTime), 
            Quaternion.Slerp(Camera.main.transform.rotation, Quaternion.Euler(marketCameraRotation), rotateSpeed * Time.fixedDeltaTime));
    }

    private void MoveToPath()
    {
        var cameraPosition = Camera.main.transform.position;
        var cameraRotation = Camera.main.transform.rotation;
/*        if (Vector3.Distance(cameraPosition, pathCameraPosition) < moveEps && Quaternion.Angle(cameraRotation, Quaternion.Euler(pathCameraRotation)) < rotateEps)
        {
            moveToPath = false;
            moveToMarket = true;
            return;
        }*/
        Camera.main.transform.SetPositionAndRotation(
            Vector3.MoveTowards(cameraPosition, pathCameraPosition, moveSpeed * Time.fixedDeltaTime), 
            Quaternion.Slerp(cameraRotation, Quaternion.Euler(pathCameraRotation), rotateSpeed * Time.fixedDeltaTime));
    }

    public void FixedUpdate()
    {
		/*if (moveToPath)
		   MoveToPath();

	  if (moveToMarket)
		   MoveToMarket();*/
	}

	public bool PlayCard(Card card)
    {
        foreach (var effect in card.effectsList)
            effect.Activate();
        return true;
    }
}
