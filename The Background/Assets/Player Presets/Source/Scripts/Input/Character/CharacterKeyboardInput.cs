using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This character movement input class is an example of how to get input from a keyboard to control the character;
    public class CharacterKeyboardInput : CharacterInput
    {
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		public KeyCode jumpKey = KeyCode.Space;
		public KeyCode runningKey = KeyCode.LeftShift;
		public KeyCode crouchKey = KeyCode.LeftControl;

		//If this is enabled, Unity's internal input smoothing is bypassed;
		public bool useRawInput = true;

        public override float GetHorizontalMovementInput()
		{
			if(useRawInput)
				return Input.GetAxisRaw(horizontalInputAxis);
			else
				return Input.GetAxis(horizontalInputAxis);
		}

		public override float GetVerticalMovementInput()
		{
			if(useRawInput)
				return Input.GetAxisRaw(verticalInputAxis);
			else
				return Input.GetAxis(verticalInputAxis);
		}
		public override bool IsRunningKeyPressed()
		{
			return Input.GetKey(runningKey);
		}
		public override bool IsRunningKeyLetGo()
		{
			return Input.GetKeyUp(runningKey);
		}

		public override bool IsJumpKeyPressed()
		{
			return Input.GetKey(jumpKey);
		}

	}
}
