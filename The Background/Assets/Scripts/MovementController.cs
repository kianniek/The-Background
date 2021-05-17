using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using System.Threading.Tasks;

public class MovementController : MonoBehaviour
{
	protected Transform tr;
	protected Mover mover;
	protected CharacterInput characterInput;
	protected CeilingDetector ceilingDetector;
	protected Controller contoller;
	protected Rigidbody rb;


	private float StoredMovementSpeed;
	private float StoredRunningSpeed;

	[Header("Climbing")]
	public float ClimbSpeed = 3f;
	public float WallDetectionRadius = 1f;
	public LayerMask wallMask;
	public bool climbing;
	RaycastHit WallHit;
	RaycastHit LedgeHit;

	//For AnimationContoller

	[HideInInspector] public bool climbingUp;
	[HideInInspector] public bool climbingDown;
	[HideInInspector] public bool climbingRight;
	[HideInInspector] public bool climbingLeft;
	[HideInInspector] public bool climbingStatic;
	[HideInInspector] public bool LeftOnEdge;
	[HideInInspector] public bool RightOnEdge;

	bool isInitialized;
	public Vector3 LedgeDetectOffset;
	public bool FoundLedge;
	Vector3 LedgeWorldCord;
	[HideInInspector] public Vector3 WallFromTopDetectOffset;
	[SerializeField] bool FoundWallFromTop;
	Vector3 WallFromTopCord;
	Vector3 wallPoint;
	[HideInInspector] 
	public Vector3 wallNormal;
	Rigidbody body;
	CapsuleCollider coll;

	[Header("Crouching")]
	//Crouch speed;
	public float crouchSpeed = 3f;
	public GameObject ScaleFromBottom;

	//Crouch Height (the collider height of player)
	public float NormalColliderHeight = 2f;
	public float crouchColliderHeight = 1f;
	private bool IsCrouching = false;


	[Header("Swimming")]
	public bool CanSwim;
	public LayerMask WaterLayer;
	public bool isSwimming;
	public ulong underwaterSpeedForward = 10; 
	public float underwaterSpeedBack = 7f;
	public float underwaterSpeedStrafe = 7f;
	public float upSpeed = 7f;
	private float waterTop;
	[HideInInspector] public bool StationaryInWater;

	public Transform cameraTransform;
	public Transform ModelTransform;
	public Transform LedgeDetector;
	public Transform WallDetector;
	public Transform WaterDetector;
	public TurnTowardTransformDirection turnTCD;
	public AdvancedWalkerController AWC;

	void Awake()
    {
		tr = transform;
		mover = GetComponent<Mover>();
		contoller = GetComponent<AdvancedWalkerController>();
		characterInput = GetComponent<CharacterInput>();
		ceilingDetector = GetComponent<CeilingDetector>();
		body = GetComponent<Rigidbody>();
		coll = GetComponent<CapsuleCollider>();
		turnTCD = GetComponentInChildren<TurnTowardTransformDirection>();
		AWC = gameObject.GetComponent<AdvancedWalkerController>();
		StoredMovementSpeed = AWC.movementSpeed;
		StoredRunningSpeed = AWC.runningSpeed;

	}

    void Update()
    {
		if (NearWall())
		{
			if (FacingWall())
			{
				if (Input.GetKeyDown(KeyCode.C) && !isSwimming) 
				{ 
					// if player presses the climb button
					if (FoundLedge)
					{
						tr.position = LedgeWorldCord;
					}
					FoundLedge = false;
					climbing = !climbing;
					isInitialized = !isInitialized;
					AWC.SetMomentum(Vector3.zero);
				}
			}
		}
		else
		{
			climbing = false;
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (FoundWallFromTop)
			{
				climbing = !climbing;
				isInitialized = !isInitialized;
				AWC.SetMomentum(Vector3.zero);
			}
			if (isSwimming)
			{
				if (FoundLedge)
				{
					tr.position = LedgeWorldCord;
				}
				isSwimming = false;
				FoundLedge = false;
				isInitialized = false;
				AWC.SetMomentum(Vector3.zero);
			}
		}
		if (!climbing && !isSwimming)
		{
			//Check if the player has initiaded to crouch;
			HandleCrouching();
			
		}
		if (climbing)
		{
			ClimbWall();
		}
		if (CanSwim)
		{
			HandleSwimming();
		}
		FindWallFromTop();
	}

