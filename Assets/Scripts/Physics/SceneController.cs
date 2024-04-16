using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    SCENE CONTROLLER MADE BY LUBA
    
    THIS MAKES THE TRANSITIONS WORK

    THIS SCRIPT IS ENTIRELY PRIVATE
    IF YOU ARE SEEING THIS TEXT PLEASE CONTACT DISCORD USER srluba
 */

[System.Serializable] public class Transition {
    [Header("Prefab Reference")] public GameObject objectReference;
    [Header("Speed")][Range(0.1f,2f)] public float speed = 1f;
    GameObject gNew;
    public bool Transitionate(bool outorin, Vector3 origin, Transform parent) {

        if (gNew != null) {
            MonoBehaviour.Destroy(gNew);
            gNew = null;
        }
        gNew = MonoBehaviour.Instantiate(objectReference, origin, Quaternion.identity);
        gNew.GetComponent<Animator>().speed = speed;
        gNew.GetComponent<Animator>().Play(outorin ? "In" : "Out", 0, 0f);

        gNew.transform.SetParent(parent);

        return true;
    }
}
public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public OverlayMenu GCmenu;

	public float generalY = 0f;
    [Header("Transitions")]public List<Transition> transitions;

    public void Awake()
    {
        instance = this;
    }
    public void Start() {
        if(Global.Game.SaveData.playerPositionChange && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name==Global.Game.SaveData.Scene) PlayerPhysics2D_Demo.instance.gameObject.transform.position = Global.Game.SaveData.playerPosition;
    }

    public void GrandCoin() {
        if(GCmenu==null)return;
        GCmenu.ShowRequest(2f);
    }
    public void ChangeSceneRequest(string name) {
        StartCoroutine(ChangeScene(name));
    }
    public IEnumerator ChangeScene(string name) {
        Transitionate(true, PlayerPhysics2D_Demo.instance.gameObject.transform, 0);
        yield return new WaitForSeconds(2f);
        Global.LoadScene(name);
    }

    public void Transitionate(bool outorin, Vector3 origin, int id)
    {
        transitions[id].Transitionate(outorin, origin, this.transform);
    }
    public void Transitionate(bool outorin, Transform origin, int id)
    {
        transitions[id].Transitionate(outorin, origin.position, origin);
    }
}
