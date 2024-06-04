using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : Controller<GoldController>
{
    public GoldScript goldScript;
    public GoldData goldData;

    void Start()
    {
        goldScript.UpdateValue(goldData.goldValue);
    }

    public void Add(int value)
    {
        goldData.goldValue += value;
        goldScript.UpdateValue(goldData.goldValue);
    }

    public bool Substract(int value)
    {
        if (value > goldData.goldValue)
            return false;
        goldData.goldValue -= value;
        goldScript.UpdateValue(goldData.goldValue);
        return true;
    }
}