	void HandleCrouching()
	{

		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			IsCrouching = true;
			mover.SetColliderHeight(crouchColliderHeight);
			AWC.movementSpeed = crouchSpeed;
			AWC.runningSpeed = crouchSpeed;
			ScaleFromBottom.transform.localScale = new Vector3(1, ScaleFromBottom.transform.localScale.y / 2, 1);
		}
		if (Input.GetKeyUp(KeyCode.LeftControl) || isSwimming || climbing)
		{
			IsCrouching = false;
		}
		if (!ceilingDetector.HitCeiling() && !IsCrouching)
		{
			IsCrouching = false;
			mover.SetColliderHeight(NormalColliderHeight);
			ScaleFromBottom.transform.localScale = new Vector3(1, 1, 1);
			AWC.movementSpeed = StoredMovementSpeed;
			AWC.runningSpeed = StoredRunningSpeed;
		}
	}
	bool NearWall()
	{
		return Physics.CheckSphere(transform.position, WallDetectionRadius, wallMask);
	}
	bool FacingWall()
	{
		RaycastHit hit;
		var facingWall = Physics.Raycast(ModelTransform.position, ModelTransform.forward, out hit, coll.radius + 1f, wallMask);
		Debug.DrawRay(ModelTransform.position, ModelTransform.forward, Color.yellow);
		wallPoint = hit.point;
		wallNormal = hit.normal;
		return facingWall;
	}
	void ClimbWall()
	{
		AWC.SetMomentum(Vector3.zero);
		body.velocity = Vector3.zero;
		FindLedge();
		
		if (!isInitialized)
		{
			GrabWall();
			isInitialized = true;
		}
		body.useGravity = false;

		ModelTransform.rotation = Quaternion.Slerp(ModelTransform.rotation , Quaternion.LookRotation(-wallNormal), 1);
		if (wallNormal == Vector3.zero) {return;}
		Debug.Log(wallNormal);

		var v = Vector3.ProjectOnPlane(ModelTransform.right, wallNormal).normalized * characterInput.GetHorizontalMovementInput();
		var h = Vector3.ProjectOnPlane(ModelTransform.up, wallNormal).normalized * characterInput.GetVerticalMovementInput();
		
		if (FoundLedge && Input.GetKey(KeyCode.W))
		{
			h = Vector3.zero;
		}
		var move = v + h;

        #region animation variables
        if (Input.GetKey(KeyCode.W))
		{
			climbingUp = true;

			climbingStatic = false;
			climbingDown = false;
			climbingRight = false;
			climbingLeft = false;
			if (FoundLedge)
			{
				climbingDown = false;
				climbingUp = false;
				climbingStatic = false;
				climbingRight = false;
				climbingLeft = false;
			}
		}
		else if(Input.GetKey(KeyCode.S))
		{
			climbingUp = false;
			climbingStatic = false;
			climbingRight = false;
			climbingLeft = false;

			climbingDown = true;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			climbingUp = false;
			climbingStatic = false;
			climbingDown = false;
			climbingRight = false;

			climbingLeft = true;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			climbingUp = false;
			climbingStatic = false;
			climbingDown = false;
			climbingLeft = false;

			climbingRight = true;

		}
		else
		{
			if (FoundLedge)
			{
				climbingDown = false;
				climbingUp = false;
				climbingStatic = false;
				climbingRight = false;
				climbingLeft = false;
			}
			else
			{
				climbingDown = false;
				climbingUp = false;
				climbingRight = false;
				climbingLeft = false;

				climbingStatic = true;
			}
		}

        #endregion

        if (FoundLedge)
		{
			if (Input.GetKey(KeyCode.A))
			{
				LeftOnEdge = true;
				RightOnEdge = false;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				RightOnEdge = true;
				LeftOnEdge = false;
			}
			else
			{
				RightOnEdge = false;
				LeftOnEdge = false;
			}
		}
		else
		{
			RightOnEdge = false;
			LeftOnEdge = false;
		}
		ApplyMove(move, ClimbSpeed);
	}
	void GrabWall()
	{
		var newPosition = wallPoint + wallNormal * (coll.radius - 0.1f);
		transform.position = Vector3.Lerp(transform.position, newPosition, 1);

		if (wallNormal == Vector3.zero)
			return;

		ModelTransform.rotation = Quaternion.LookRotation(-wallNormal);
	}
	void ApplyMove(Vector3 move, float speed)
	{
		body.MovePosition(transform.position + move * speed * Time.deltaTime);
	}
	void FindLedge()
	{
		Physics.Raycast(LedgeDetector.position + LedgeDetectOffset, -ModelTransform.up, out LedgeHit, 3f, wallMask);
		Debug.DrawRay(LedgeDetector.position + LedgeDetectOffset, -ModelTransform.up, Color.blue);
		LedgeWorldCord = LedgeHit.point;
		if (LedgeHit.normal.y >= 0.7 && Vector3.Distance(tr.position, LedgeWorldCord) < 5)
		{
			FoundLedge = true;
		}
		else
		{
			FoundLedge = false;
		}
	}
	void FindWallFromTop()
	{
		Physics.Raycast(WallDetector.position + WallFromTopDetectOffset, -ModelTransform.forward, out WallHit, 5f, wallMask);
		Debug.DrawRay(WallDetector.position + WallFromTopDetectOffset, -ModelTransform.forward, Color.red);
		WallFromTopCord = WallHit.point;
		if (Vector3.Dot(WallHit.normal, ((WallDetector.position + WallFromTopDetectOffset) - WallHit.point).normalized) > 0)
		{
			FoundWallFromTop = true;
			if (Input.GetKeyDown(KeyCode.C)){
				var newPosition = WallFromTopCord + wallNormal * (coll.radius - 0.1f);
				transform.position = Vector3.Lerp(transform.position, newPosition, 1);

				if (wallNormal == Vector3.zero)
					return;

				ModelTransform.rotation = Quaternion.LookRotation(-wallNormal);
			}
		}
		else
		{
			FoundWallFromTop = false;
		}
	}

	void HandleSwimming()
	{
		if (isSwimming)
		{
			
			contoller.enabled = false;
			DoSwimming();
			turnTCD.enabled = false;
			FindLedge();
		}
		else
		{
			contoller.enabled = true;
			turnTCD.enabled = true;
		}
	}
	void DoSwimming()
	{
		
		
		Vector3 movement = cameraTransform.forward;
		ModelTransform.rotation = Quaternion.LookRotation(movement);

		AWC.SetMomentum(Vector3.zero);
		body.velocity = Vector3.zero;
		if (WaterDetector.transform.position.y >= waterTop)
		{
			return;
		}
		else
		{
			if (Input.GetKey(KeyCode.W))
			{
				ModelTransform.rotation = Quaternion.Lerp(ModelTransform.rotation, Quaternion.LookRotation(cameraTransform.forward), 1f);
				transform.Translate(Vector3.forward * Time.deltaTime * underwaterSpeedForward, Camera.main.transform);
				StationaryInWater = false;
			}
			if (Input.GetKey(KeyCode.S))
			{
				ModelTransform.rotation = Quaternion.Lerp(ModelTransform.rotation, Quaternion.LookRotation(-cameraTransform.forward), 1f);
				transform.Translate(-Vector3.forward * Time.deltaTime * underwaterSpeedBack, Camera.main.transform);
				StationaryInWater = false;
			}
			if (Input.GetKey(KeyCode.D))
			{
				ModelTransform.rotation = Quaternion.Lerp(ModelTransform.rotation, Quaternion.LookRotation(cameraTransform.right), 1f);
				transform.Translate(Vector3.right * Time.deltaTime * underwaterSpeedStrafe, Camera.main.transform);
				StationaryInWater = false;
			}
			if (Input.GetKey(KeyCode.A))
			{
				ModelTransform.rotation = Quaternion.Lerp(ModelTransform.rotation, Quaternion.LookRotation(-cameraTransform.right), 1f); 
				transform.Translate(-Vector3.right * Time.deltaTime * underwaterSpeedStrafe, Camera.main.transform);
				StationaryInWater = false;
			}
			else if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
			{
				StationaryInWater = true;
				ModelTransform.rotation = Quaternion.Lerp(ModelTransform.rotation, Quaternion.LookRotation(new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z)),1f);
			}
			if (Input.GetKey(KeyCode.Space))
			{

				if (WaterDetector.transform.position.y >= waterTop)
				{
					StationaryInWater = true;
					return;
				}
				else
				{
					body.AddForce(Vector3.up * Time.deltaTime * upSpeed, ForceMode.VelocityChange);
					StationaryInWater = false;
				}
			}
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.layer == WaterLayer || other.gameObject.CompareTag("Water"))
		{

			if(Physics.Raycast(tr.position, -tr.up, 5f, ~WaterLayer))
			{
				return;
			}
			isSwimming = true;
			waterTop = other.gameObject.transform.position.y + other.gameObject.transform.localScale.y / 2;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == WaterLayer || other.gameObject.CompareTag("Water"))
		{
			isSwimming = false;
		}
	}
}
