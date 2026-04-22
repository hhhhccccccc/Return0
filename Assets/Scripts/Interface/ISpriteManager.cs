using UnityEngine;

public interface ISpriteManager : IManager
{
    public Sprite GetSprite(string spriteName);
}
