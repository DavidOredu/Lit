using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustRotate : MonoBehaviour {

public bool canRotate=true;
public float rotateSpeed=10;
 
	void Update ()
	{
		if(canRotate)
		  transform.Rotate(rotateSpeed*Vector3.forward*Time.deltaTime);
	}
}
