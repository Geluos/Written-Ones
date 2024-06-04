using UnityEngine;

[CreateAssetMenu(fileName = "GoldData", menuName = "GoldData/GoldData", order = -1)]
public class GoldData : ScriptableObject
{
    [SerializeField]
    public int goldValue;
}
