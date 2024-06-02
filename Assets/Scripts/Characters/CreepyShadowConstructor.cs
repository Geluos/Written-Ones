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

[Serializable]
public class BodyPartsNumberProbability
{
    public int bodyPartsNumber;
    public float probability;
}

public class CreepyShadowConstructor : MonoBehaviour
{
    public List<BodyPart> optionalBodyParts;
    public List<BodyPartsNumberProbability> bodyPartsNumberDistribution;

    private readonly Random r = new();

    private int GetBodyPartsNumber()
    {
        var probabilitiesSum = bodyPartsNumberDistribution.Sum(x => x.probability);
        var normalizedCollection = bodyPartsNumberDistribution.Select(x => new KeyValuePair<int, float>(x.bodyPartsNumber, x.probability / probabilitiesSum));
        var randomFloat = (float)r.NextDouble();
        var cumSum = 0f;
        foreach(var kv in normalizedCollection)
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
