using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhysics2D_Demo : MonoBehaviour
{
	public static PlayerPhysics2D_Demo instance;


	public PowerUpController_Demo powerUpController;
	public Sprite cWarpSprite;
	public Sprite StylishSprite;
	public  Sprite defaultcWarpSprite;
	//Propiedades cambiables en tiempo real
	[Header("Las siguientes propiedades son los halts (activar para que algo deje de funcionar), sirve para hacer un Animation Override (por ejemplo poner la animacion de powerup)")]
	public bool animationHalt = false; 
	public bool physicsHalt = false;

	[Header("Grab")]
	public bool grab = false;
	public SpriteRenderer grabSpriteRenderer;
	public Sprite writeGrabSprite;
	public GameObject grabObject;

    public List<string> flags;

	public float gravityWater, normalGravity;
    public LayerMask enemyMask;
	public PlayerPhysicsData physics, physicsWater, physicsNormal;

	float momentum = 0f;
	public Animator mainAnim;
	//Aqui tienes una variable que referencia a la camara de tipo CameraMov, puedes buscarlo en la carpeta Physics/CameraMov.cs
	public CameraMov cam;
	//Prefab del polvo cuando te deslizas.
	public GameObject FX;
	public Vector3 FXOffset;
	//Referencia del transform del sprite del jugador
	public Transform spriteT;
	public Transform angleT;
    //Animador del jugador
    //public Animator mainAnim;
    //Fisicas/Rigidbody2D del jugador
    public Rigidbody2D rb;
	//PowerUp actual del jugador (deberia estar en global pero es solo para lo early)
	public int PowerUp;
	// Variables de fisicas (eventualmente deberian ponerse en un archivo (ScriptableObject))
	public LayerMask floorMask;
	public LayerMask groundPoundingMask;
	public LayerMask wallMask;
	public LayerMask playerMask;
	public Vector2 floorOffset;
	// Esta en el suelo?
	public bool Grounded;
	//public Vector2 instantiateOffset;
	// Esto es para modificar el offset de la camara horizontalemente dependiendo del input del jugador
	public float offsetFactor;
	// Velocidad horizontal actual de las fisicas del jugador.
	public float curSpeed;
	// Input del jugador, se actualiza con la variable estatica GlobalInput, puedes checkearla en Controls.cs, es para mejor compatibilidad con controles y demas.
	Vector2 horInput;
	// Se explica solo, el float especifica cada cuanto va a sonar el sonido del skid y aparezca el efecto en el piso.
	float timerSkid;
	// esta deslizandose? (cambiando de direccion abruptamente), pasa cuando estas apretando el lado contrario al que estas yendo (HorInput.x < 0f && curSpeed > 0f), ejemplo de uso.
	bool skid = false;
	// esta dado vuelta horizontalmente?, usado para saber para que lado van los proyectiles y a su vez para que lado mira el jugador.
	public bool mirrored = false;
	public bool groundPoundAttack = false;

	public float actualAngle = 0f;
	public SpriteRenderer shellRend;
	public AudioClip JumpAC, SkidAC, SpinJumpAC, JumpWaterAC, BurstAC;
	bool toss = false;
	float tossTimer = 0f;
	bool spinJump=false;

	public float offsetVX = 0f;


	// Special Movement Effects - They Affect Physics When Active
	[Header("Special Movement Effects - They Affect Physics When Active")] public bool S_Swim = false;
	[Header("Special Movement Effects - They Affect Physics When Active")] public bool S_Duck = false;

	

	public Vector2 SmallColSize, BigColSizeNoCrouch, BigColSizeCrouch;
	public Vector2 SmallColOffset, BigColOffsetNoCrouch, BigColOffsetCrouch;


	public bool sliding = false;
	public float sliding_exit_cooldown = 1f;
	public bool isOnSlidable = false;


	public void Awake() {
		instance = this;
		Global.PlayerState.dead = false;
		cWarpSprite = defaultcWarpSprite;
	}
	// MOBILE - StartJump (Starts jump as the function name implies)
	bool startedJump = false;
	public void StartJump()
	{
		Jump();
	}

	public AudioClip GroundPoundASFX, GroundPoundBSFX;
	public float GroundPoundSpeed;
	bool groundPounding=false;
	public void doGroundPound(bool androidFix) {
		if (!androidFix) if (!GlobalInput.downp) return;
		if (Grounded)return;
		if (grab) return;
		if (groundPounding)return;
		if (groundPoundAttack)return;
		

		StartCoroutine(GroundPound());
	}
	public GameObject GroundPoundFXA;
	public GameObject GroundPoundFXB;
	[HideInInspector]public float floorY = 0f;
	public float groundPoundingYOffset;
	public IEnumerator GroundPound() {
		animationHalt=true;
		physicsHalt=true;
		groundPounding=true;
		SCParticleOne.Play();
		SCParticleTwo.Play();
		rb.velocity = new Vector2(0f, 0f);
		rb.isKinematic=true;
		groundPoundAttack=true;
		SoundManager.instance.Play(259265, GroundPoundASFX,1f,1f);
		PlayAnimForce("GroundPound");
		Instantiate(GroundPoundFXA, this.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.25f);

		while(this.transform.position.y > floorY+32f){
			if (!groundPounding) yield break;
			this.transform.Translate(0f,-GroundPoundSpeed*Time.deltaTime,0f);
			yield return new WaitForSeconds(0.001f);
		}
		if (!groundPounding) yield break;
		this.transform.position = new Vector3(this.transform.position.x, floorY+groundPoundingYOffset+(float)((Global.PlayerState.One.powerUp>0) ? 0f : 8f), this.transform.position.z);
		curSpeed=0f;
		rb.velocity = new Vector2(0f, 333f);
		Instantiate(GroundPoundFXB, this.transform.position, Quaternion.identity);
		SoundManager.instance.Play(259265, GroundPoundBSFX,1f,1f);
		animationHalt=false;
		physicsHalt=false;
		rb.isKinematic=false;
		groundPounding=false;

		yield return new WaitForSeconds(0.1f);
		groundPoundAttack=false;
	}
	public void Reset() {
		PlayAnimForce  ("Idle");
		rb.isKinematic	= false;
		burstHalt	    = false;
		didBurst	    = false;
		spinJump	    = false;
		groundPoundAttack=false;
		spinJumped	    = false;
		animationHalt   = false;
		physicsHalt     = false;
		jumped          = false;
		groundPounding  = false;
		sliding			= false;
		SCParticleOne.Stop();
		SCParticleTwo.Stop();
		Global.PlayerState.dead = false;
	}
	public void Start() {
		Reset();
	}
	// void Toss, usa powerup (animation halt / toss)
	public void Toss() {
		toss=true;
		canToss=false;
	}
	bool didBurst = false;
	int burstCount = 0;
	public void Burst() {
		if (Grounded || didBurst) return;
		StartCoroutine(doBurst());
	}
	public GameObject burstParticle;
	bool burstHalt = false;
	public int wallJumpCancelBurstCount = 4;
	int wallJumpCancelIndex = 0;
	public AudioClip streakBurstAC;
	public IEnumerator doBurst(){
		animationHalt=true;
		burstCount++;
		groundPoundAttack=false;
		if (burstCount > 3) {
			ScoreController.instance.GetPointsExplicit(250, null, StylishSprite, this.transform.position+new Vector3(0f,8f,0f), new Vector3(0.5f,0.5f,0.5f));
			SoundManager.instance.Play(242, streakBurstAC, 0.5f, 1f);
		}else {
			ScoreController.instance.GetPoints(false, false, 1, this.transform.position+new Vector3(0f,8f,0f));
			SoundManager.instance.Play(242, BurstAC, 0.5f, 1f);
		}

		didBurst=true;
		burstHalt=true;
		
		
		Instantiate(burstParticle,this.transform.position,Quaternion.identity);
	 	PlayAnimForce("Burst");
	 	rb.velocity = new Vector2(rb.velocity.x, 0f);
	 	
		if (wallJumpCancelIndex < wallJumpCancelBurstCount) {
			wallJumpTimer=0f;
			lWallJumpSide = false;
			rWallJumpSide = false;
		}
		wallJumpCancelIndex++;
		yield return new WaitForSeconds(0.1f);
		burstHalt=false;
		animationHalt=false;
		yield return new WaitForSeconds(0.2f);
	 	didBurst=false;
	 	
	 	
	}

    void UpdateToss() {
		if (toss) {
			tossTimer+=Time.deltaTime;
			if (tossTimer>=0.25f) {
				toss=false;
				tossTimer=0f;
                canToss = true;
            }
		}
	}
	
	
	public void crouchUpdate() {
		if (Global.PlayerState.One.powerUp==0) {S_Duck = false; return;}
		if (GlobalInput.down && Grounded && !S_Duck) S_Duck=true;
		if (!GlobalInput.down) S_Duck = false;
		
		if (S_Duck) {
			curSpeed=Mathf.Lerp(curSpeed,0f,5f*Time.deltaTime);
			skid=false;
		}
		
	}
	public bool rd_l, rd_m, rd_r;
	bool coyoteTime = false;
	float coyoteTimer = 0f;
	public RaycastHit2D groundHit, angleHit, gpHit;
	public RaycastHit2D lastGroundHit;
	public float raycast_sep_sides = 8f;
	public float raycast_sep_bottom;

    public bool jumpCheck = false;
	public float floorAngle = 0f;
	float momentumY = 0f;
	public float ly_offset = 0f;
	public float ry_offset = 0f;
	float pFloorAngle;
	bool jumped = false;
	public float floorAngleFactor = 0.05f;
	public float groundYOffset;
	public float sideRayDistance = 4f;
	
	public void canJump(){

		groundHit = Physics2D.Raycast(this.transform.position + new Vector3(0f, raycast_sep_bottom, 0f), Vector2.down, physics.rayDistance, floorMask);
		angleHit = Physics2D.Raycast(this.transform.position + new Vector3(0f, raycast_sep_bottom, 0f), Vector2.down, Mathf.Infinity, floorMask);
		gpHit = Physics2D.Raycast(this.transform.position + new Vector3(0f, raycast_sep_bottom, 0f), Vector2.down, Mathf.Infinity, groundPoundingMask);
		
		rd_l = Physics2D.Raycast(this.transform.position + new Vector3(-raycast_sep_sides, raycast_sep_bottom+(pFloorAngle*-floorAngleFactor), 0f), Vector2.down, sideRayDistance, floorMask);
		rd_r = Physics2D.Raycast(this.transform.position + new Vector3(raycast_sep_sides,  raycast_sep_bottom+(pFloorAngle*floorAngleFactor), 0f), Vector2.down, sideRayDistance, floorMask);
		rd_m = groundHit.collider != null;
		
		if (gpHit.collider!=null) {

			floorY = (angleHit.collider.tag=="Slope") ? gpHit.point.y+16f : (Global.PlayerState.One.powerUp>0) ? (gpHit.collider.tag=="Destroyable" || gpHit.collider.tag=="Brick") ? gpHit.point.y-32f : gpHit.point.y : gpHit.point.y;
		}
		
		Grounded = (rd_l || rd_m || rd_r);

		if (rd_l || rd_m || rd_r) lastGroundHit=groundHit;
		if (angleHit.collider!=null) floorAngle = Vector2.SignedAngle(angleHit.normal, Vector2.up)*-1f;
		if (angleHit.collider!=null) pFloorAngle= Vector2.SignedAngle(angleHit.normal, Vector2.up)*-1f;
		
		if (Grounded)
		{
			
			initialWallJump = false;
			burstCount =0;
			wallJumpCancelIndex=0;
			lWallJumpSide=false;
			rWallJumpSide=false;
			wallJumpTimer=0f;
		
			didBurst=false;
			if (rb.velocity.y<0f && spinJumped) { spinJump = false;spinJumped=false; }
			if (rb.velocity.y < 0f && jumped) jumped = false;
			
			coyoteTime = false;
			jumpCheck = false;
			coyoteTimer = 0f;

			if (angleHit.collider != null) actualAngle = Mathf.LerpAngle(actualAngle, floorAngle, 15f * Time.deltaTime);
			if (rd_m || rd_l || rd_r)
			{
				if (!groundPounding && groundHit.collider!=null) this.transform.SetParent(groundHit.collider.transform);
				if (groundHit.collider==null) this.transform.SetParent(null);
				
			}
			else {
				this.transform.SetParent(null);
				floorAngle = 0f;
            }
		}
		else {

			if (momentumY != 0f) {
				rb.velocity = new Vector2(rb.velocity.x, momentumY);
				momentumY = 0f;
			}
			if (rb.velocity.y < 0f) startedJump = false;
			if (!jumpCheck)
			{
				coyoteTimer += Time.deltaTime;
				coyoteTime = coyoteTimer < physics.coyoteTime;
			}
			else 
			{
				coyoteTime = false;
				coyoteTimer = 0f;
            }

			this.transform.SetParent(null);
            offsetVX = 0f;
            floorAngle = 0f;

			if (!sliding)
			{
				actualAngle = Mathf.LerpAngle(actualAngle, 0f, 15f*Time.deltaTime);
			}
			else {
				actualAngle = Mathf.LerpAngle(actualAngle, floorAngle, 15f * Time.deltaTime);
			}
        }
	}
	public AudioClip SwimAC;
	bool canSwim = false;
	float swimTimer = 0f;

	public float sliding_speed = 15f;
	public void UpdateSliding() {

		if (!Grounded) { isOnSlidable = false; doGroundPound(false); return; }
		if (groundPounding) {isOnSlidable = false; return; }
		isOnSlidable = actualAngle < -1f || actualAngle > 1f;

		if (isOnSlidable)
		{
			if (GlobalInput.down && !sliding) {
				sliding = true;
			}
		}

		if (sliding) {
			if (!isOnSlidable) sliding = false;
			timerSkid += Time.deltaTime;
			if (!GlobalInput.down) {
				sliding = false;
			}
			if (curSpeed > 0f && actualAngle > 0f) curSpeed -= sliding_speed;
			if (curSpeed < 0f && actualAngle < 0f) curSpeed += sliding_speed;
			curSpeed += (float)((actualAngle > 0f) ? -sliding_speed: sliding_speed) * Time.deltaTime;
		}
	}
	public void UpdateSwim() {
		if (!S_Swim){
			BubbleFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			return;
		}
		BubbleFX.Play();
		if (canSwim) Swim(false);
		if (swimTimer>0f)swimTimer-=Time.deltaTime;
		canSwim = (swimTimer<=0f);
	}
	public ParticleSystem BubbleFX;
	public void Swim(bool androidFix) {
		if (!S_Swim)return;
		if (!androidFix) if (!GlobalInput.ap) return;
		
		rb.velocity=new Vector2(rb.velocity.x, physics.jumpForce);
		SoundManager.instance.Play(SwimAC, true);
		swimTimer = 0.5f;
	}
	// Llama a esta funcion para saltar.
	public void Jump() {
		if (this.physicsHalt) return;
		if (S_Swim) {Swim(false); return;}
		if (water && canSwim) Swim(false);
		if (!water && !Grounded && !coyoteTime) return;
		if (!water && jumped) return;
		//if (Application.platform == RuntimePlatform.Android && startedJump) return;
		rb.velocity=new Vector2(rb.velocity.x, physics.jumpForce+(Mathf.Abs(curSpeed)*physics.jumpMomentumFactor));
		SoundManager.instance.Play(1,JumpAC,1f,1f);
		spinJump=false;
		spinJumped=false;
		jumpCheck = true;
		jumped = true;
		startedJump = true;
	}
	bool spinJumped = false;
	public GameObject spinJumpParticle;
	public AudioClip spinJumpSFX;
	// Llama a esta funcion para G.
	public void SpinJump() {
		
		if (spinJumped || S_Duck) return;
		if (groundPounding) SoundManager.instance.Play(242, BurstAC, 1f, 1f);

		groundPoundAttack=false;
		Instantiate(spinJumpParticle, this.transform.position, Quaternion.identity);
		SoundManager.instance.Play(5925, spinJumpSFX, 1f, 1f);
		rb.velocity=new Vector2(rb.velocity.x, physics.spinJumpForce);
		spinJump=true;
		SCParticleOne.Play();
		SCParticleTwo.Play();
		spinJumped=true;
		animationHalt=false;
		physicsHalt=false;
		rb.isKinematic=false;
		groundPounding=false;
	}
	public void SpinJumpForce() {
		if (puHalt)return;

		groundPoundAttack=false;
		Instantiate(spinJumpParticle, this.transform.position, Quaternion.identity);
		SoundManager.instance.Play(5925, spinJumpSFX, 1f, 1f);
		rb.velocity=new Vector2(rb.velocity.x, physics.spinJumpForce);
		spinJump=true;
		SCParticleOne.Play();
		SCParticleTwo.Play();
		spinJumped=true;
		animationHalt=false;
		physicsHalt=false;
		rb.isKinematic=false;
		groundPounding=false;

	}
	public void GrabPowerUp(PowerUpData data) {
		StartCoroutine(PU_Goto(data));
	}
	public BoxCollider2D col;

	bool puHalt = false;
	public IEnumerator PU_Goto(PowerUpData data) {
		Global.PlayerState.One.powerUp = data.id;
        animationHalt = true;
		physicsHalt = true;
		puHalt=true;
        Global.Game.systemHalt = true;
        SoundManager.instance.Play(100, data.grabSound);
		yield return new WaitForSeconds(.01f);
		string gotostr=Global.PlayerState.One.powerUp.ToString()+"_Goto";
		
		PlayAnimForce(gotostr);
		this.flags = new List<string>();
		for (int i = 0; i < data.flags.Count; i++) {
			this.flags.Add(data.flags[i]);
		}
		ScoreController.instance.GetPoints(false, false, 1, this.transform.position);
		yield return new WaitForSeconds(1f);
        animationHalt = false;
        physicsHalt = false;
        Global.Game.systemHalt = false;
        puHalt=false;
    }
	public void PlayAnimForce(string anim) {
		mainAnim.Play(anim, 0, 0f);
	}
	//Funcion que devuelve string acomodado a al estilo del juego y el power up del jugador.
	public void PlayAnim(string anim) {
		if (animationHalt) return;
		if (mainAnim.IsInTransition(0)) return;

		if (S_Swim) {
			mainAnim.Play((canSwim) ? "SwimIdle" : "Swim", 0);
			return;
		}

		if (grab)
		{

			if (!mainAnim.GetCurrentAnimatorStateInfo(0).IsName(anim+ "_Grab")) mainAnim.Play(anim + "_Grab", 0);
		}
		else {

			if (!mainAnim.GetCurrentAnimatorStateInfo(0).IsName(anim)) mainAnim.Play(anim, 0);
        }
       
	}
	public float angleAnim1=45f;
	public float angleAnim2=90f;
	//Esta funcion anima al personaje.
	public void Animate() {
		if (animationHalt) {
			mainAnim.speed = 1f;
			return;
        }
        if(puHalt){return;}
		// Abs (Valor Absoluto), por si no lo conoces, lo unico que hace es hacer que un valor sea siempre positivo.
		// sacamos el valor absoluto de la velocidad actual del jugador
		float realSpeed = Mathf.Abs(curSpeed);
		//cambiamos la escala horizontal del jugador dependiendo de donde estas mirando.
        spriteT.transform.localScale = new Vector3((curSpeed > 0.1f) ? 1f : (curSpeed < -0.1f) ? -1f : spriteT.transform.localScale.x, spriteT.transform.localScale.y, spriteT.transform.localScale.z);

        //actualizamos mirrored para uso externo.
        mirrored = (curSpeed<-0.1f) ? true : (curSpeed>0.1f) ? false : mirrored;
		//Esta en el piso?
		if (Grounded) {

			// Si no se esta deslizando
			if (!sliding)
			{
				if (!skid)
				{
					// Esta moviendose?
					if (realSpeed < 1f)
					{
						//Animacion Idle 
						//string idleSTR = "Idle_" + (string)((pFloorAngle > angleAnim1 && pFloorAngle < angleAnim2) ? "1" : (pFloorAngle > angleAnim2) ? "2" : "0");
						PlayAnim(S_Duck ? "Crouch" : (string)((toss) ? "Toss" : "Idle"));
					}
					else if (realSpeed > 1)
					{
						//Animacion Walk
						PlayAnim(S_Duck ? "Crouch" : ((toss) ? "Toss" : (Global.PlayerState.One.speedCap ? "Walk_SC" : "Walk")));
					}
				}
				else
				{
					//Animacion Skid (Deslizar)
					PlayAnim("Skid");

				}
			}
			else {
				PlayAnim("GroundPound");
			}
			//cambiamos la velocidad de animacion acomodandose a la velocidad del jugador.
			if (!skid && !S_Swim) {
				mainAnim.speed = (float)((Mathf.Abs(curSpeed) > 0f)  ? (Mathf.Abs(curSpeed)*physics.animFactor) : 1f);
			}else{mainAnim.speed = 1f;}
		} else {
			if (!sliding)
			{
				if (grab)
				{
					if (skid)
					{
						PlayAnim("Skid");
					}
					else
					{
						PlayAnim((rb.velocity.y > 0f) ? "Jump" : "Fall");
					}
				}
				else
				{
					// deducimos que esta saltando.
					if (rb.velocity.y > 0f)
					{
						//Si la velocidad y va cuesta arriba, ponemos la animacion Jump
						PlayAnim(S_Duck ? "Crouch" : ((spinJump) ? "Spin" : (toss) ? "Toss_Jump" : (Global.PlayerState.One.speedCap ? "Jump_SC" : "Jump")));
					}
					else
					{
						// Si la velocidad y va cuesta abajo, ponemos la animacion Fall
						PlayAnim(S_Duck ? "Crouch" : ((spinJump) ? "Spin" : (toss) ? "Toss_Fall" : (Global.PlayerState.One.speedCap ? "Fall_SC" : "Fall")));
					}
				}
			}
			else {
				PlayAnim("GroundPound");
			}
			// cambiamos la velocidad de animacion
			mainAnim.speed = 1f;
		}
	}
	public void InputUpdate() {
		//checkeamos el input horizontal
		horInput = new Vector2((float)(GlobalInput.right ? 1f : 0f) - (float)(GlobalInput.left ? 1f : 0f),(float)((GlobalInput.up) ? 1f : 0f) - (float)((GlobalInput.down) ? 1f : 0f));
	}
	bool rayRight, rayLeft;
	bool water;
	public float hit_ra, hit_la;
	public void Move() {
		if (physicsHalt)
		{
			rb.velocity = new Vector2(0f, 0f);
	
			return;
		}

		rb.mass = (S_Swim) ? gravityWater : normalGravity;
		//actualizamos la velocidad de las fisicas.
		rb.velocity = new Vector2(curSpeed+offsetVX + (float)((Grounded) ? 0f : momentum), Mathf.Clamp(rb.velocity.y, physics.maxFallSpeed, physics.maxJumpSpeed));
		// actualizamos la velocidad del jugador
		float maxSpeed = (Global.PlayerState.One.speedCap) ? physics.maxSpeedSC : ((GlobalInput.b) ? physics.maxSpeedR : physics.maxSpeedW);
		// modificamos el offset horizontal de la camara dependiendo de la velocidad del jugador, para poder ver bien lo que viene despues (enemigos, caidas, etc).
		cam.modifiableOffset = curSpeed*offsetFactor;
		//Controlamos el skid (Sonido y FX)
		if (timerSkid>=0.1f && Grounded) {
			SoundManager.instance.Play(99,SkidAC,1f,1f);
			timerSkid=0f;
			Instantiate(FX, this.transform.position+ FXOffset, Quaternion.identity);
		}
		// Movimiento horizontal
		// Derecha
		if (!S_Duck) {
			RaycastHit2D hitr = Physics2D.Raycast(this.transform.position + new Vector3(0f,6f,0f), Vector2.right, 9f, floorMask);
			
			RaycastHit2D hitl = Physics2D.Raycast(this.transform.position + new Vector3(0f, 6f, 0f), Vector2.left, 9f, floorMask);
			hit_ra = Vector2.Angle(hitr.normal, Vector2.up)-90f;
			hit_la = Vector2.Angle(hitl.normal, Vector2.up)-90f;


			rayRight = hitr.collider!=null && hit_ra == 0f;
			rayLeft =  hitl.collider!=null && hit_la == 0f;


			if ((horInput.x > 0.1f && rayRight) || (horInput.x < -0.1f && rayLeft)) { if (!sliding) curSpeed = Mathf.Lerp(curSpeed, 0f, physics.dec * Time.deltaTime); }
				if (!sliding) {  
				if (horInput.x > 0.1f && !rayRight) {
					if (curSpeed<0f) {
						skid=true;
						timerSkid+=Time.deltaTime;
						curSpeed+=physics.skidSpeed*Time.deltaTime;
                   
					}
					else {
						skid=false;
					}
					if (curSpeed < maxSpeed) {
						curSpeed+=physics.acc*Time.deltaTime;
					}
					if (curSpeed > maxSpeed) {
						curSpeed=Mathf.Lerp(curSpeed,maxSpeed,5f*Time.deltaTime);
					}
				}else if (horInput.x < -0.1f && !rayLeft) {
					// Izquierda
					if (curSpeed>0f) {
						skid=true;
						timerSkid+=Time.deltaTime;
						curSpeed-=physics.skidSpeed*Time.deltaTime;
                   
					}
					else {
						skid=false;
					}
					if (curSpeed > -maxSpeed) {
						curSpeed-=physics.acc*Time.deltaTime;
					}
					if (curSpeed < -maxSpeed) {
						curSpeed=Mathf.Lerp(curSpeed,-maxSpeed,5f*Time.deltaTime);
					}
				}else {
					//Quieto
				
					if (!initialWallJump) curSpeed = Mathf.Lerp(curSpeed,0f,physics.dec*Time.deltaTime);
				
					skid=false;
				}
			}
		}
		
	}
	GameObject toss1, toss2;
	bool canToss = true;

	public void ShootAbility() {
		if (canToss)
		{
			if (toss1 == null)
			{
				toss1 = Instantiate(powerUpController.selfPickup.tossGB, this.transform.position, Quaternion.identity);
				toss1.GetComponent<Toss>().right = !this.mirrored;
				Toss();
			}
			else if (toss2 == null)
			{

				toss2 = Instantiate(powerUpController.selfPickup.tossGB, this.transform.position, Quaternion.identity);
				toss2.GetComponent<Toss>().right = !this.mirrored;
				Toss();
			}
		}
	}
	public void UpdatePowerUps() {
		if (flags.Contains("BULLET")&& powerUpController.selfPickup.tossGB==null) {
			if (GlobalInput.bp && canToss && !toss) {Toss();}
		}
		if (flags.Contains("BULLET") && !toss && powerUpController.selfPickup.tossGB!=null) {
			if (GlobalInput.bp) {
				ShootAbility();
			}
		}
	}
	public float wallJumpTime = 0.25f;
	float wallJumpTimer = 0f;
	public AudioClip wallJumpAC;
	public float wallJumpNormalSpeed = 200f;
	bool rWallJumpSide, lWallJumpSide;
	bool initialWallJump = false;
	public void WallJumpUpdate(bool androidFix) {
		if (!androidFix) if (horInput.x==0f)return;
		if (groundPounding)return;

		RaycastHit2D right = Physics2D.BoxCast(this.transform.position+new Vector3(0f,8f,0f), new Vector2(4f, 16f), 0f, Vector2.right, 9f, wallMask);
		RaycastHit2D left = Physics2D.BoxCast(this.transform.position+new Vector3(0f,8f,0f), new Vector2(4f, 16f), 0f, Vector2.left, 9f, wallMask);
		if (right.collider==null && left.collider==null) return;
		if (Grounded) return;
		wallJumpTimer += Time.deltaTime*2f;
		if (!androidFix) if (!GlobalInput.ap) return;
		if (wallJumpTimer > wallJumpTime) return;
		
		int internalWallJumpSide = (right.collider!=null && horInput.x>0f) ? 1 : (left.collider!=null && horInput.x<0f) ? -1 : 1;

		if (!initialWallJump) { 
		
			if (lWallJumpSide && internalWallJumpSide==-1) return;
			if (rWallJumpSide && internalWallJumpSide==1) return;

			if (internalWallJumpSide==-1 && horInput.x>0f) return;
			if (internalWallJumpSide==1 && horInput.x<0f) return;
		}

		initialWallJump = true;
		curSpeed = (curSpeed > 0f) ? -(wallJumpNormalSpeed) : wallJumpNormalSpeed;
		Instantiate(KickFX, this.transform.position, Quaternion.identity);
		SoundManager.instance.Play(99595, wallJumpAC, 1f, 1f);
		rb.velocity = new Vector2(rb.velocity.x, 200f);
		wallJumpTimer=0f;
		lWallJumpSide = (internalWallJumpSide == -1) ? true : false;
		rWallJumpSide = (internalWallJumpSide == 1) ? true : false;
	}

	bool localSC = false;
	float SC_timer = 0f;
	public AudioSource SC_AS;
	public List<Image> hud_sc;
	public Sprite hud_sc_on, hud_sc_off;
	public Animator hud_sc_capter;
	

	public GameObject SCParticleOnce;
	public ParticleSystem SCParticleOne, SCParticleTwo;

	public bool scOnce = false;

	public AudioClip scOnceAC;
	bool scOnce2 = false;
	public void SC() {
		if (skid && SC_timer>1f)  SC_timer=1f;
		localSC=(Mathf.Abs(curSpeed) >= physics.maxSpeedW && horInput.x!=0f && GlobalInput.b);
		Global.PlayerState.One.speedCap=(SC_timer>=3f);
		if (!Global.PlayerState.One.speedCap && !localSC) scOnce = false;

		if (Global.PlayerState.One.speedCap && Grounded){
			if (S_Duck) return;

			if (!scOnce) {
				
				Instantiate(SCParticleOnce, this.transform.position, Quaternion.identity);
				SoundManager.instance.Play(58, scOnceAC, 1f, 1f);
				SCParticleOne.Play();
				SCParticleTwo.Play();
				scOnce=true;
			}

			if (!scOnce2){
				
				SCParticleOne.Play();
				SCParticleTwo.Play();
				scOnce2=true;
			}
		}else {
			if (!spinJump && !groundPounding) {
				SCParticleOne.Stop(true, ParticleSystemStopBehavior.StopEmitting);
				SCParticleTwo.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
			scOnce2=false;
		}
		if (localSC) {
			if (SC_timer>=3f) {
				SC_timer=3f;
			}
			SC_timer+=Time.deltaTime;

			
		}else {
			
			if (SC_timer > 0f) {SC_timer-=Time.deltaTime*2f;}else {SC_timer=0f;}
		}
	}
	public void UpdateMomentum() {
		if (Grounded) {if (Mathf.Abs(curSpeed) > 1f) {momentum=Mathf.Lerp(momentum,(mirrored) ? physics.momentumFactor*-1f : physics.momentumFactor,25f*Time.deltaTime);}else {momentum=Mathf.Lerp(momentum,0f,1f*Time.deltaTime);} }
	}
	
	public void GrabUpdate() {
		if (!grab) return;

		grabSpriteRenderer.sprite = writeGrabSprite;

		if (grabObject==null) {
			grab=false;
		}

		if (!GlobalInput.b) { StartCoroutine(Kick()); }
	}
	public bool getDirection() {
		
		if (horInput.x==0f) {
			return !mirrored;
		}else {
			return (horInput.x>0f) ? true : false;
		}
		if(skid && grab) {
			return mirrored;
		}

		return !mirrored;
	}
	public AudioClip kickAC;
	public IEnumerator Kick() {
        grab = false;
        this.animationHalt = true;
		PlayAnimForce("Kick");
		SoundManager.instance.Play(4259, kickAC, 1f, 1f);
		this.grabObject.SetActive(true);
		bool sider =  getDirection();
        this.grabObject.transform.position = this.transform.position+ new Vector3( (float)((sider) ? 10f : -10f),0f,0f);
        ShellIntersectType myType = ShellIntersectType.Kick;
        if (GlobalInput.down && Grounded) myType = ShellIntersectType.Drop;
        if (GlobalInput.up && !GlobalInput.down) myType = ShellIntersectType.UpThrow; 

        this.grabObject.GetComponent<ShellScript>().Interact(myType, this.rb, (Vector2)this.transform.position,  sider);

        yield return new WaitForSeconds(0.25f);
        this.animationHalt = false;
    }
	bool hInputMirrored = true;
	public bool musicFix = false;
    public void Update() {

    	if (horInput.x != 0f) hInputMirrored = (horInput.x > 0f) ? true : false;
		shellRend.color = grab ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0f);
		if (!musicFix) MusicController.instance.underWater = water;
		physics = S_Swim ? physicsWater : physicsNormal;
        
        InputUpdate();
		Animate();
		UpdateSwim();
		doGroundPound(false);
		GrabUpdate();
		if (Application.platform != RuntimePlatform.Android) WallJumpUpdate(false);
		UpdateSliding();

		this.col.size = (Global.PlayerState.One.powerUp==0) ? SmallColSize : (S_Duck) ? BigColSizeCrouch : BigColSizeNoCrouch;
		this.col.offset = (Global.PlayerState.One.powerUp==0) ? SmallColOffset : (S_Duck) ? BigColOffsetCrouch : BigColOffsetNoCrouch;

		if (burstHalt) rb.velocity = new Vector2(rb.velocity.x, 0f);

		if (puHalt) {
			animationHalt = true;
			physicsHalt = true;
		}
	}
	public AudioClip hitSound, dieSound;
	public void Hit(float sourceX) {
		if(Global.PlayerState.One.star) return;
		if (burstHalt) return; 
		if (!Global.PlayerState.One.vulnerable) return;
		
		Global.PlayerState.One.vulnerable = false;
		
		if (Global.PlayerState.One.powerUp > 0)
		{
            Global.Game.systemHalt = true;
            StartCoroutine(HitAnim(sourceX));
		}
		else {
            Global.Game.systemHalt = true;
            Die();
        }
    }

	public IEnumerator HitAnim(float sourceX) { 
		this.puHalt=true;
		this.physicsHalt  = true;
		this.animationHalt = true;
		PlayAnimForce(Global.PlayerState.One.powerUp.ToString()+"_Loss");
		SoundManager.instance.Play(12256, hitSound,1f,1f);
		this.mainAnim.speed = 1.5f;
		yield return new WaitForSeconds(1.5f);
		this.flags.Clear();
		Global.PlayerState.One.powerUp = (Global.PlayerState.One.powerUp > 1) ? 1 : 0;
        this.physicsHalt = false;
		this.animationHalt = false;
        Global.Game.systemHalt = false;
        if (Global.PlayerState.One.powerUp==0)cWarpSprite = defaultcWarpSprite;
        this.puHalt=false;
    }

	[Header("DiePrefab")] public GameObject diePrefab;
	public void Die() {
		if(Global.PlayerState.One.star) return;
		Instantiate(diePrefab, this.transform.position-new Vector3(0f,8f,0f), Quaternion.identity);
		Destroy(this.gameObject);	
	}
	[Header("DiePrefabLava")] public GameObject diePrefabLava;
	public void DieLava() {
		Instantiate(diePrefabLava, this.transform.position-new Vector3(0f,8f,0f), Quaternion.identity);
		Destroy(this.gameObject);	
	}

  
	public GameObject waterSplash;

	public GameObject DestroyableFX;
	public AudioClip DestroyableSFX;	
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Death") {
			Die();
		} else if (other.tag == "Lava") {
			DieLava();
		}
		else if (other.tag == "Hurt") {
			Hit(other.gameObject.transform.position.x);
		}
		else if (other.tag == "AvoidableHurt") {
			if (!spinJump || rb.velocity.y > 0f) {Hit(other.gameObject.transform.position.x);} else{
				rb.velocity = new Vector2(0f,(GlobalInput.a) ? 200f : 150f);
				Instantiate(KickFX, this.transform.position, Quaternion.identity);
			}
		}else if (other.tag == "Bullet") {
			if(Global.PlayerState.One.star){
				rb.velocity = new Vector2(0f,(GlobalInput.a) ? 200f : 150f);
				Instantiate(KickFX, this.transform.position, Quaternion.identity);
				other.gameObject.GetComponent<BulletScript>().Die();
				SoundManager.instance.Play(04259, kickAC, 1f, 1f);
				return;
			}
			if (rb.velocity.y > 0f || this.transform.position.y < other.gameObject.transform.position.y) {
				Hit(other.gameObject.transform.position.x);	
				return;
			}else {
				rb.velocity = new Vector2(0f,(GlobalInput.a) ? 200f : 150f);
				Instantiate(KickFX, this.transform.position, Quaternion.identity);
				other.gameObject.GetComponent<BulletScript>().Die();
				SoundManager.instance.Play(04259, kickAC, 1f, 1f);
			}
			
			
		} else if (other.tag == "Enemy") 
    	{
    		EnemyScript get = other.GetComponent<EnemyScript>();
    		if (other.gameObject.GetComponent<EnemyScript>().died) return;
    		if (Global.PlayerState.One.star) {get.Die(1);return;}
			if (sliding) { get.Die(1); return; }
			if (groundPoundAttack) {get.Die(3);return;}
			if (!Grounded) return;
			
			if(!groundPoundAttack && !groundPounding)Hit(other.transform.position.x);
        }
		
		else if (other.tag == "Coin")
		{
			other.GetComponent<CoinScript>().Grab();
		}else if (other.tag == "Water")
        {
			Transform transform = Instantiate(waterSplash, this.transform.position, Quaternion.identity).transform;

			transform.position = new Vector3(transform.position.x, other.transform.position.y + (other.transform.localScale.y / 2f), 0f);
			water = true;
			S_Swim=true;
        }else if (other.tag == "Shell")
        {
            ShellScript otherSCR = other.gameObject.GetComponent<ShellScript>();

            if (otherSCR==null) return;
            if (GlobalInput.b) return;

            ShellIntersectType myType = ShellIntersectType.Top;

            if (this.transform.position.y < other.gameObject.transform.position.y && Grounded) myType = ShellIntersectType.Kick;

            otherSCR.Interact(myType, this.rb, (Vector2)this.transform.position,  hInputMirrored);
        }else if (other.tag == "Boss")
        {
			if (rb.velocity.y<0f) {rb.velocity = new Vector2(rb.velocity.x, 200f); return;}
			
			if (burstHalt) return;
			Hit(other.gameObject.transform.position.x);
		}
		
	}

    public void OnTriggerExit2D(Collider2D other)
    {

		 if (other.tag == "Water")
		{
			Transform transform = Instantiate(waterSplash, this.transform.position, Quaternion.identity).transform;

			transform.position = new Vector3(transform.position.x, other.transform.position.y+(other.transform.localScale.y/2f), 0f);
			water = false;
			S_Swim=false;
		}
	}
    public GameObject KickFX;

    public void OnTriggerStay2D(Collider2D other)
    {
   
    	if (other.tag == "Enemy") 
    	{
    		EnemyScript get = other.GetComponent<EnemyScript>();
    		if (get.died) return;
			if (Global.PlayerState.One.star) { get.Die(1); return; }
			if (sliding) {get.Die(1);return;}
			if (groundPoundAttack) {get.Die(3);return;}
			
			if(!Grounded && rb.velocity.y < 0f) {
				get.Die(spinJump ? 3 : 0);
				rb.velocity = new Vector2(rb.velocity.x, GlobalInput.a ? this.physics.jumpForceStumpP : this.physics.jumpForceStomp);
				return;
			}
        }
        else if (other.tag == "Shell")
        {
            ShellScript otherSCR = other.gameObject.GetComponent<ShellScript>();

            if (otherSCR==null) return;
            if (otherSCR.interactHalt) return;

            if (GlobalInput.b ) {
            	grab = true;
            	writeGrabSprite = otherSCR.selfPreview.sprite;
            	grabObject = other.gameObject;
            	grabObject.SetActive(false);
            	return;
            }
        }else if (other.tag == "BossHit")
        {
			if (rb.velocity.y>0f) {rb.velocity = new Vector2(rb.velocity.x, 200f); return;}
			BossHitScript scrB = other.gameObject.GetComponent<BossHitScript>();
			if (!scrB.canHit)return;

			if (scrB.groundPoundMust && groundPounding) {rb.velocity = new Vector2(rb.velocity.x, 200f); physicsHalt=false; groundPounding=false; SpinJumpForce(); scrB.TryHit();} 
			else if (!scrB.groundPoundMust && rb.velocity.y<0f)  {rb.velocity = new Vector2(rb.velocity.x, 200f);  physicsHalt=false; groundPounding=false; scrB.TryHit();} 
			else {groundPounding=false; return;}
		}else if (other.tag == "Destroyable" || other.tag == "Brick"){
			if (groundPounding && Global.PlayerState.One.powerUp>0) 
			{
				Instantiate(DestroyableFX, other.transform.position, Quaternion.identity);
				Destroy(other.gameObject);
				SoundManager.instance.Play(295299, DestroyableSFX, 1f, 1f);
			}
		}
    }

    public void FixedUpdate(){
    	
		angleT.transform.eulerAngles = new Vector3(0f,0f,actualAngle);
		if (Application.platform != RuntimePlatform.Android) { 
			if (GlobalInput.ap) Jump();
			if (GlobalInput.cp) SpinJump();
			if (GlobalInput.dp) Burst();
		}
		canJump();
		SC();
		UpdateToss();
		UpdateMomentum();
		Move();
		crouchUpdate();

		

		if (!animationHalt) UpdatePowerUps();
	}
}
