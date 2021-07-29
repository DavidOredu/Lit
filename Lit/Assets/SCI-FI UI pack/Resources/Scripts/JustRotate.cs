using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class JustRotate : MonoBehaviour {
	public float rotateSpeed = 15f;
 
	void FixedUpdate () {
		transform.Rotate(rotateSpeed * Vector3.forward * Time.deltaTime, Space.Self);
	}
}
