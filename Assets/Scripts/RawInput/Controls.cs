using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		//DontDestroyOnLoad (this.gameObject);
		GlobalInput.a =  Keyboard.current.zKey.isPressed|| Keyboard.current.sKey.isPressed;
		GlobalInput.ap = Keyboard.current.zKey.wasPressedThisFrame|| Keyboard.current.sKey.wasPressedThisFrame;
		GlobalInput.b =  Keyboard.current.xKey.isPressed;
		GlobalInput.bp = Keyboard.current.xKey.wasPressedThisFrame;
		GlobalInput.c =  Keyboard.current.cKey.isPressed|| Keyboard.current.dKey.isPressed;
		GlobalInput.cp = Keyboard.current.cKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame;
		GlobalInput.d =  Keyboard.current.aKey.isPressed|| Keyboard.current.leftShiftKey.isPressed;
		GlobalInput.dp =  Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftShiftKey.wasPressedThisFrame;
		GlobalInput.e =  Keyboard.current.sKey.isPressed;
		GlobalInput.ep =  Keyboard.current.sKey.wasPressedThisFrame;
		GlobalInput.f =  Keyboard.current.dKey.isPressed;
		GlobalInput.fp =  Keyboard.current.dKey.wasPressedThisFrame;
		GlobalInput.right = Keyboard.current.rightArrowKey.isPressed;
		GlobalInput.rightp = Keyboard.current.rightArrowKey.wasPressedThisFrame;
		GlobalInput.left = Keyboard.current.leftArrowKey.isPressed;
		GlobalInput.leftp = Keyboard.current.leftArrowKey.wasPressedThisFrame;
		GlobalInput.up = Keyboard.current.upArrowKey.isPressed;
		GlobalInput.upp = Keyboard.current.upArrowKey.wasPressedThisFrame;
		GlobalInput.down = Keyboard.current.downArrowKey.isPressed || Keyboard.current.leftCtrlKey.isPressed;
		GlobalInput.downp = Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.leftCtrlKey.wasPressedThisFrame;
		GlobalInput.start = Keyboard.current.enterKey.isPressed;
		GlobalInput.startp = Keyboard.current.enterKey.wasPressedThisFrame;
		GlobalInput.select = Keyboard.current.spaceKey.isPressed || Keyboard.current.oKey.isPressed || Keyboard.current.dKey.isPressed;
		GlobalInput.selectp = Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.oKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame;
	}
}
