using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class systemController : MonoBehaviour {

	public GameObject mapCam;
	float mapKeyTime;
	public playerController player;
	public wallController wc;
	public Text gameTimeText;
	float gameTime;
	public Text speedupTimeText;
	float speedupTime;
	float translateSpeed;
	public GameObject timePanel;
	public GameObject menuPanel;
	public GameObject winPanel;
	public GameObject losePanel;
	public Text bestRecord;

	private bool isGameStart;

	public enum GameState{
		ready,
		playing,
		win,
		lose
	}

	private GameState gameState;

	void Start(){
		mapKeyTime = 0f;
		translateSpeed = player.translateSpeed;
		switchGameState (GameState.ready);
	}

	void Update () {
		mapKeyTime += Time.deltaTime;
		if (Input.GetKey (KeyCode.M) && mapKeyTime > 1f) {
			mapCam.SetActive(mapCam.activeSelf ? false : true);
			mapKeyTime = 0f;
		}
		if (isGameStart) {
			speedupTime = speedupTime < Time.deltaTime ? 0f : speedupTime - Time.deltaTime;
			gameTime = gameTime < Time.deltaTime ? 0f : gameTime - Time.deltaTime;
			if (gameTime == 0f)
				switchGameState (GameState.lose);
		}

		if (speedupTime == 0f) {
			player.translateSpeed = translateSpeed / 1.5f;
			speedupTimeText.gameObject.SetActive (false);
		}
		speedupTimeText.text = ((int)speedupTime / 60).ToString ("00") + ":" + ((int)speedupTime % 60).ToString ("00") + ":" + ((int)(speedupTime * 100) % 100).ToString ("00");
		gameTimeText.text = ((int)gameTime / 60).ToString ("00") + ":" + ((int)gameTime % 60).ToString ("00") + ":" + ((int)(gameTime * 100) % 100).ToString ("00");
		gameTimeText.color = gameTime > 20f ? new Color (50f / 255f, 50f / 255f, 50f / 255f) : new Color (1, 50f / 255f, 50f / 255f);


		if (Vector3.Distance (player.transform.position, new Vector3 (1, 0, 1)) > 1f)
			isGameStart = true;

		if (Vector3.Distance (player.transform.position, new Vector3 (wc.mazeSize.x * 2 - 1, 0, wc.mazeSize.y * 2 - 1)) < 1f)
			switchGameState (GameState.win);
	}

	public void addGameTime(float time){
		gameTime += time;
	}

	public void addSpeedupTime(float time){
		speedupTime += time;
		player.translateSpeed = translateSpeed * 1.5f;
		speedupTimeText.gameObject.SetActive (true);
	}

	public void switchGameState(GameState gs){
		switch (gs) {
		case GameState.ready:
			wc.initMaze ();
			player.isMovable = false;
			player.transform.position = new Vector3 (1, 0, 1);
			player.switchMode (playerController.PlayerMode.FirstPerson);
			Cursor.lockState = CursorLockMode.None;
			gameTime = 120f;
			speedupTime = 0f;
			timePanel.SetActive (false);
			menuPanel.SetActive (true);
			winPanel.SetActive (false);
			losePanel.SetActive (false);
			isGameStart = false;
			break;
		case GameState.playing:
			player.isMovable = true;
			Cursor.lockState = CursorLockMode.Locked;
			timePanel.SetActive (true);
			menuPanel.SetActive (false);
			break;
		case GameState.win:
			player.isMovable = false;
			Cursor.lockState = CursorLockMode.None;
			isGameStart = false;
			if (gameTime > PlayerPrefs.GetFloat ("best", 0f)) {
				PlayerPrefs.SetFloat ("best", gameTime);
				PlayerPrefs.Save ();
			}
			winPanel.SetActive (true);
			float b = PlayerPrefs.GetFloat ("best", gameTime);
			bestRecord.text = "Best Record : " + ((int)b / 60).ToString ("00") + ":" + ((int)b % 60).ToString ("00") + ":" + ((int)(b * 100) % 100).ToString ("00");
			break;
		case GameState.lose:
			player.isMovable = false;
			Cursor.lockState = CursorLockMode.None;
			isGameStart = false;
			losePanel.SetActive (true);
			break;
		default:
			break;
		}
	}

	public void onStartBtnPress(){
		switchGameState (GameState.playing);
	}

	public void onRetryBtnPress(){
		switchGameState (GameState.ready);
	}
}
