using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public TMP_Text lives;
    public TMP_Text hintxt;
    public TMP_Text targetScene;
    public List<string> hints;
    void Start() {
        hintxt.text = hints[Random.Range(0,hints.Count-1)];
        targetScene.text = Global.targetScene;
    }
    void Update()
    {
        lives.text = Global.PlayerState.One.lives.ToString();
    }
}
