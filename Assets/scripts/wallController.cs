using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallController : MonoBehaviour {

	public GameObject wallUnit;
	public GameObject plus1s;
	public GameObject plus5s;
	public GameObject plus10s;
	public GameObject speedup10s;
	public GameObject speedup20s;
	public GameObject speedup30s;
	public Vector2 mazeSize;
	public Terrain terrain;
	public Camera mapCamera;
	public Sprite start;
	public Sprite end;
	public Material diffuse;

	void Start () {
		terrain.terrainData.size = new Vector3 (mazeSize.x * 2, terrain.terrainData.size.y, mazeSize.y * 2);
		initMaze ();
		mapCamera.transform.position = new Vector3 (mazeSize.x, 10, mazeSize.y);
		mapCamera.orthographicSize = Mathf.Max (mazeSize.x, mazeSize.y) + 1;
		float slope = (float)Screen.height / (float)Screen.width;
		float MCWidth = slope > 1 ? 0.25f : 0.25f * slope;
		float MCHeight = slope > 1 ? 0.25f / slope : 0.25f;
		mapCamera.rect = new Rect (0.95f - MCWidth, 0.95f - MCHeight, MCWidth, MCHeight);
	}

	int[,] g;
	int[] dx , dy;
	int n, m;
	bool[,] used, ret;

	public void initMaze(){
		foreach (Transform child in transform)
			Destroy (child.gameObject);

		used = new bool[(int)mazeSize.x * 2 + 1, (int)mazeSize.y * 2 + 1];
		ret = new bool[(int)mazeSize.x * 2 + 1, (int)mazeSize.y * 2 + 1];

		for (int i = 0; i < mazeSize.x * 2 + 1; i++) {
			for (int j = 0; j < mazeSize.y * 2 + 1; j++) {
				used [i, j] = false;
				ret [i, j] = true;
			}
		}
		dx = new int[] {0, 0, 1, -1};
		dy = new int[] {1, -1, 0, 0};
		n = (int)mazeSize.x;
		m = (int)mazeSize.y;
		dfs (0, 0, -1, -1);
		for (int i = 0; i <= (int)mazeSize.x * 2; i++) {
			for (int j = 0; j <= (int)mazeSize.y * 2; j++) {
				if ((i == 1 && j == 1) || (i == (int)mazeSize.x * 2 - 1 && j == (int)mazeSize.y * 2 - 1)) {
					GameObject s = new GameObject ((i == 1 && j == 1) ? "start" : "end", typeof(SpriteRenderer));
					s.transform.SetParent(transform);
					s.transform.localPosition = new Vector3 (i, 0.001f, j);
					s.transform.localRotation = new Quaternion(90f, 0, 0, 90f);
					s.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
					s.GetComponent<SpriteRenderer> ().sprite = (i == 1 && j == 1) ? start : end;
					s.GetComponent<SpriteRenderer> ().material = diffuse;
					continue;
				}
				if (ret [i, j]) {
					Instantiate (wallUnit, new Vector3 (i, 1.5f, j), new Quaternion (0, 0, 0, 0), transform);
					continue;
				}
				if (Random.value > 0.95f) {
					if (Random.value > 0.1f)
						Instantiate (plus1s, new Vector3 (i, 1f, j), new Quaternion (0, 0, 0, 0), transform);
					else
						Instantiate (speedup10s, new Vector3 (i, 1f, j), new Quaternion (0, 0, 0, 0), transform);
				} 
				else if (Random.value > 0.98f) {
					if (Random.value > 0.1f)
						Instantiate (plus5s, new Vector3 (i, 1f, j), new Quaternion (0, 0, 0, 0), transform);
					else
						Instantiate (speedup20s, new Vector3 (i, 1f, j), new Quaternion (0, 0, 0, 0), transform);
				}
				else if (Random.value > 0.99f) {
					if (Random.value > 0.1f)
						Instantiate (plus10s, new Vector3 (i, 1f, j), new Quaternion (0, 0, 0, 0), transform);
					else
						Instantiate (speedup30s, new Vector3 (i, 1f, j), new Quaternion (0, 0, 0, 0), transform);
				}
			}
		}
		/*
		for (int i = 0; i <= (int)mazeSize.x; i++) {
			for (int j = 0; j <= (int)mazeSize.y; j++) {
				if (i == 0 || i == (int)mazeSize.x || j == 0 || j == (int)mazeSize.y)
					Instantiate (wallUnit, new Vector3 (i, 2, j), new Quaternion (0, 0, 0, 0), transform);
				else {
					if (Random.value > 0.65)
						Instantiate (wallUnit, new Vector3 (i, 2, j), new Quaternion (0, 0, 0, 0), transform);
				}
			}
		}
		*/
	}
		
	void dfs(int x, int y, int px, int py) {
		if (x < 0 || y < 0 || x >= n || y >= m)
			return;
		if (used [x, y])
			return;
		used [x, y] = true;
		if (px >= 0) {
			ret [((x * 2 + 1) + (px * 2 + 1)) / 2, ((y * 2 + 1) + (py * 2 + 1)) / 2] = false;
		}
		ret [x * 2 + 1, y * 2 + 1] = false;
		int cnt = 0, dir = 0;
		if (x == n - 1 && y == m - 1)
			return;
		while (cnt < 10) {
			dir = Random.Range (0, 4);
			dfs (x + dx [dir], y + dy [dir], x, y);
			cnt++;
		}
	}

}
