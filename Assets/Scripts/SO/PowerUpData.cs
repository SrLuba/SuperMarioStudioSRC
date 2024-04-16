using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]public class PowerUpData : ScriptableObject
{
    [Header("--- GENERAL ---")]

    [Header("Power Up ID")] public int id = 0;
    [Header("Power Up Tier")] public int tier = 0;
    [Header("Power Up Flags")] public List<string> flags;
    [Header("Power Up Object/Prefab")]public GameObject prefab;
    [Header("Power Up Preview Sprite")]public Sprite sprite;
    [Header("Sound that plays when you grab the power up")]public AudioClip grabSound;

    [Header("--- PLAYER ---")]
    [Header("Sprites")] public List<SpriteConfig> sprites;

    [Header("--- MOVEMENT ---")]
    [Header("Does it move?")]public bool move = false;
    [Header("Does it move? | Move Speed")]public float moveSpeed = 16f;
    

    [Header("--- STAR ---")]
    [Header("It is a star?")]public bool star = false;
    [Header("It is a star? | Star effect duration")]public float starTime = 10f;

    [Header("--- BULLET ---")]
    [Header("Bullet | Prefab / ENABLED BY ADDING 'TOSS' TO FLAGS")] public GameObject tossGB;
    [Header("Bullet | Count")] public int tossCount = 1;

    public Sprite getSprite(string reference){
        string[] res = reference.Split('_');
        Debug.Log("PD |RES | "+res[0]);
        int index = sprites.FindIndex(x=>x.identifier==res[0]);
        if(index<0)return null;
        return sprites[index].getSprite(reference);
    }
}
