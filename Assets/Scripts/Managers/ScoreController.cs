using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class ScoreItem {
    public Sprite sprite;
    public AudioClip clip;
    public float pitch;
    public int value;
    public int UpCount;
}
public class ScoreController : MonoBehaviour
{
    public static ScoreController instance;
    public List<ScoreItem> items;
    public GameObject pointDrop;
    public PlayerPhysics2D_Demo player;
    public float spareTime = 1f;
    bool canOne = true;
    public void Awake()
    {
        if (instance == null) instance = this;
    }
    public void Update()
    {
        canOne = (Global.PlayerState.One.timer <= 0f);
        if (canOne) Global.PlayerState.One.pointStreak = 0;

        if (player.Grounded && canOne) Global.PlayerState.One.pointStreak = 0;
    }
    public void FixedUpdate()
    {
        if (Global.PlayerState.One.timer > 0f) Global.PlayerState.One.timer -= Time.deltaTime;
    }
    public void GetPointsExplicit(int points, AudioClip AC, Sprite sprite, Vector3 position) {

        Instantiate(pointDrop, position, Quaternion.identity).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;

        if (AC!=null) SoundManager.instance.Play(5, AC, 1f, 1f);
        Global.PlayerState.score += points;
        
    }
    public void GetPointsExplicit(int points, AudioClip AC, Sprite sprite, Vector3 position, Vector3 scale) {

        GameObject obj = Instantiate(pointDrop, position, Quaternion.identity);

        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
        obj.transform.localScale = scale;
        if (AC!=null) SoundManager.instance.Play(5, AC, 1f, 1f);
        Global.PlayerState.score += points;
        
    }
    public void GetPoints(bool soundEffect, bool streak, int player, Vector3 position) {
        ScoreItem resultItem = (Global.PlayerState.One.pointStreak >= items.Count - 1) ? items[items.Count - 1] : items[Global.PlayerState.One.pointStreak];

        Instantiate(pointDrop, position, Quaternion.identity).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = resultItem.sprite;

        if (soundEffect) SoundManager.instance.Play(5, resultItem.clip, 1f, resultItem.pitch);

        Global.PlayerState.score += resultItem.value;
        Global.PlayerState.One.timer = spareTime;
        if (streak)
        {
            Global.PlayerState.One.pointStreak++;
            Global.PlayerState.One.lives += resultItem.UpCount;
        }
        else {
            Global.PlayerState.One.pointStreak=0;
        }
    }
}
