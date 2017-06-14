using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	bool isActive = true;
	GameObject menu;

	void Start () {
		this.menu = GameObject.Find ("Network Manager");
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			toggleMenu ();
		}
	}
	
	void toggleMenu() {
		if (this.menu != null) {
			this.isActive = !this.isActive;
			this.menu.SetActive (this.isActive);
		}
	}
}