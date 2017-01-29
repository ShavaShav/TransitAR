using UnityEngine;
using System.Collections;

public class Bus : MonoBehaviour {
	
	private Vector3 newVectorForward;
	private Vector3 newVectorBack; 
	private	bool goingF = true;
	private bool goingB = false;
	// Use this for initialization
	void Start () {
		newVectorForward = new Vector3(gameObject.transform.position.x + 5f, gameObject.transform.position.y, gameObject.transform.position.z);
		newVectorBack = new Vector3(gameObject.transform.position.x - 5f, gameObject.transform.position.y, gameObject.transform.position.z);
	}
	
	// Update is called once per frame
	/*
	void Update () {
		if (goingF && transform.position != newVectorForward){
			transform.position = Vector3.MoveTowards (transform.position, newVectorForward, Time.deltaTime);
		} else if (transform.position == newVectorForward){
			goingF = false;
			goingB = true;
		}
		if (goingB && transform.position != newVectorBack) {
			transform.position = Vector3.MoveTowards (transform.position, newVectorBack, Time.deltaTime);
		} else if (transform.position == newVectorForward){
			goingF = true;
			goingB = false;
		}

	}*/
}
