using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	public enum PlayerMode
	{
		FirstPerson,
		ThirdPerson
	}

	public float translateSpeed;
	public float rotateSpeed;
	public Vector3 firstPersonCameraPos;
	public Vector3 thirdPersonCameraPos;
	public Vector3 firstPersonPlayerPos;
	public Vector3 thirdPersonPlayerPos;
	public Animator playerAnimator = null;
	public bool crouch = false;
	Camera[] cameras;
	Transform playerCam;
	Vector3 transVal;
	[HideInInspector]
	public bool isMovable;
	bool isCameraStand;
	[HideInInspector]
	public PlayerMode pm= PlayerMode.FirstPerson; 

	float modeKeyTime;

	void Start () {
		cameras = transform.GetComponentsInChildren<Camera> ();
		switchMode (pm);
		Cursor.lockState = CursorLockMode.Locked;
		isMovable = true;
		isCameraStand = true;
		modeKeyTime = 0;
	}

	void Update () {
		if (playerCam == null)
			return;

		#region WASD Moves and Mouse Rotation
		if (isMovable) {
			transVal = new Vector3(0, 0, 0);
			if (Input.GetKey(KeyCode.W)) {
				Vector3 f = transform.forward;
				f.y = 0;
				f = Vector3.Normalize(f);
				transVal += f * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.S)) {
				Vector3 f = transform.forward;
				f.y = 0;
				f = Vector3.Normalize(f);
				transVal -= f * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.A)) {
				Vector3 r = transform.right;
				r.y = 0;
				r = Vector3.Normalize(r);
				transVal -= r * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.D)) {
				Vector3 r = transform.right;
				r.y = 0;
				r = Vector3.Normalize(r);
				transVal += r * Time.deltaTime;
			}
			transform.Translate(transVal * translateSpeed, Space.World);
			if (playerAnimator != null){
				playerAnimator.SetFloat("Speed", transVal == Vector3.zero ? 0 : (Vector3.Dot(transform.forward, transVal) > 0 ? translateSpeed : -translateSpeed));
			}

			if (Input.GetKey(KeyCode.LeftShift) && crouch && pm != PlayerMode.ThirdPerson) {
				if (isCameraStand) {
					playerCam.transform.Translate(new Vector3(0, -2f, 0), Space.World);
					isCameraStand = false;
				}
			}
			else {
				if (!isCameraStand) {
					playerCam.transform.Translate(new Vector3(0, 2f, 0), Space.World);
					isCameraStand = true;
				}
			}

			float rotX = Input.GetAxis("Mouse X");
			if (rotX != 0)
				transform.Rotate(Vector3.up * rotX * rotateSpeed);
			float rotY = Input.GetAxis("Mouse Y");
			if (rotY != 0 && pm != PlayerMode.ThirdPerson) {
				if (Mathf.Round(playerCam.rotation.eulerAngles.x) <= 90 && playerCam.rotation.eulerAngles.x + Vector3.left.x * rotY * rotateSpeed > 90)
					playerCam.rotation = Quaternion.Euler(new Vector3(90, playerCam.rotation.eulerAngles.y, playerCam.rotation.eulerAngles.z));
				else if (Mathf.Round(playerCam.rotation.eulerAngles.x) >= 270 && playerCam.rotation.eulerAngles.x + Vector3.left.x * rotY * rotateSpeed < 270)
					playerCam.rotation = Quaternion.Euler(new Vector3(270, playerCam.rotation.eulerAngles.y, playerCam.rotation.eulerAngles.z));
				else
					playerCam.Rotate(Vector3.left * rotY * rotateSpeed);
			}
		}
		#endregion

		modeKeyTime += Time.deltaTime;
		if (Input.GetKey (KeyCode.Tab) && modeKeyTime > 1f) {
			if (pm == PlayerMode.FirstPerson)
				switchMode (PlayerMode.ThirdPerson);
			else if (pm == PlayerMode.ThirdPerson)
				switchMode (PlayerMode.FirstPerson);
			modeKeyTime = 0f;
		}
	}

	public void switchMode(PlayerMode m){
		pm = m;
		if (m == PlayerMode.FirstPerson) {
			if (cameras.Length > 1) {
				playerCam = cameras [0].transform;
				cameras [0].gameObject.SetActive (true);
				cameras [1].gameObject.SetActive (false);
			}
			playerCam.localPosition = firstPersonCameraPos;
			playerCam.localRotation = new Quaternion(0, 0, 0, 0);
			playerAnimator.transform.localPosition = firstPersonPlayerPos;
		} 
		else if (m == PlayerMode.ThirdPerson) {
			if (cameras.Length > 1) {
				playerCam = cameras [1].transform;
				cameras [0].gameObject.SetActive (false);
				cameras [1].gameObject.SetActive (true);
			}
			playerCam.localPosition = thirdPersonCameraPos;
			playerCam.localRotation = new Quaternion(-90f, 0, 0, -90f);
			playerAnimator.transform.localPosition = thirdPersonPlayerPos;
		}
	}
}