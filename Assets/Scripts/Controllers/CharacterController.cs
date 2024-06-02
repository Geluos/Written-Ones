using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class CharacterController : Controller<CharacterController>
{
	public List<GameObject> oneMonsterPointer = new List<GameObject>();
	public List<GameObject> twoMonsterPointers = new List<GameObject>();
	public List<GameObject> threeMonsterPointers = new List<GameObject>();

	public void Start()
	{

	}
}
