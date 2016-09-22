using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;
[AddComponentMenu("TPS/TPSSimpleCamera")]
public class TPSSimpleCameraScript : BaseCameraScript
{

    public Texture reticle;

    public Texture leftTopReticle;
    public Texture rightTopReticle;
    public Texture leftBottomReticle;
    public Texture rightBottomReticle;

    protected Shader transparentShader;
    protected Shader solidShader;
    

    protected float drx;
    protected float dry;

    protected float winTime = -1;
    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    public override CameraType GetCameraType()
    {
        return CameraType.TPSCamera;
    }

    // Use this for initialization
    void Start()
    {
        solidShader = Shader.Find("iPhone/LightMap");
        transparentShader = Shader.Find("iPhone/AlphaBlend_Color");
        GetComponent<Camera>().GetComponent<AudioSource>().Play();
        //AudioSource backgroundMusic = cameraTransform.Find("backgroundMusic").audio;
        //backgroundMusic.Play();
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
        if (!GetComponent<Camera>().GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<Camera>().GetComponent<AudioSource>().Play();
        }
    }

    void LateUpdate()
    {
        if (!started)
            return;

        deltaTime = Time.deltaTime;




        //Iphone: camera rotation from input controller
        float rx = player.InputController.CameraRotation.x;
        float ry = player.InputController.CameraRotation.y;

        //Windows: camera rotation from mouse input

        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (player.InputController.EnableTurningAround)
            {

                rx = Input.GetAxis("Mouse X") * 30 * Time.deltaTime;
                ry = Input.GetAxis("Mouse Y") * 30 * Time.deltaTime;
            }

        }


        float distance = reticlePosition.x - Screen.width / 2;

        //Reticle in range, do not move camera, move reticle.
        if (allowReticleMove)
        {
            if (Mathf.Abs(distance) < reticleLogoRange * Screen.width || distance * rx < 0)
            {

                reticlePosition = new Vector2(reticlePosition.x + rx * reticleMoveSpeed, reticlePosition.y);

                if (limitReticle)
                {
                    if (!(reticlePosition.y <= 40 && ry > 0) && !(reticlePosition.y > 310 && ry < 0))
                    {
                        reticlePosition = new Vector2(reticlePosition.x, reticlePosition.y - ry * reticleMoveSpeed);
                    }

                }
                else
                {
                    reticlePosition = new Vector2(reticlePosition.x, reticlePosition.y - ry * reticleMoveSpeed);

                }

            }
            else
            {
                angelH += rx * deltaTime * cameraSwingSpeed;
                reticlePosition = new Vector2(reticlePosition.x, reticlePosition.y - ry * reticleMoveSpeed);
                angelV = fixedAngelV;

            }
        }
        else
        {
            if (Time.timeScale != 0)
            {
                angelH += rx * 0.03f * cameraSwingSpeed;
                angelV += ry * 0.03f * cameraSwingSpeed;
            }
            if (isAngelVFixed)
            {
                angelV = fixedAngelV;
            }

            angelV = Mathf.Clamp(angelV, minAngelV, maxAngelV);

        }

        if (player.GetWeapon().Deflection.x == 0 && player.GetWeapon().Deflection.y == 0)
        {
            drx = Mathf.Lerp(drx, player.GetWeapon().Deflection.x, deltaTime * 5.0f);
            dry = Mathf.Lerp(dry, player.GetWeapon().Deflection.y, deltaTime * 5.0f);
        }
        else
        {
            drx = player.GetWeapon().Deflection.x;
            dry = player.GetWeapon().Deflection.y;
        }


