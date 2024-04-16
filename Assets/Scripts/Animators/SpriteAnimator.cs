using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class SpriteAnimationBank {
    public bool active;
    public int childIndex;
    public Sprite sprite;
    public float ox, oy;
    public bool fx, fy;
    public float sx, sy;
    public float ex, ey, ez;

    public void Update(Transform parent) {

        if (childIndex > parent.childCount - 1) return;
        Transform target = parent.GetChild(childIndex);
        if (target == null) return;
        
        
        
        SpriteRenderer tryGet = target.GetComponent<SpriteRenderer>();

        if (tryGet != null) { 
            if (sprite!=null) tryGet.sprite = sprite;
            tryGet.flipX = fx;
            tryGet.flipY = fy;
            //tryGet.color = (active) ? new Color(1f,1f,1f,1f) : new Color(1f,1f,1f,0f);
           
        }
        
        target.localPosition = new Vector3(ox, oy, target.localPosition.z);
        target.localEulerAngles = new Vector3(ex, ey, ez);
        target.localScale = new Vector3(sx, sy, target.localScale.z);
    }
}
[System.Serializable]
public class SpriteAnimationFrame {
    public float ox, oy;
    public float sx, sy;
    public bool fx, fy;
    public int sprite;
    public AudioClip clip;
    public Color color = new Color(1f, 1f, 1f, 1f);
    public Vector3 euler;
    public List<SpriteAnimationBank> spriteAnimationBanks;

    public void UpdateBanks(Transform parent) {
        if (spriteAnimationBanks.Count <= 0) return;
        for (int i = 0; i < spriteAnimationBanks.Count; i++) {
            SpriteAnimationBank cb = spriteAnimationBanks[i];
            cb.Update(parent);
        }
    }
}
[System.Serializable] 
public class SpriteAnimation {
    public string name;
    public bool loop;
    
    public float baseAnimation;
    public List<SpriteAnimationFrame> frames;
}

public class SpriteAnimator : MonoBehaviour
{
    public bool play;
    public SpriteRenderer targetRenderer;
    public SpriteAnimationData animationData;
    public float speed = 1f;
    int cAnim = 0;
    int cFrame = 0;
    float mainTimer = 0f;
    public float resultSpeed;
    public void Play(string name, int placeholder) {
        int index = animationData.animations.FindIndex(x => x.name == name);
        if (index < 0) return;
        if (cAnim != index) { cFrame = 0; UpdateFrame(); }

            cAnim = index;
       
    }
    SpriteAnimationFrame cf;
    public void UpdateFrame()
    {
        

        if (cFrame > animationData.animations[cAnim].frames.Count - 1 && animationData.animations[cAnim].loop)
        {
            cFrame = 0;
        }
        else if (cFrame > animationData.animations[cAnim].frames.Count - 1 && !animationData.animations[cAnim].loop)
        {
            cFrame = animationData.animations[cAnim].frames.Count - 1;
        }

        cf.UpdateBanks(targetRenderer.transform);
        if (cf.clip != null) SoundManager.instance.Play(cf.clip);
        mainTimer = 0f;
    }
    public void Update()
    {
        cf = animationData.animations[cAnim].frames[cFrame];
        targetRenderer.sprite = animationData.sprites[cf.sprite];
        targetRenderer.transform.localPosition = new Vector3(cf.ox, cf.oy, 0f);
        targetRenderer.flipX = cf.fx;
        targetRenderer.flipY = cf.fy;
        targetRenderer.color = cf.color;
        targetRenderer.transform.localEulerAngles = cf.euler;
    }
    void FixedUpdate()
    {
        if (!this.play) { mainTimer = 0f; return; }
        mainTimer += Time.deltaTime;
        resultSpeed = 1f / (animationData.animations[cAnim].baseAnimation + speed);
        if (mainTimer >= resultSpeed) {
            cFrame++;
            UpdateFrame();
        }
    }
}
