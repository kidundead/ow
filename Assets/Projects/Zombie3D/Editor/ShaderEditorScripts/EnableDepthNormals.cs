using UnityEngine;

[RequireComponent (typeof (Camera))]
public class EnableDepthNormals : MonoBehaviour {
	void Awake () {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
	}
}
