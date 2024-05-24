using System.Collections.Generic;
using UnityEngine;

public class Decorations : MonoBehaviour
{
    protected static readonly System.Random random = new();
    public virtual void Decorate() { }

    protected static Sprite RandomSprite(List<Sprite> sprites)
    {
        return sprites[random.Next(0, sprites.Count)];
    }
}
