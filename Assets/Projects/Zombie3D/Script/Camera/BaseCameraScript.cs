using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;
public enum CameraType
{
    TPSCamera,
    TopWatchingCamera
}

public abstract class BaseCameraScript : MonoBehaviour
{
    protected float angelH = 0.0f;
    protected float angelV = 0.0f;
    protected float lastUpdateTime;
    protected float deltaTime = 0;

    public Player player;
    public GameScene gameScene;

    public Vector3 cameraDistanceFromPlayerWhenIdle;
    public Vector3 cameraDistanceFromPlayerWhenAimed;

    public float cameraSwingSpeed;
    public float minAngelV;
    public float maxAngelV;
    public float fixedAngelV;
    public bool isAngelVFixed;

    public bool limitReticle;
    public bool allowReticleMove;
    public float reticleLogoRange = 0.15f;
    public float reticleMoveSpeed = 20.0f;
    public float mutipleSizeReticle;

    protected GameObject[] lastTransparentObjList = new GameObject[5];
    protected Vector3 moveTo;
    protected bool behindWall = false;
    public Vector3 cameraDistanceFromPlayer;
    public bool lastInWall = false;
    protected ScreenBloodScript bs;
    protected bool started = false;

    public float CAMERA_AIM_FOV = 22.0f;
    public float CAMERA_NORMAL_FOV = 38.0f;
    protected Vector2 reticlePosition;

    protected Transform cameraTransform;

    protected CameraType cameraType;

    public AudioSource loseAudio;

    public Transform CameraTransform
    {
        get
        {
            return cameraTransform;
        }
    }

    public Vector2 ReticlePosition
    {
        get
        {
            return reticlePosition;
        }
        set
        {
            reticlePosition = value;
        }
    }
        

    public abstract CameraType GetCameraType();

    public virtual void Init()
    {
        gameScene = GameApp.GetInstance().GetGameScene();
        player = gameScene.GetPlayer();
        angelH = player.GetTransform().rotation.eulerAngles.y;
        cameraDistanceFromPlayer = cameraDistanceFromPlayerWhenIdle;

        transform.position = player.GetTransform().TransformPoint(cameraDistanceFromPlayer);
        transform.rotation = Quaternion.Euler(-(angelV), angelH, 0.0f);

        //! Screen.lockCursor = true;
		Cursor.lockState = CursorLockMode.Locked;

		//! Cursor.visible = false;
		Cursor.visible = true;

        reticlePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            cameraSwingSpeed *= 20;
        }
        
        else if (Screen.width == 960)
        {
            cameraSwingSpeed *= 0.4f;
        }

        float[] distancesCull = new float[32];
        for (int i = 0; i < distancesCull.Length; i++)
        {
            distancesCull[i] = 100;
        }
        distancesCull[PhysicsLayer.SKY] = 1000;
        GetComponent<Camera>().layerCullDistances = distancesCull;
        GameObject sbobj = transform.Find("Screen_Blood").gameObject;

        bs = sbobj.GetComponent<ScreenBloodScript>();
        started = true;

        //AudioPlayer.PlayAudio(audio);
        if (!GameApp.GetInstance().GetGameState().MusicOn)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 100;
        }
        
    }

    public virtual void CreateScreenBlood(float damage)
    {

    }

    public virtual void ZoomIn(float deltaTime)
    {
        cameraDistanceFromPlayer.x = Mathf.Lerp(cameraDistanceFromPlayer.x, cameraDistanceFromPlayerWhenAimed.x, deltaTime * Constant.CAMERA_ZOOM_SPEED);      
        cameraDistanceFromPlayer.y = Mathf.Lerp(cameraDistanceFromPlayer.y, cameraDistanceFromPlayerWhenAimed.y, deltaTime * Constant.CAMERA_ZOOM_SPEED);
        cameraDistanceFromPlayer.z = Mathf.Lerp(cameraDistanceFromPlayer.z, cameraDistanceFromPlayerWhenAimed.z, deltaTime * Constant.CAMERA_ZOOM_SPEED);
        GetComponent<Camera>().fov = Mathf.Lerp(GetComponent<Camera>().fov, CAMERA_AIM_FOV, deltaTime * Constant.CAMERA_ZOOM_SPEED);

    }

    public virtual void ZoomOut(float deltaTime)
    {
        cameraDistanceFromPlayer.x = Mathf.Lerp(cameraDistanceFromPlayer.x, cameraDistanceFromPlayerWhenIdle.x, deltaTime * Constant.CAMERA_ZOOM_SPEED);        
        cameraDistanceFromPlayer.y = Mathf.Lerp(cameraDistanceFromPlayer.y, cameraDistanceFromPlayerWhenIdle.y, deltaTime * Constant.CAMERA_ZOOM_SPEED);
        cameraDistanceFromPlayer.z = Mathf.Lerp(cameraDistanceFromPlayer.z, cameraDistanceFromPlayerWhenIdle.z, deltaTime * Constant.CAMERA_ZOOM_SPEED);
        GetComponent<Camera>().fov = Mathf.Lerp(GetComponent<Camera>().fov, CAMERA_NORMAL_FOV, deltaTime * Constant.CAMERA_ZOOM_SPEED);
    }

}

