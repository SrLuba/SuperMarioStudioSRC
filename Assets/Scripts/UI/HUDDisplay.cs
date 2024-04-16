using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDDisplay : MonoBehaviour
{
	public NumberFont coins,timer,lives,world,score;
	
	float ttimer;
	bool timing = true;
    void Update()
    {
        world.value = Global.PlayerState.CurrentLevel.world.ToString();
		timer.value = Global.PlayerState.CurrentLevel.timer.ToString();
		lives.value = Global.PlayerState.One.lives.ToString();
		score.value = Global.PlayerState.score.ToString().PadLeft(9, '0');

        coins.value = Global.PlayerState.coins.ToString();
		if (timing) {
			ttimer+=Time.deltaTime;
			if (ttimer>=1f) {
				Global.PlayerState.CurrentLevel.timer--;
				
				if (Global.PlayerState.CurrentLevel.timer<=0) {
					timing=false;
				}
				ttimer=0f;
			}
		}
    }
}
