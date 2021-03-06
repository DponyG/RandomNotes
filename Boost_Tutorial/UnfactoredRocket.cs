using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    AudioSource audiosource;
    

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    
    private void ProcessInput() {
        if(Input.GetKey(KeyCode.Space)) { //can thrust while rotating.
            rigidbody.AddRelativeForce(Vector3.up);
            if (!audiosource.isPlaying) {
                audiosource.Play();
            }
        } else {
            audiosource.Stop();
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward);
        }
    }
}
