using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WarpType {
    Pipe,
    Warp,
    Door
}
public enum WarpDirection{
    Left,Right,Up,Down
}
public class WarpFinal : MonoBehaviour
{
    public int id;

    [HideInInspector] public WarpDirection exitDir;
    [HideInInspector] public WarpDirection entranceDir;

    public WarpType type;
    public Animator anim;
    public AudioClip warpAC;

    public bool downwards, onlyExit;

    public bool canUngrounded;

    public WarpFinal other;

    public BGData bgExit;

    public LevelArea targetArea;

    public bool musicChange = false;
    public MusicData selfMusic;

    [HideInInspector]public CameraMov camScript;
    public SpriteRenderer warpSprite;

    [HideInInspector]public Transform camera;

    [HideInInspector] public GameObject target;



    public bool Warping = true;
    public void Awake() {
        
        camera=Camera.main.transform;
        camScript=Camera.main.gameObject.GetComponent<CameraMov>();

        float ang = Mathf.Abs(this.transform.eulerAngles.z);
        if (ang==0f) {
            entranceDir = WarpDirection.Down;
            exitDir = WarpDirection.Up;
        }else if (ang==90f) {
            entranceDir = (this.transform.eulerAngles.z<0f) ? WarpDirection.Left : WarpDirection.Right;
            exitDir = (this.transform.eulerAngles.z<0f) ? WarpDirection.Right : WarpDirection.Left;
        }else if (ang==180f) {
           entranceDir = WarpDirection.Up;
           exitDir = WarpDirection.Down;
        }else if (ang==270f) {
            entranceDir = (this.transform.eulerAngles.z<0f) ? WarpDirection.Right : WarpDirection.Left;
            exitDir = (this.transform.eulerAngles.z<0f) ? WarpDirection.Left : WarpDirection.Right;
        }
    }
    public void Start(){
        if(other==null && id<0) other = this;
        if (other==null && id>=0) {
            WarpFinal[] seek = FindObjectsByType<WarpFinal>(FindObjectsSortMode.None);
            List<WarpFinal> warps = new List<WarpFinal>();

            for (int i = 0; i < seek.Length; i++) {
                warps.Add(seek[i]);
            }

            other = warps.Find(x => x.id == this.id && x != this && x.type==this.type);

            if (other==null) other=this;
        }
    }
    public void OnTriggerStay2D(Collider2D col) {
        if (col.tag!= "Player") return;
        if (Warping) return;
        if (!PlayerPhysics2D_Demo.instance.Grounded && !canUngrounded) return;
        if (onlyExit) return;
        if (type==WarpType.Pipe) {
            if (!GlobalInput.down && entranceDir == WarpDirection.Down) return;
            if (!GlobalInput.up && entranceDir == WarpDirection.Up) return;
            if (!GlobalInput.right && entranceDir == WarpDirection.Right) return;
            if (!GlobalInput.left && entranceDir == WarpDirection.Left) return;
            if (downwards) return;
            if (warpSprite!=null) 
            {
                warpSprite.sprite = PlayerPhysics2D_Demo.instance.powerUpController.selfPickup.getSprite("WarpDown_0");
            }
            target = col.gameObject;
            SoundManager.instance.Play(2159, warpAC, 1f, 1f);
            if(anim!=null) anim.Play("Enter", 0, 0f);
            target.SetActive(false);
            Warping=true;
            StartCoroutine(Warp());
        }else if (type==WarpType.Door) {

            if (!GlobalInput.up) return;

            if (warpSprite!=null) warpSprite.sprite = PlayerPhysics2D_Demo.instance.powerUpController.selfPickup.getSprite("WarpUp_0");
            target = col.gameObject;
            SoundManager.instance.Play(2159, warpAC, 1f, 1f);
            if(anim!=null) anim.Play("Enter", 0, 0f);
            target.SetActive(false);
            Warping=true;
            StartCoroutine(Warp());
        }else if (type==WarpType.Warp) {
            if (warpSprite!=null) warpSprite.sprite = PlayerPhysics2D_Demo.instance.powerUpController.selfPickup.getSprite("WarpUp_0");
            target = col.gameObject;
            SoundManager.instance.Play(2159, warpAC, 1f, 1f);
            target.SetActive(false);
            Warping=true;
            StartCoroutine(Warp());
        }

        if (other.warpSprite!=null)target.transform.position =other.warpSprite.transform.position+new Vector3(0f,8f,0f);
    }
    public float playeryoffsetv1,playeryoffsetv2;
    public float playeryoffseth1,playeryoffseth2;
    public void GetBackOBJ() {
        Vector3 off = new Vector3(0f,0f,0f);
        if (exitDir==WarpDirection.Down) off=warpSprite.transform.position-new Vector3(0f,Global.PlayerState.One.powerUp>0 ? 16f+playeryoffsetv2 : 8f+playeryoffsetv1,0f);
        else if (exitDir==WarpDirection.Right) off=warpSprite.transform.position+new Vector3(Global.PlayerState.One.powerUp>0 ? 16f+playeryoffseth2 : 8f+playeryoffseth1,0f,0f);
        else if (exitDir==WarpDirection.Left) off=warpSprite.transform.position-new Vector3(Global.PlayerState.One.powerUp>0 ? 16f+playeryoffseth2 : 8f+playeryoffseth1,0f,0f);
        else off=warpSprite.transform.position+new Vector3(0f,Global.PlayerState.One.powerUp>0 ? 16f+playeryoffsetv2 : 8f+playeryoffsetv1,0f);
        target.transform.position = off;
        target.SetActive(true);
        PlayerPhysics2D_Demo.instance.actualAngle = warpSprite.transform.eulerAngles.z;
        PlayerPhysics2D_Demo.instance.angleT.eulerAngles = new Vector3(0f,0f,warpSprite.transform.eulerAngles.z);
        PlayerPhysics2D_Demo.instance.Reset();
    }
    public IEnumerator Exit() {
        if (musicChange) {
            MusicController.instance.RequestTransition(selfMusic, null, false);
        }
        warpSprite.sprite = PlayerPhysics2D_Demo.instance.powerUpController.selfPickup.getSprite("WarpDown_0");
        SceneController.instance.Transitionate(false, this.transform.position, 1);
        SoundManager.instance.Play(2159, warpAC, 1f, 1f);
        if(anim!=null) anim.Play("Exit", 0, 0f);
        Warping=true;
        yield return new WaitForSeconds(0.5f);
        camScript.canFollow =true;
        Warping=false;
        if (type==WarpType.Warp)GetBackOBJ();
    }

    public IEnumerator Warp() {
        camScript.canFollow =false;
        SceneController.instance.Transitionate(true, this.transform.position, 1);
        yield return new WaitForSeconds(2f);
        if (bgExit!=null) Parallax.instance.LoadBG(bgExit);
        if (targetArea!=null)
        {
            SceneController.instance.generalY = targetArea.generalY;
            Global.Game.cameraBoundsMin = targetArea.minC;
            Global.Game.cameraBoundsMax = targetArea.maxC;
        }
        yield return new WaitForSeconds(1f);
       
        camera.position = new Vector3(Mathf.Clamp(other.transform.position.x, Global.Game.cameraBoundsMin.x, Global.Game.cameraBoundsMax.x),Mathf.Clamp(other.transform.position.y, Global.Game.cameraBoundsMin.y, Global.Game.cameraBoundsMax.y),-100f);
        other.target = this.target;
        if (warpSprite!=null && other.warpSprite!=null) other.warpSprite.sprite = this.warpSprite.sprite;
        
        other.StartCoroutine(other.Exit());
        Warping=false;
    }
}
