//! @file DebugInfo.cs


using UnityEngine;


//! @class DebugInfo
public class DebugInfo : MonoBehaviour
{
	public static bool Enable = true;

	public void OnGUI()
	{
// 	#if !UNITY_IPHONE
// 		GUILayout.Label("DebugInfo " + Enable);
// 		GUILayout.Label("FPS " + 1.0f / Time.deltaTime);
// 		GUILayout.Label("All " + FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length);
// 		GUILayout.Label("Textures " + FindObjectsOfTypeAll(typeof(Texture)).Length);
// 		GUILayout.Label("AudioClips " + FindObjectsOfTypeAll(typeof(AudioClip)).Length);
// 		GUILayout.Label("Meshes " + FindObjectsOfTypeAll(typeof(Mesh)).Length);
// 		GUILayout.Label("Materials " + FindObjectsOfTypeAll(typeof(Material)).Length);
// 		GUILayout.Label("GameObjects " + FindObjectsOfTypeAll(typeof(GameObject)).Length);
// 		GUILayout.Label("Components " + FindObjectsOfTypeAll(typeof(Component)).Length);
// 	#endif
    }
}

