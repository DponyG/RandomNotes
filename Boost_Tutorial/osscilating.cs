using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] //Require one component
public class occilator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(20f, 0, 0); // will move 10 units in 2 seconds
    [SerializeField] float period = 5f;

    
    [Range(0,1)]
    [SerializeField]
    float movementFactor; // 0 for not moved for 1 for fully moved

    Vector3 startingPos;

    // Use this for initialization
    void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //TODO protect against period zero

        if (period <= Mathf.Epsilon) { return; }

            float cycles = Time.time / period; //grows from 0
            const float tau = Mathf.PI * 2f;
            float rawSinWave = Mathf.Sin(cycles * tau);
            print(rawSinWave);

            //Vector3 offset = movementVector * movementFactor;
            //transform.position = offset + startingPos; for manuel movement

            movementFactor = rawSinWave / (2f + .5f); //1 to -1 then .5 to -.5 add .5 goes to 1
            Vector3 offset = movementFactor * movementVector;
            transform.position = offset + startingPos;
        

	}
}
