using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;
[AddComponentMenu("TPS/TopWatchingCamera")]
public class TopWatchingCameraScript : BaseCameraScript
{

    protected bool cameraset = false;
    protected Vector3 absoluteDistanceFromPlayer;

    public override CameraType GetCameraType()
    {
        return CameraType.TopWatchingCamera;
    }




    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {

    }

    public override void Init()
    {
        base.Init();

        moveTo = player.GetTransform().TransformPoint(cameraDistanceFromPlayer);

        absoluteDistanceFromPlayer = moveTo - player.GetTransform().position;


        transform.LookAt(player.GetTransform());

        started = true;

    }



    public override void CreateScreenBlood(float damage)
    {
        if (bs != null)
        {
            bs.NewBlood(damage);
        }
        else
        {
            Debug.Log("bs null");
        }
    }

    void Update()
    {

    }
    
    void LateUpdate()
    {
        if (!started)
            return;

        deltaTime = Time.deltaTime;
        
        
        float rx = player.InputController.CameraRotation.x;
        float ry = player.InputController.CameraRotation.y;

        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            rx = Input.GetAxis("Mouse X");
            ry = Input.GetAxis("Mouse Y");
        }
        
        angelH += rx * deltaTime * cameraSwingSpeed;
        angelV += ry * deltaTime * cameraSwingSpeed;

        angelV = Mathf.Clamp(angelV, minAngelV, maxAngelV);

        float smooth = 100f;
        if (gameScene.PlayingState == PlayingState.GamePlaying)
        {
            player.GetTransform().rotation = Quaternion.Euler(0.0f, angelH, 0.0f);

            moveTo = player.GetTransform().position + absoluteDistanceFromPlayer;
            cameraTransform.position = moveTo;// Vector3.Lerp(cameraTransform.position, moveTo, smooth * Time.time);
        }


        lastUpdateTime = Time.time;
    }


}

