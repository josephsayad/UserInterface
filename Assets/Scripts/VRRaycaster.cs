using UnityEngine;
using UnityEngine.Events;

public class VRRaycaster : MonoBehaviour {

	[System.Serializable]
	public class Callback : UnityEvent<Ray, RaycastHit> {}

	public Transform leftHandAnchor = null;
	public Transform rightHandAnchor = null;
	public Transform centerEyeAnchor = null;
	public LineRenderer lineRenderer = null;
	public float maxRayDistance = 500.0f;
	public LayerMask excludeLayers;
	public VRRaycaster.Callback raycastHitCallback;
	public GameObject UserInterface;
	public GameObject[] Models;

	private bool mounted = false;
	private uint entityToMount;
	private Vector3 mountDistance = new Vector3(1.0f, 1.0f, 1.0f);
	private float zDist = 3.0f;
	private float xDist = 0.0f;

	void Awake() {
		if (leftHandAnchor == null) {
			Debug.LogWarning ("Assign LeftHandAnchor in the inspector!");
			GameObject left = GameObject.Find ("LeftHandAnchor");
			if (left != null) {
				leftHandAnchor = left.transform;
			}
		}
		if (rightHandAnchor == null) {
			Debug.LogWarning ("Assign RightHandAnchor in the inspector!");
			GameObject right = GameObject.Find ("RightHandAnchor");
			if (right != null) {
				rightHandAnchor = right.transform;
			}
		}
		if (centerEyeAnchor == null) {
			Debug.LogWarning ("Assign CenterEyeAnchor in the inspector!");
			GameObject center = GameObject.Find ("CenterEyeAnchor");
			if (center != null) {
				centerEyeAnchor = center.transform;
			}
		}
		if (lineRenderer == null) {
			Debug.LogWarning ("Assign a line renderer in the inspector!");
			lineRenderer = gameObject.AddComponent<LineRenderer> ();
			lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			lineRenderer.receiveShadows = false;
			lineRenderer.widthMultiplier = 0.02f;
		}
	}

	Transform Pointer {
		get {
			OVRInput.Controller controller = OVRInput.GetConnectedControllers ();
			if ((controller & OVRInput.Controller.LTrackedRemote) != OVRInput.Controller.None) {
				return leftHandAnchor;
			} else if ((controller & OVRInput.Controller.RTrackedRemote) != OVRInput.Controller.None) {
				return rightHandAnchor;
			}
			// If no controllers are connected, we use ray from the view camera.
			// This looks super ackward! Should probably fall back to a simple reticle!
			return centerEyeAnchor;
		}
	}

	void Update() {

		Transform pointer = Pointer;

		if (pointer == null) {
			return;
		}

		Ray laserPointer = new Ray (pointer.position, pointer.forward);

		if (lineRenderer != null) {
			lineRenderer.SetPosition (0, laserPointer.origin);
			lineRenderer.SetPosition (1, laserPointer.origin + laserPointer.direction * maxRayDistance);
		}

		/* Handle raycast hits when raycaster is not mounted & handle entityMounted
		 * when raycaster is mounted
		 */

		 if(!mounted) {

			 RaycastHit hit;
			 if (Physics.Raycast (laserPointer, out hit, maxRayDistance, ~excludeLayers)) {
			 		if (lineRenderer != null) {
			 			lineRenderer.SetPosition (1, hit.point);
			 		}

			 		if (raycastHitCallback != null) {
			 			raycastHitCallback.Invoke (laserPointer, hit);
			 		}

					// GameObject must be tagged for further functionality
			 		if(hit.transform.tag == "Untagged") {

			 		} else if(hit.transform.tag == "Button" && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) {

			 				// Sends a message. If model button, simple update. If mount button, checks to see if it
			 				// exists in scene. If yes, does nothing. If no, sends message back to VRRaycaster with
			 				// GameObject Name: VRRaycasterObject.SendMessage('Mount', ModelName); Which will make
			 				// it active and mount it to raycaster. Update position until drop.
			 				UserInterface.SendMessage("OnVRTriggerDown", hit.transform.name);
			 		}
			 }

		 } else if(mounted) {

			 // If PrimaryIndexTrigger down while mount, set mounted to false and
			 // activate user interface
			 if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) {
				 mounted = false;
				 UserInterface.SetActive(true);
			 } else { // else update position of mounted entity

				 // If entity is mounted, touchpad can be used to move entity along z
				 // x-axes
				 if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad)) {
					 if(GetDirection(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote)) == Vector2.up) {
						 zDist += 0.1f;
					 } else if(GetDirection(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote)) == Vector2.down) {
						 zDist -= 0.1f;
					 } else if(GetDirection(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote)) == Vector2.left) {
						 xDist -= 0.1f;
					 } else if(GetDirection(OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote)) == Vector2.right) {
						 xDist += 0.1f;
					 }
				 }

				 mountDistance.x = laserPointer.origin.x + laserPointer.direction.x + xDist;
				 mountDistance.y = laserPointer.origin.y + laserPointer.direction.y;
				 mountDistance.z = zDist; // value here dependent on touchpad interaction

				 Models[entityToMount].transform.position = mountDistance;
			 }
		 }
	}

	void Mount(uint id) {
		mounted = true;
		entityToMount = id;
		Models[entityToMount].SetActive(true);
		UserInterface.SetActive(false);
	}

	Vector2 GetDirection(Vector2 input) {
		Vector2[] directions = new Vector2[] {
      Vector2.up,
      Vector2.right,
      Vector2.down,
      Vector2.left
    };

		Vector2 direction = Vector2.zero;
    float max = Mathf.NegativeInfinity;

    foreach (Vector2 vec in directions) {
    	float dot = Vector2.Dot (vec, input.normalized);

      if (dot > max) {
      	direction = vec;
      	max = dot;
      }
    }

    return direction;
	}
}
