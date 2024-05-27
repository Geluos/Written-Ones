using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class BodyPart
{
    public List<GameObject> gameObjects;
}

public class CreepyShadowConstructor : MonoBehaviour
{
    public List<BodyPart> optionalBodyParts;

    private readonly Random r = new();
    private readonly Dictionary<int, float> bodyPartsNumberDistribution = new Dictionary<int, float>()
    {
        [0] = 0.15f,
        [1] = 0.6f,
        [2] = 0.25f,
    };

    private int GetBodyPartsNumber()
    {
        var probabilitiesSum = bodyPartsNumberDistribution.Values.Sum();
        var normalizedDictionary = bodyPartsNumberDistribution.Select(x => new KeyValuePair<int, float>(x.Key, x.Value / probabilitiesSum));
        var randomFloat = (float)r.NextDouble();
        var cumSum = 0f;
        foreach(var kv in normalizedDictionary)
        {
            cumSum += kv.Value;
            if (randomFloat < cumSum)
                return kv.Key;
        }
        return 0;
    }

    void OnEnable()
    {
        var bodyPartsNumber = GetBodyPartsNumber();
        var chosenBodyPartsObjects = optionalBodyParts.OrderBy(x => r.Next()).Take(bodyPartsNumber)
            .Select(x => x.gameObjects).SelectMany(x => x).ToList();
        foreach(var obj in optionalBodyParts.Select(x => x.gameObjects).SelectMany(x => x))
        {
            if (chosenBodyPartsObjects.Contains(obj))
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
    }


}
