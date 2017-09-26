using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemController : MonoBehaviour {

	public enum itemType{
		plus1s,
		plus5s,
		plus10s,
		speedup10s,
		speedup20s,
		speedup30s
	}
	Transform sprite;
	Transform player;
	public itemType type;
	systemController sc;

	void Start () {
		sprite = GetComponentInChildren<SpriteRenderer> ().transform;
		player = GameObject.Find ("player").transform;
		sc = Object.FindObjectOfType<systemController> ();
	}
	

	void Update () {
		switch (player.GetComponent<playerController> ().pm) {
		case playerController.PlayerMode.FirstPerson:
			sprite.forward = new Vector3 (player.position.x - sprite.position.x, 0, player.position.z - sprite.position.z); 
			break;
		case playerController.PlayerMode.ThirdPerson:
			sprite.forward = new Vector3 (player.position.x - sprite.position.x, 1, player.position.z - sprite.position.z); 
			break;
		default:
			break;
		}
	}

	void OnTriggerEnter(Collider c){
		switch (type) {
		case itemType.plus1s:
			sc.addGameTime (1f);
			break;
		case itemType.plus5s:
			sc.addGameTime (5f);
			break;
		case itemType.plus10s:
			sc.addGameTime (10f);
			break;
		case itemType.speedup10s:
			sc.addSpeedupTime (10f);
			break;
		case itemType.speedup20s:
			sc.addSpeedupTime (20f);
			break;
		case itemType.speedup30s:
			sc.addSpeedupTime (30f);
			break;
		default:
			break;
		}
		Destroy (gameObject);
	}
}
