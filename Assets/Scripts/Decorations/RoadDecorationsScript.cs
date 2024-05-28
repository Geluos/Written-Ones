using System.Collections.Generic;
using UnityEngine;

public class RoadDecorationsScript : Decorations
{
    public GameObject Cloud1Object;
    public List<Sprite> Cloud1DaySprites;
    public List<Sprite> Cloud1NightSprites;
    public GameObject Cloud2Object;
    public List<Sprite> Cloud2DaySprites;
    public List<Sprite> Cloud2NightSprites;

    public bool Randomize = true;
    public bool Night = false;

    private void Awake()
    {
        Decorate();
    }

    public override void Decorate()
    {
        if (!Randomize)
            return;

        if (Night)
            RandomizeClouds(Cloud1NightSprites, Cloud2NightSprites);
        else
            RandomizeClouds(Cloud1DaySprites, Cloud2DaySprites);
    }

    private void RandomizeClouds(List<Sprite> cloud1Sprites, List<Sprite> cloud2Sprites)
    {
        // Cloud 1
        Cloud1Object.SetActive(true);
        var sr1 = Cloud1Object.GetComponent<SpriteRenderer>();
        sr1.sprite = RandomSprite(cloud1Sprites);

        // Cloud 2
        Cloud2Object.SetActive(true);
        var sr2 = Cloud2Object.GetComponent<SpriteRenderer>();
        do
        {
            sr2.sprite = RandomSprite(cloud2Sprites);
        } while (sr2.sprite == sr1.sprite);
    }
}
