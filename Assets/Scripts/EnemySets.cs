using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemySets", menuName = "EnemySets/EnemySets", order = -1)]
public class EnemySets : ScriptableObject
{
	[SerializeField]
	public List<ESet> enemySets;
}

[System.Serializable]
public class ESet
{
	public string nameId;
	public List<Enemy> enemies;
	public int cost;
	public bool isBoss = false;
}
