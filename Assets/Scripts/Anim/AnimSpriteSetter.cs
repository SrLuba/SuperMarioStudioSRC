using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class SpriteConfig {
    public string identifier;
    public List<Sprite> sprite;

    public Sprite getSprite(string reference){
        string[] res = reference.Split('_');

        int parsed = int.Parse(res[1]);

        if(sprite.Count-1 < parsed) return null;

        return sprite[parsed];
    }
}
public class AnimSpriteSetter : MonoBehaviour
{
    public bool playerAutomaticSet = false;
    public List<SpriteConfig> sprites;
    public SpriteRenderer target;
    public void Update() {
        if (playerAutomaticSet) sprites = Global.PlayerState.One.getSprites();
    }
    public void Set(string id) {
        if (target==null)return;
        string[] res = id.Split('_');
        //Debug.Log("RES | "+res[0]);
        int index = sprites.FindIndex(x=>x.identifier==res[0]);
        if(index<0)return;
        target.sprite = sprites[index].getSprite(id);
    }
}
