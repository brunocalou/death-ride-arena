using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenu : MonoBehaviour {

	public AudioClip[] audio1;

	private Canvas canvas;
	private bool isShowing = false;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		canvas = this.gameObject.GetComponent<Canvas> ();
		canvas.enabled = isShowing;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			isShowing = !isShowing;
			canvas.enabled = isShowing;
		}

		if (audioSource.isPlaying) {
			return ;
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			audioSource.PlayOneShot (audio1 [0]);
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			audioSource.PlayOneShot (audio1 [1]);
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			audioSource.PlayOneShot (audio1 [2]);
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			audioSource.PlayOneShot (audio1 [3]);
		} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			audioSource.PlayOneShot (audio1 [4]);
		} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
			audioSource.PlayOneShot (audio1 [5]);
		} else if (Input.GetKeyDown (KeyCode.Alpha7)) {
			audioSource.PlayOneShot (audio1 [6]);
		} else if (Input.GetKeyDown (KeyCode.Alpha8)) {
			audioSource.PlayOneShot (audio1 [7]);
		} else if (Input.GetKeyDown (KeyCode.Alpha9)) {
			audioSource.PlayOneShot (audio1 [8]);
		} else if (Input.GetKeyDown (KeyCode.Alpha0)) {
			audioSource.PlayOneShot (audio1 [9]);
		}
	}
}
