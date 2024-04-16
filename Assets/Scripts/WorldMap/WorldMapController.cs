using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldMapController : MonoBehaviour
{
    public bool Halt;
    public static WorldMapController instance;
    public WorldMapData data;

    public Transform player;
    public float speedPlayer;
    public Vector3 playerOffset;

    public int index = 0;
    float progress = 0f;
    public AudioClip MoveAC, EnterAC, OpenInvAC;
    public TMP_Text lvln_text, lvld_text;
    public IEnumerator EnterLevel() {
        this.Halt = true;
        SoundManager.instance.Play(EnterAC);
        TransitionManager.instance.Transition(player, true, 0);
        yield return new WaitForSeconds(1.1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(data.items[index].LevelScene);
    }
    void Awake()
    {
        if (instance == null) instance = this;
    } 
    // Start is called before the first frame update
    void Start()
    {
        //LE.Audio.Music.Play(0, data.music);
        player.position = data.items[0].position;
    }

    private void FixedUpdate()
    {
        
        Vector3 targetPosPlayer = data.items[index].position+ playerOffset;

        if (Vector3.Distance(player.position, targetPosPlayer) > 0.1f)
        {
            player.position = Vector3.MoveTowards(player.position, targetPosPlayer, speedPlayer*Time.deltaTime);
            progress += Time.deltaTime;
        }
        else {
            progress = 0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        lvln_text.text = data.items[index].Name;
        lvld_text.text = data.items[index].Description;
        Vector3 targetPosPlayer = data.items[index].position+playerOffset;

        if (Vector3.Distance(player.position, targetPosPlayer) <= 1f && !Halt) { 
            if (GlobalInput.rightp)
            {
                if (data.items[index].right != -1) {
                    index = data.items[index].right;
                    SoundManager.instance.Play(MoveAC);
                }
            }
            if (GlobalInput.leftp)
            {
                if (data.items[index].left != -1)
                {
                    index = data.items[index].left;
                    SoundManager.instance.Play(MoveAC);
                }
            }
            if (GlobalInput.downp)
            {
                if (data.items[index].down != -1)
                {
                    index = data.items[index].down;
                    SoundManager.instance.Play(MoveAC);
                }
            }
            if (GlobalInput.upp)
            {
                if (data.items[index].up != -1)
                {
                    index = data.items[index].up;
                    SoundManager.instance.Play(MoveAC);
                }
            }
            if (GlobalInput.ap && data.items[index].LevelScene!="") {
                StartCoroutine(EnterLevel());
            }
        }
    }
}