        cameraTransform.rotation = Quaternion.Euler(-(angelV + drx), angelH + dry, 0.0f);
        float smooth = 100f;
        if (gameScene.PlayingState == PlayingState.GamePlaying)
        {
            player.GetTransform().rotation = Quaternion.Euler(0.0f, angelH, 0.0f);
            moveTo = player.GetTransform().TransformPoint(cameraDistanceFromPlayer);

            Vector3 dir = moveTo - player.GetTransform().position;
            Ray ray = new Ray(player.GetTransform().position, dir);
            float dis = dir.magnitude;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, dis, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL))
            {
                //moveTo = hit.point - dir * 0.2f + Vector3.up * 1.0f;
                //player.SetTransparent(true);

                GameObject obj = hit.collider.gameObject;
                if (obj.GetComponent<Renderer>() == null)
                {
                    obj = obj.transform.parent.gameObject;
                }
                if (obj.GetComponent<Renderer>() != null)
                {
                    obj.layer = PhysicsLayer.TRANSPARENT_WALL;


                    foreach (Material material in obj.GetComponent<Renderer>().materials)
                    {
                        Texture old = material.GetTexture("_texBase");
                        material.shader = transparentShader;
                        Color color = Color.gray;
                        color.a = 0.1f;
                        material.SetColor("_TintColor", color);
                        material.SetTexture("_MainTex", old);
                    }
                    for (int i = 0; i < 5; i++)
                    {

                        if ((lastTransparentObjList[i] == obj))
                        {
                            break;
                        }
                        else if (lastTransparentObjList[i] == null)
                        {
                            lastTransparentObjList[i] = obj;
                            //Debug.Log(lastTransparentObjList[i].name + " became transparent");
                            break;
                        }

                    }
                }
                //behindWall = true;
                //moveTo = hit.point + dir * 0.2f + new Vector3(0, 1, 0);
                //cameraTransform.LookAt(player.GetTransform());
                //Debug.DrawLine(ray.origin, hit.point);
                //smooth = 0.01f;

                //Debug.Log("Change Shader To Transparent. " + obj.name);

                //obj.renderer.enabled = false;

            }

            else
            {
                //player.SetTransparent(false);
                //Debug.Log("Change Shader To Normal. ");
                for (int i = 0; i < 5; i++)
                {

                    if (lastTransparentObjList[i] != null)
                    {
                        //Debug.Log(lastTransparentObjList[i].name + " became solid");
                        foreach (Material material in lastTransparentObjList[i].GetComponent<Renderer>().materials)
                        {
                            material.shader = solidShader;
                        }
                        //lastTransparentObjList[i].renderer.material.shader = solidShader;
                        lastTransparentObjList[i] = null;

                    }

                }

            }

            //cameraTransform.position = moveTo;// Vector3.Lerp(cameraTransform.position, moveTo, smooth * Time.time);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, moveTo, smooth * Time.deltaTime);

        }
        else if ( gameScene.PlayingState == PlayingState.GameLose)
        {
            minAngelV = -70;
            maxAngelV = 70;

            cameraTransform.position = player.GetTransform().TransformPoint(3 * Mathf.Sin(Time.time * 0.3f), 4.0f, 3 * Mathf.Cos(Time.time * 0.3f));
            cameraTransform.LookAt(player.GetTransform());
        }
        else if(gameScene.PlayingState == PlayingState.GameWin)
        {
            minAngelV = -70;
            maxAngelV = 70;

            if (winTime == -1)
            {
                winTime = Time.time;
            }

            float deltaWinTime = Time.time - winTime;
            cameraTransform.position = player.GetTransform().TransformPoint(3 * Mathf.Sin((deltaWinTime - 1.7f) * 0.3f), 2.0f, 3 * Mathf.Cos((deltaWinTime - 1.7f) * 0.3f));
            cameraTransform.LookAt(player.GetTransform());
        }


        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            deltaTime = 0;
        }

        //lastUpdateTime = Time.time;
    }


    
    void OnGUI()
    {
        if (Time.time != 0 && Time.timeScale != 0)
        {

            if (player != null)
            {

                if (GameApp.GetInstance().GetGameScene().PlayingState == PlayingState.GamePlaying && player.InputController.EnableShootingInput)
                {

                    Weapon weapon = player.GetWeapon();
                    if (weapon != null)
                    {
                        if (weapon.GetWeaponType() == WeaponType.Sniper)
                        {
                            GUI.DrawTexture(new Rect((Sniper.lockAreaRect.xMin - AutoRect.AutoValue(leftTopReticle.width / 2)), (Sniper.lockAreaRect.yMin - AutoRect.AutoValue(leftTopReticle.height / 2)), AutoRect.AutoValue(leftTopReticle.width), AutoRect.AutoValue(leftTopReticle.height)), leftTopReticle);
                            GUI.DrawTexture(new Rect((Sniper.lockAreaRect.xMax - AutoRect.AutoValue(rightTopReticle.width / 2)), (Sniper.lockAreaRect.yMin - AutoRect.AutoValue(rightTopReticle.height / 2)), AutoRect.AutoValue(rightTopReticle.width), AutoRect.AutoValue(rightTopReticle.height)), rightTopReticle);
                            GUI.DrawTexture(new Rect((Sniper.lockAreaRect.xMin - AutoRect.AutoValue(leftBottomReticle.width / 2)), (Sniper.lockAreaRect.yMax - AutoRect.AutoValue(leftBottomReticle.height / 2)), AutoRect.AutoValue(leftBottomReticle.width), AutoRect.AutoValue(leftBottomReticle.height)), leftBottomReticle);
                            GUI.DrawTexture(new Rect((Sniper.lockAreaRect.xMax - AutoRect.AutoValue(rightBottomReticle.width / 2)), (Sniper.lockAreaRect.yMax - AutoRect.AutoValue(rightBottomReticle.height / 2)), AutoRect.AutoValue(rightBottomReticle.width), AutoRect.AutoValue(rightBottomReticle.height)), rightBottomReticle);
                        

                            Sniper sniper = (Sniper)weapon;
                            List<NearestEnemyInfo> neis = sniper.GetNearestEnemyInfoList();
                            foreach (NearestEnemyInfo info in neis)
                            {
                                GUI.DrawTexture(new Rect(info.currentScreenPos.x - AutoRect.AutoValue((reticle.width * 0.5f)), info.currentScreenPos.y - AutoRect.AutoValue((reticle.height * 0.5f)), AutoRect.AutoValue(reticle.width), AutoRect.AutoValue(reticle.height)), reticle);

                            }

                        }
                        else
                        {
                            //Debug.Log(reticlePosition.x);
                            GUI.DrawTexture(new Rect(reticlePosition.x - AutoRect.AutoValue(reticle.width * 0.5f * mutipleSizeReticle), reticlePosition.y - AutoRect.AutoValue((reticle.height * 0.5f * mutipleSizeReticle)), AutoRect.AutoValue(reticle.width * mutipleSizeReticle), AutoRect.AutoValue(reticle.height * mutipleSizeReticle)), reticle);

                        }
                    }
                }
            }
        }



    
    }
    
}

