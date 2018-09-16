using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip mainEngine;

    enum State {
        Alive, Dying, Transcending
    }
    State state = State.Alive;


    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
    //TODO somewhere stop sound.
	void Update () {
        if(state == State.Alive) {
            Thrusting();
            Rotate();
        }
	}

    void OnCollisionEnter(Collision collision) {

        if (state != State.Alive) {
            return;
        }

        switch(collision.gameObject.tag) {
            case "friendly": 
                    print("hit friendly");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f); //paramertise this
                break;
            default:
                state = State.Dying;
                Invoke("LoadPreviousLevel", 1f);
                print("Dead");
                return;

        }
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(1);
    }

    private void LoadPreviousLevel() {
        SceneManager.LoadScene(1);
    }

    private void Rotate() {
     
        rigidbody.freezeRotation = true; // take manual control of rotation


        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbody.freezeRotation = false; // let physics take control of rotation now.
    }

    private void Thrusting() {
        float thrustingThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) { //can thrust while rotating.
            rigidbody.AddRelativeForce(Vector3.up* thrustingThisFrame);
            if (!audiosource.isPlaying) {
                audiosource.PlayOneShot(mainEngine);
            }
        } else {
            audiosource.Stop();
        }
    }


}
