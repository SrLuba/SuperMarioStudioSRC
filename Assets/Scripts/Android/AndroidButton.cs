using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AndroidButton : MonoBehaviour
{

    bool animToggle = false;
    public Image selfImage;
    public Sprite s_active, s_inactive;

    public int selfID = 0;
    public bool inGame = false;

    int jumpFix = 0;

    public void Start()
    {
       // if (Application.platform != RuntimePlatform.Android) Destroy(this.gameObject);
    }
    public void OnMouseDown()
    {
        Clicked();
    }
    public void Clicked()
    {
        if (!inGame || Application.platform != RuntimePlatform.Android) return;
        if (selfID == 4)
        {
            PlayerPhysics2D_Demo.instance.StartJump();
            PlayerPhysics2D_Demo.instance.WallJumpUpdate(true);
        }
        else if (selfID == 9) {
            PlayerPhysics2D_Demo.instance.Burst();
        }
    }
    public void Update()
    {
        if (selfImage != null) { 
            selfImage.sprite = animToggle ? (s_active) : (s_inactive);
        }
    }

    public void Press(int id) {
        switch (id) {
            case 0:
                GlobalInput.up = true;
                break;
            case 1:
                GlobalInput.right = true;
                break;
            case 2:
                GlobalInput.down = true;
                break;
            case 3:
                GlobalInput.left = true;
                break;
            case 4:
                if (!GlobalInput.a) PlayerPhysics2D_Demo.instance.Jump();
               GlobalInput.a = true;
                
               // GlobalInput.ap = true;
                break;
            case 5:
                GlobalInput.b = true;
               // GlobalInput.bp = true;
                break;
            case 6:
                if (!GlobalInput.c) PlayerPhysics2D_Demo.instance.SpinJump();
                     GlobalInput.c = true;
              //  GlobalInput.cp = true;
                break;
            case 7:
                GlobalInput.start = true;
                break;
            case 8:
                GlobalInput.select = true;
                break;
            case 9:
                GlobalInput.dp = true;
                GlobalInput.d = true;
                break;
        }
    }
    public void Release(int id) {
        switch (id)
        {
            case 0:
                GlobalInput.up = false;
                break;
            case 1:
                GlobalInput.right = false;
                break;
            case 2:
                GlobalInput.down = false;
                break;
            case 3:
                GlobalInput.left = false;
                break;
            case 4:
                GlobalInput.a = false;
              //  GlobalInput.ap = false;
                break;
            case 5:
                GlobalInput.b = false;
              //  GlobalInput.bp = false;
                break;
            case 6:
                GlobalInput.c = false;
              //  GlobalInput.cp = false;
                break;
            case 7:
                GlobalInput.start = false;
                break;
            case 8:
                GlobalInput.select = false;
                break;
            case 9:
                GlobalInput.dp = false;
                GlobalInput.d = false;
                break;
        }
    }

    public void Toggle(int id) {
        animToggle = !animToggle;
        switch (id)
        {
            case 0:
                GlobalInput.up =!GlobalInput.up;
                break;
            case 1:
                GlobalInput.right = !GlobalInput.right;
                break;
            case 2:
                GlobalInput.down = !GlobalInput.down;
                break;
            case 3:
                GlobalInput.left = !GlobalInput.left;
                break;
            case 4:
                GlobalInput.a = !GlobalInput.a;
                break;
            case 5:
                GlobalInput.b = !GlobalInput.b;
                break;
            case 6:
                GlobalInput.c = !GlobalInput.c;
                break;
            case 7:
                GlobalInput.start = !GlobalInput.start;
                break;
            case 8:
                GlobalInput.select = !GlobalInput.select;
                break;
            case 9:
                GlobalInput.d = !GlobalInput.d;
                break;
        }

    }
}
