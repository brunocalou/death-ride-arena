//using UnityEngine;
//using System.Collections;
//using UnityEngine.Networking;
//
//public class PlayerName : NetworkBehaviour {
//
//	[SyncVar(hook="OnPlayerName")]
//	string playerName;
//	public UnityEngine.UI.Text nameText;
//
//	void OnPlayerName (string name) {
//		setName (name);
//	}
//
//	public string getName () {
//		return playerName;
//	}
//
//	public void setName(string newName) {
//		Debug.Log ("Set name to " + newName);
//		playerName = newName;
////		if (nameText == null) {
////			nameText = transform.Find ("Name Canvas/Name").gameObject;
////		}
//		nameText.text = playerName;
//	}
//
////	public override void OnStartClient() {
////		playerName = NameGenerator.getName ();
////	}
//
////	private void Start ()
////	{
////		NetworkInstanceId playerId = GetComponent<NetworkIdentity> ().netId;
////
////		if (isLocalPlayer) {
////			this.playerName = NameGenerator.getName ();
////			CmdSetName (playerId, this.playerName);
////		}
//////		if (isServer) {
//////			Debug.Log ("Player name start");
//////			RpcSetName (playerId, playerName);
//////		}
////	}
////
////	[Command]
////	void CmdSetName(NetworkInstanceId playerId, string name) {
////		RpcSetName (playerId, name);
////	}
////
//	[ClientRpc]
//	void RpcSetName(NetworkInstanceId id, string name) {
//		Debug.Log ("Rpc set name");
////		if (isLocalPlayer) {
//			GameObject player = ClientScene.FindLocalObject (id);
//
//			if (player != null) {
//				PlayerName playerNameScript = player.GetComponent<PlayerName> ();
//				if (playerNameScript != null) {
//					playerNameScript.setName (playerName);
//				}
//			}
////		}
//	}
////
////	Vector3 screenPos;
////
////	void Update ()
////	{
////		screenPos = Camera.main.WorldToScreenPoint (transform.position);
////	}
////
////	void LateUpdate () {
////		nameText.transform.position = screenPos;
////	}
//}
