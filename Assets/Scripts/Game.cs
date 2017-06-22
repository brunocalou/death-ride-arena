using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Game : NetworkBehaviour {
	public int maxTimeSeconds = 121; // seconds
	public UnityEngine.UI.Text timerText;
	public RectTransform winnerScreen;
	public RectTransform loserScreen;

	[SyncVar(hook = "OnTimeChange")]
	private float timeLeft;
	private bool isGameRunning = false;

	string getTimeLeftString()
	{
		int seconds = ((int)timeLeft) % 60;
		string seconds_str = "" + seconds;
		if (seconds < 10)
		{
			seconds_str = "0" + seconds_str;
		}

		return Mathf.Floor(timeLeft / 60) + ":" + seconds_str;
	}

	// Use this for initialization
	void Start () {
		Debug.Log (winnerScreen);

		hideScreens ();

		Debug.Log ("Start");
		if (isServer) {
			timeLeft = maxTimeSeconds;
			this.isGameRunning = true;
			RpcStartGame ();
		}
	}

	void OnTimeChange (float time) {
		this.timeLeft = time;
		timerText.text = getTimeLeftString();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;

//		Debug.Log ("Update");
		if (this.isGameRunning) {
//			Debug.Log ("Game is running");
			timeLeft -= Time.deltaTime;

			if (timeLeft < 0) {
				timeLeft = 0;

				Health[] healthScripts = FindObjectsOfType<Health> ();

				if (healthScripts.Length > 0) {
					Health winner = healthScripts [0];

					foreach (var health in healthScripts) {
						if (health.getNumberOfDeaths () < winner.getNumberOfDeaths ()) {
							winner = health;
						}
						Debug.Log (health.getNumberOfDeaths ());
					}

					Debug.Log (winner);

					RpcEndGame (winner.GetComponent<NetworkIdentity> ().netId);
					Invoke ("RpcRestartGame", 5);
				}
			}
		}
	}

	[ClientRpc]
	void RpcRestartGame () {
		// TODO: Reload the scene correctly (the line below resets the scene, but all the players are destroyed
		// and the scenario objects are deactivated
//		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		RpcStartGame ();
	}

	[ClientRpc]
	void RpcStartGame () {
		timeLeft = maxTimeSeconds;
		this.isGameRunning = true;
		hideScreens ();

		Health[] healthScripts = FindObjectsOfType<Health> ();

		if (healthScripts.Length > 0) {
			foreach (var health in healthScripts) {
				health.resetDeaths ();
			}
		}
		Debug.Log ("Start game");
	}

	void hideScreens () {
		if (winnerScreen != null) {
			winnerScreen.gameObject.SetActive (false);
		}

		if (loserScreen!= null) {
			loserScreen.gameObject.SetActive (false);
		}
	}

	void showWinnerScreen () {
		if (winnerScreen != null) {
			winnerScreen.gameObject.SetActive (true);
		}
	}

	void showLoserScreen () {
		if (loserScreen!= null) {
			loserScreen.gameObject.SetActive (true);
		}
	}

	[ClientRpc]
	void RpcEndGame (NetworkInstanceId winnerId) {
		Debug.Log ("End of the game");
		this.isGameRunning = false;

		PlayerMovement[] players = FindObjectsOfType<PlayerMovement> ();
		bool isWinner= false;

		foreach (PlayerMovement player in players) {
			if (player.isLocalPlayer) {
				if (player.GetComponent<NetworkIdentity> ().netId == winnerId) {
					showWinnerScreen ();
					isWinner = true;
				}
			}
		}

		if (!isWinner) {
			showLoserScreen ();
		}

		Debug.Log (isWinner);
	}
}
