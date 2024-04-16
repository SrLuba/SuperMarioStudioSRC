using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Archivo para definir datos de las fisicas de cada jugador, se puede modificar por personaje dentro de CharacterDB.



[CreateAssetMenu]  public class PlayerPhysicsData : ScriptableObject {
	public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
	public float maxSpeedW, maxSpeedR, maxSpeedSC, acc, dec, skidSpeed;
	public float jumpForce, gravity;
	public float rayDistance;
	public float animFactor;
	public float jumpMomentumFactor = 1f;
	public float momentumFactor = 1f;
	public float coyoteTime = 1f;
	public float jumpForceStomp, jumpForceStumpP;
	public float maxFallSpeed, maxJumpSpeed;
	public float spinJumpForce;
}