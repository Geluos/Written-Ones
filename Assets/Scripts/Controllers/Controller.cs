using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T main { get; private set; }

	public Controller()
	{
		main = (this as T);
	}

}