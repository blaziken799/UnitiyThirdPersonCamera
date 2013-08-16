/// <summary>
/// UnityTutorials - A Unity Game Design Prototyping Sandbox
/// <copyright>(c) John McElmurray and Julian Adams 2013</copyright>
/// 
/// UnityTutorials homepage: https://github.com/jm991/UnityTutorials
/// 
/// This software is provided 'as-is', without any express or implied
/// warranty.  In no event will the authors be held liable for any damages
/// arising from the use of this software.
///
/// Permission is granted to anyone to use this software for any purpose,
/// and to alter it and redistribute it freely, subject to the following restrictions:
///
/// 1. The origin of this software must not be misrepresented; you must not
/// claim that you wrote the original software. If you use this software
/// in a product, an acknowledgment in the product documentation would be
/// appreciated but is not required.
/// 2. Altered source versions must be plainly marked as such, and must not be
/// misrepresented as being the original software.
/// 3. This notice may not be removed or altered from any source distribution.
/// </summary>

using UnityEngine;
using System.Collections;

/// <summary>
/// #DESCRIPTION OF CLASS#
/// </summary>
public class TestCamera : MonoBehaviour
{
	#region variables (private)
	
	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform followXForm;
	
	///Private global only
	private Vector3 lookDir;
	private Vector3 targetPosition;
	
	///Smoothing and damping
	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime = 0.1f;
	
	#endregion


	#region Properties (public)

	#endregion


	#region unity event functions

	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start () 
	{
		followXForm = GameObject.FindWithTag("Player").transform;
	}
	
	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update () 
	{
	
	}

	/// <summary>
	/// Debugging information should be put here.
	/// </summary>
	void OnDrawGizmos ()
	{
		
	}
	
	void LateUpdate()
	{
		Vector3 characterOffset = followXForm.position + new Vector3(0f, distanceUp, 0f);
		
		/// Calculate direction from camera to player, kill Y, and normalise to give a valid direction with unit magnitude
		lookDir = characterOffset - this.transform.position;
		lookDir.y = 0;
		lookDir.Normalize();
		Debug.DrawRay(this.transform.position, lookDir, Color.green);
		
		
		/// setting the target position to be the correct offset from the hovercraft
		targetPosition = characterOffset + followXForm.up * distanceUp - lookDir * distanceAway;
	//	Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red);
	//	Debug.DrawRay(follow.position, -1f * follow.forward * distanceAway, Color.blue);
	//	Debug.DrawLine(followXForm.position, targetPosition, Color.magenta);
		
		CompensateForWalls(characterOffset, ref targetPosition);
		
		smoothPosition(this.transform.position, targetPosition);
		
		// make sure the camera is looking the right way!
		transform.LookAt(characterOffset);
	}
	
	#endregion


	#region Methods
	
	private void smoothPosition(Vector3 fromPos, Vector3 toPos)
	{
		///making a smooth transition between camera's current position and the position it want t be in
		this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}
	
	private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
	{
		Debug.DrawLine(fromObject, toTarget, Color.cyan);
		/// Compensate for walls between camera
		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast(fromObject, toTarget, out wallHit))
		{
			Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
			toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z); 
		}
	}
	
	#endregion Methods
}