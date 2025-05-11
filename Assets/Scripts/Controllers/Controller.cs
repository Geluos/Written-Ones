using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T main { get; private set; }

	public Controller()
	{
		if (main == null)
			main = (this as T);
		else
			Destroy(this);
	}

}