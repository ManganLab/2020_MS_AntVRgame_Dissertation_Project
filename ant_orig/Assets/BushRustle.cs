using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushRustle : MonoBehaviour {

    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = this.GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        audio.Play();
        Debug.Log("Entered collider");
    }

    private void OnTriggerExit(Collider other)
    {
        audio.Stop();
        Debug.Log("Exit collider");
    }


}
