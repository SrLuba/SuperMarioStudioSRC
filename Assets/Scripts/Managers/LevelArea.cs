using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable] public class ShadingState_Light {
    public Light2D light;
    public float intensity;
    public float transitionSpeed;
    public void Update() {
        light.intensity = Mathf.Lerp(light.intensity, intensity, transitionSpeed*Time.deltaTime);
    }
}
[System.Serializable] public class ShadingState_Volume {
    public Volume volume;
    public float bloomIntensity;
    public float chromaticIntensity;
    public float bloomSpeed, chromaticSpeed;
    Bloom selfBloom;
    ChromaticAberration chromaticAberration;
    public void Update() {
       volume.profile.TryGet(out selfBloom);
       volume.profile.TryGet(out chromaticAberration);

       selfBloom.intensity.value = Mathf.Lerp(selfBloom.intensity.value, bloomIntensity, bloomSpeed*Time.deltaTime);
       chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, chromaticIntensity, chromaticSpeed * Time.deltaTime);
    }
}
[System.Serializable] public class ShadingState {
    public List<ShadingState_Light> lights;
    public List<ShadingState_Volume> volumes;

    public void Update() {
        for (int l = 0; l < lights.Count; l++)
        {
            lights[l].Update();
        }
        for (int v = 0; v < volumes.Count; v++)
        {
            volumes[v].Update();
        }
    }
}
public class LevelArea : MonoBehaviour
{
    public ShadingState onState;
	public Vector2 minC, maxC;
	public float generalY = 0f;

    public bool checkPoint;
    public Vector2 marioPosition;
    public BGData bgData;
    public MusicData musicData;

    public string changeScene = "";


    public void Awake()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        onState.Update();
		SceneController.instance.generalY = this.generalY;
        Global.Game.cameraBoundsMin = minC;
		Global.Game.cameraBoundsMax = maxC;
    }
     public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        if (checkPoint){
            Global.Game.SaveState(marioPosition, bgData, musicData);
        }
        if (changeScene!=""){
            SceneController.instance.ChangeSceneRequest(changeScene);
        }

    }
}
