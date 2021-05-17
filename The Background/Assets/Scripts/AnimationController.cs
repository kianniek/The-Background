using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;
using UnityEngine.Animations.Rigging;

public class AnimationController : MonoBehaviour
{
	Controller controller;
	MovementController Mcontroller;
	Animator animator;
	Transform animatorTransform;
	Transform tr;

	//Whether the character is using the strafing blend tree;
	public bool useStrafeAnimations = false;

	//Velocity threshold for landing animation;
	//Animation will only be triggered if downward velocity exceeds this threshold;
	public float landVelocityThreshold = 5f;

	private float smoothingFactor = 40f;
	Vector3 oldMovementVelocity = Vector3.zero;

	public LayerMask WaterLayer;

	//Setup;
	void Awake()
	{
		controller = GetComponent<Controller>();
		Mcontroller = GetComponent<MovementController>();
		animator = GetComponentInChildren<Animator>();
		animatorTransform = animator.transform;

		tr = transform;
	}

	//OnEnable;
	void OnEnable()
	{
		//Connect events to controller events;
		controller.OnLand += OnLand;
		controller.OnJump += OnJump;
	}

	//OnDisable;
	void OnDisable()
	{
		//Disconnect events to prevent calls to disabled gameobjects;
		controller.OnLand -= OnLand;
		controller.OnJump -= OnJump;
	}

	//Update;
	void Update()
	{
		if (Mcontroller.climbing)
		{
			animator.speed = 1;
			animator.SetBool("IsClimbing", true);
			if (Mcontroller.climbingUp)
			{
				animator.SetBool("ClimbingDown", false);
				animator.SetBool("ClimbingRight", false);
				animator.SetBool("ClimbingLeft", false);

				animator.SetBool("ClimbingUp", true);
				animator.speed = 1;
			}
			else if (Mcontroller.climbingDown)
			{
				animator.SetBool("ClimbingUp", false);
				animator.SetBool("ClimbingRight", false);
				animator.SetBool("ClimbingLeft", false);

				animator.SetBool("ClimbingDown", true);
				animator.speed = 1;
			}
			else if (Mcontroller.climbingRight)
			{
				animator.SetBool("ClimbingDown", false);
				animator.SetBool("ClimbingUp", false);
				animator.SetBool("ClimbingLeft", false);

				animator.SetBool("ClimbingRight", true);
			}
			else if (Mcontroller.climbingLeft)
			{
				animator.SetBool("ClimbingDown", false);
				animator.SetBool("ClimbingUp", false);
				animator.SetBool("ClimbingRight", false);

				animator.SetBool("ClimbingLeft", true);
			}
			else if(Mcontroller.climbingStatic)
			{
				animator.speed = 0;
				animator.SetBool("ClimbingDown", false);
				animator.SetBool("ClimbingUp", false);
				animator.SetBool("ClimbingRight", false);
				animator.SetBool("ClimbingLeft", false);
			}
			else
			{
				animator.speed = 1;
				animator.SetBool("ClimbingDown", false);
				animator.SetBool("ClimbingUp", false);
				animator.SetBool("ClimbingRight", false);
				animator.SetBool("ClimbingLeft", false);
			}

			if (Mcontroller.FoundLedge)
			{
				animator.SetBool("EdgeDetected", true);
				if (Mcontroller.LeftOnEdge)
				{
					animator.SetBool("LeftOnEdge", true);
					animator.SetBool("RightOnEdge", false);
				}
				else if (Mcontroller.RightOnEdge)
				{
					animator.SetBool("RightOnEdge", true);
					animator.SetBool("LeftOnEdge", false);
				}
				else
				{
					animator.SetBool("RightOnEdge", false);
					animator.SetBool("LeftOnEdge", false);
				}
			}
			else
			{
				animator.SetBool("EdgeDetected", false);
			}
		}
		else
		{
			animator.SetBool("IsClimbing", false);
			animator.speed = 1;
		}

		//Get controller velocity;
		Vector3 _velocity = controller.GetVelocity();

		//Split up velocity;
		Vector3 _horizontalVelocity = VectorMath.RemoveDotVector(_velocity, tr.up);
		Vector3 _verticalVelocity = _velocity - _horizontalVelocity;

		//Smooth horizontal velocity for fluid animation;
		_horizontalVelocity = Vector3.Lerp(oldMovementVelocity, _horizontalVelocity, smoothingFactor * Time.deltaTime);
		oldMovementVelocity = _horizontalVelocity;

		animator.SetFloat("VerticalSpeed", _verticalVelocity.magnitude * VectorMath.GetDotProduct(_verticalVelocity.normalized, tr.up));
		animator.SetFloat("HorizontalSpeed", _horizontalVelocity.magnitude);

		//If animator is strafing, split up horizontal velocity;
		if (useStrafeAnimations)
		{
			Vector3 _localVelocity = animatorTransform.InverseTransformVector(_horizontalVelocity);
			animator.SetFloat("ForwardSpeed", _localVelocity.z);
			animator.SetFloat("StrafeSpeed", _localVelocity.x);
		}

		//Pass values to animator;
		animator.SetBool("IsGrounded", controller.IsGrounded());
		animator.SetBool("IsStrafing", useStrafeAnimations);

		if (Mcontroller.isSwimming)
		{
			animator.SetBool("Swimming", true);
			if (Mcontroller.StationaryInWater)
			{
				animator.SetBool("StationarySwimming", true);
			}
			else
			{
				animator.SetBool("StationarySwimming", false);
			}
		}
		if (!Mcontroller.isSwimming)
		{
			animator.SetBool("Swimming", false);
		}
	}

	void OnLand(Vector3 _v)
	{
		//Only trigger animation if downward velocity exceeds threshold;
		if (VectorMath.GetDotProduct(_v, tr.up) > -landVelocityThreshold)
			return;

		animator.SetTrigger("OnLand");
	}

	void OnJump(Vector3 _v)
	{

	}
}
