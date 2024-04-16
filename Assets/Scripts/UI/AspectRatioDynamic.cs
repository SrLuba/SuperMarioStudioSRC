using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class ResolutionSetting {
    public int width = 800;
    public int height = 600;
    public string hudReference = "4:3";
    public int hud = 0;
    public Animator hud_capter;
}
public class AspectRatioDynamic : MonoBehaviour
{
    public static AspectRatioDynamic instance;
    public Animator hud_capter;


    public List<ResolutionSetting> resolutions;
    public List<GameObject> huds;

    public bool forceRes = false;
    public bool Fullscreen = true;

    public string resolutionIndex = "16:9";

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        
    }

    public void Update()
    {
        if (Camera.main.aspect >= 1.7f)
        {
            ChangeRes("16:9", true);
        }
        else if (Camera.main.aspect >= 1.33f)
        {
            ChangeRes("4:3", true);
        }
        else {
            ChangeRes("8:7", true);
        }
    }
    public void UpdateRes() {
        int sid = resolutions.FindIndex(x => x.hudReference == resolutionIndex);
        if (sid < 0) return;
        for (int i = 0; i < huds.Count; i++) { huds[i].SetActive(false); }
        huds[resolutions[sid].hud].SetActive(true);
        if (forceRes) Screen.SetResolution(resolutions[sid].width, resolutions[sid].height, Fullscreen);
        this.hud_capter = resolutions[sid].hud_capter;
    }
   
    public void ChangeRes(string id, bool fullscreen) { if (id == resolutionIndex) { return; } resolutionIndex = id; Fullscreen = fullscreen; UpdateRes(); }
}
