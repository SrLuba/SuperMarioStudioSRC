using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
	public bool canFollow = true;
	public float speed;
	public Vector2 min,max;
	public Transform target;
	public Vector2 offset;
	public float modifiableOffset, multOffsetMod;
	void Start() {
		//if (Global.Game.SaveData.playerPositionChange && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name==Global.Game.SaveData.Scene) this.transform.position = Global.Game.SaveData.playerPosition;

		this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x,min.x,max.x), Mathf.Clamp(this.transform.position.y,min.y,max.y));
	}
    void Update()
    {
    	if (PlayerPhysics2D_Demo.instance==null) this.enabled=false;
    	if (target==null) target = PlayerPhysics2D_Demo.instance.gameObject.transform;
        if (target==null) return;
	
		min = Global.Game.cameraBoundsMin;
		max = Global.Game.cameraBoundsMax;
        

        if (canFollow)
		{
			Vector3 targetPos = new Vector3(Mathf.Clamp(target.position.x+offset.x+(multOffsetMod*modifiableOffset),min.x,max.x), Mathf.Clamp(target.position.y+offset.y,min.y,max.y), -100f);
			this.transform.position = Vector3.Lerp(new Vector3(Mathf.Clamp(this.transform.position.x, min.x, max.x),Mathf.Clamp(this.transform.position.y, min.y, max.y), -100f), targetPos,speed*Time.deltaTime);
		} 
	}
}
