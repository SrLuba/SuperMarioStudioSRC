using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]public class SpriteAnimationData : ScriptableObject
{
    public string name;
    public List<Sprite> sprites;
    public List<SpriteAnimation> animations;
    public float colSX, colSY, colOX, colOY, property1;
}
