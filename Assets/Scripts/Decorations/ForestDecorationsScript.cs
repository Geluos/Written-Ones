using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestDecorationsScript : Decorations
{
    private static readonly System.Random random = new();
    public GameObject Tree1Object;
    public List<Sprite> Tree1Sprites;
    public GameObject Tree2Object;
    public List<Sprite> Tree2Sprites;
    public GameObject Tree3Object;
    public List<Sprite> Tree3Sprites;
    public GameObject Cloud1Object;
    public List<Sprite> Cloud1DaySprites;
    public List<Sprite> Cloud1NightSprites;
    public GameObject Cloud2Object;
    public List<Sprite> Cloud2DaySprites;
    public List<Sprite> Cloud2NightSprites;
    public List<GameObject> StickObjects;
    public List<Sprite> StickSprites;
    public List<GameObject> StumpObjects;
    public List<Sprite> StumpSprites;

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

        RandomizeTrees();

        if (Night)
            RandomizeClouds(Cloud1NightSprites, Cloud2NightSprites);
        else
            RandomizeClouds(Cloud1DaySprites, Cloud2DaySprites);

        RandomizeSticks();

        RandomizeStumps();
    }

    private void RandomizeTrees()
    {
        // Tree 1
        Tree1Object.SetActive(true);
        var sr1 = Tree1Object.GetComponent<SpriteRenderer>();
        sr1.sprite = RandomSprite(Tree1Sprites);

        // Tree 2
        Tree2Object.SetActive(true);
        var sr2 = Tree2Object.GetComponent<SpriteRenderer>();
        do
        {
            sr2.sprite = RandomSprite(Tree2Sprites);
        } while (sr2.sprite == sr1.sprite);

        // Tree 3
        Tree3Object.SetActive(true);
        var sr3 = Tree3Object.GetComponent<SpriteRenderer>();
        do
        {
            sr3.sprite = RandomSprite(Tree3Sprites);
        } while (sr3.sprite == sr1.sprite || sr3.sprite == sr2.sprite);
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

    private void RandomizeSticks()
    {
        int n = random.Next(0, StickObjects.Count);
        List<GameObject> stickObjects = new(StickObjects);
        for (int i = 0; i < n; i++)
        {
            var stickObject = RandomListPop(stickObjects);
            stickObject.SetActive(true);
            var sr = stickObject.GetComponent<SpriteRenderer>();

            // Fix for bad stick/sprite combination
            if (stickObject.name == "Stick 3")
            {
                do
                {
                    sr.sprite = RandomSprite(StickSprites);
                } while (sr.sprite.name == "stick_05");
            }
            else
                sr.sprite = RandomSprite(StickSprites);
        }

        foreach (var so in stickObjects)
        {
            so.SetActive(false);
        }
    }

    private void RandomizeStumps()
    {
        int n = random.Next(1, StumpObjects.Count);
        List<GameObject> stumpObjects = new(StumpObjects);
        List<SpriteRenderer> spriteRenderers = new();
        for (int i = 0; i < n; i++)
        {
            var stumpObject = RandomListPop(stumpObjects);
            // Rightmost stump fix: should not be visible if sticks are there
            if (stumpObject.name == "Stump 3" && (StickObjects[0].activeSelf || StickObjects[1].activeSelf))
            {
                stumpObject.SetActive(false);
                continue;
            }

            stumpObject.SetActive(true);
            var sr = stumpObject.GetComponent<SpriteRenderer>();
            do
            {
                sr.sprite = RandomSprite(StumpSprites);
            } while (spriteRenderers.Any(s => s.sprite == sr.sprite));
            spriteRenderers.Add(sr);

            foreach (var so in stumpObjects)
            {
                so.SetActive(false);
            }
        }
    }

    private static Sprite RandomSprite(List<Sprite> sprites)
    {
        return sprites[random.Next(0, sprites.Count)];
    }

    private static T RandomListPop<T>(List<T> lst)
    {
        int idx = random.Next(0, lst.Count);
        T value = lst[idx];
        lst.RemoveAt(idx);
        return value;
    }
}
