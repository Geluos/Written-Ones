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

    private Vector3 pathCameraPosition = new(2.167f, 4.07f, -8.341f);
    private Vector3 pathCameraRotation = new(99.08899f, 1.076996f, 0.08999634f);
    private Vector3 marketCameraPosition = new(2.166f, 4.025f, -9.456f);
    private Vector3 marketCameraRotation = new(77.994f, -0.634f, -1.728f);
    private float moveSpeed = 0.5f;
    private float rotateSpeed = 2f;
    private float moveEps = 0.1f;
    private float rotateEps = 0.1f;
    //private bool moveToMarket = true;
    private bool moveToPath = false;

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
        return true;
    }
}
