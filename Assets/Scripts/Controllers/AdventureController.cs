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

    private Vector3 pathCameraPosition = new(2.206f, 4.07f, -8.341f);
    private Vector3 pathCameraRotation = new(99.08899f, 1.076996f, 0.08999634f);
    private Vector3 marketCameraPosition = new(2.166f, 4.025f, -9.456f);
    private Vector3 marketCameraRotation = new(77.994f, -1.728f, -0.634f);
    private float moveSpeed = 0.5f;
    private float rotateSpeed = 2f;

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
        var currentPos = Camera.main.transform.position;
        var currentRot = Camera.main.transform.rotation;
        Camera.main.transform.position = Vector3.MoveTowards(currentPos, marketCameraPosition, moveSpeed * Time.fixedDeltaTime);
        Camera.main.transform.rotation = Quaternion.Slerp(currentRot, Quaternion.Euler(marketCameraRotation), rotateSpeed * Time.fixedDeltaTime);
    }

    private void MoveToPath()
    {
        var currentPos = Camera.main.transform.position;
        var currentRot = Camera.main.transform.rotation;
        Camera.main.transform.position = Vector3.MoveTowards(currentPos, pathCameraPosition, moveSpeed * Time.fixedDeltaTime);
        Camera.main.transform.rotation = Quaternion.Slerp(currentRot, Quaternion.Euler(pathCameraRotation), rotateSpeed * Time.fixedDeltaTime);
    }

    public void FixedUpdate()
    {
        //MoveToPath();
        //MoveToMarket();
    }

    public bool PlayCard(Card card)
    {
        return true;
    }
}
