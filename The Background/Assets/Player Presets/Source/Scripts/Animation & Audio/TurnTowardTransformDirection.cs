using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script rotates an object toward the 'forward' direction of another target transform;
	public class TurnTowardTransformDirection : MonoBehaviour {

		public Transform targetTransform;
		Transform tr;
		Transform parentTransform;

		//Setup;
		void Start () {
			tr = transform;
			parentTransform = transform.parent;

			if(targetTransform == null)
				Debug.LogWarning("No target transform has been assigned to this script.", this);
		}
		
		//Update;
		void LateUpdate () {

			if(!targetTransform)
				return;

			//Calculate up and forward direction;
			Vector3 _forwardDirection = Vector3.ProjectOnPlane(targetTransform.forward, parentTransform.up).normalized;
			Vector3 _forwardDirectionClimbing = Vector3.ProjectOnPlane(tr.up, -parentTransform.GetComponent<MovementController>().wallNormal).normalized;
			Vector3 _upDirection = parentTransform.up;

			//Set rotation;
			if (parentTransform.GetComponent<MovementController>().climbing)
			{
				tr.rotation = Quaternion.LookRotation(-parentTransform.GetComponent<MovementController>().wallNormal, _upDirection);
				if (-parentTransform.GetComponent<MovementController>().wallNormal == Vector3.zero)
				{
					tr.rotation = Quaternion.LookRotation(-transform.forward, _upDirection);
				}
			}
			else
			{
				tr.rotation = Quaternion.LookRotation(_forwardDirection, _upDirection);
			}
			
		}
	}
}
