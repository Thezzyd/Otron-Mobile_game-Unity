using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAndGo : MonoBehaviour {
	
	//[SerializeField]
	//float moveSpeed = 5f;

	//Rigidbody2D rb;

	Touch touch;
	Vector3 touchPosition, whereToMove;
	//bool isMoving = false;

	//float previousDistanceToTouchPos, currentDistanceToTouchPos;

	void Start () 
	{
	//	rb = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {

		/*if (isMoving)
			currentDistanceToTouchPos = (touchPosition - transform.position).magnitude;*/
		
		if (Input.touchCount > 0) {
			touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Began)
				{
					//previousDistanceToTouchPos = 0;
					//currentDistanceToTouchPos = 0;
				//	isMoving = true;
					touchPosition = Camera.main.ScreenToWorldPoint (touch.position);
					touchPosition.z = 0;
					whereToMove = (touchPosition - transform.position).normalized;
				//rb.velocity = new Vector2 (whereToMove.x * moveSpeed, whereToMove.y * moveSpeed);
				transform.position = new Vector2(whereToMove.x, whereToMove.y);
				}
		}

		/*if (currentDistanceToTouchPos > previousDistanceToTouchPos) {
			isMoving = false;
			rb.velocity = Vector2.zero;
		}*/

		/*if (isMoving)
			previousDistanceToTouchPos = (touchPosition - transform.position).magnitude*/
	}
}
