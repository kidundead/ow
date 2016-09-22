using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Zombie3D
{
    class CreateColliders_ParkingLot
    {
        [MenuItem("PreProcess/CreateColliders_ParkingLot")]
        static void Execute()
        {
            GameObject c1 = GameObject.Find("stopping_place");
            if (c1 != null)
            {
                for (int i = 0; i < c1.transform.GetChildCount(); i++)
                {

                    GameObject obj = c1.transform.GetChild(i).gameObject;
                    Object.DestroyImmediate(obj.GetComponent<Collider>());

                    if (
                        (obj.name.StartsWith("car_")
                        || obj.name.StartsWith("pillar_")
                        || obj.name.StartsWith("rail_")
                        || obj.name.StartsWith("lift")
                        )
                        && !obj.name.StartsWith("pillar_25")
                        )
                    {
                        obj.AddComponent<BoxCollider>();
                    }
                    else if(
                        obj.name.StartsWith("fire_ex")
                        || obj.name.StartsWith("ground")
                        || obj.name.StartsWith("sky")
                        )
                    {

                    }
                    else
                    {
                        obj.AddComponent<MeshCollider>();
                    }
                    if (obj.name.Contains("floor"))
                    {
                        obj.layer = PhysicsLayer.FLOOR;
                    }
                    else
                    {
                        obj.layer = PhysicsLayer.WALL;
                    }

                    /*
                    if (!obj.name.Contains("sky") && !obj.name.Contains("lamp") && !obj.name.Contains("light") && !obj.name.Contains("rail") && !obj.name.Contains("tree"))
                    {
                        BoxCollider bc = null;
                        if (!obj.name.StartsWith("floor") && !obj.name.StartsWith("stairs") && !obj.name.StartsWith("wall_") && !obj.name.StartsWith("beam_"))
                        {
                            if (obj.collider == null)
                            {
                                bc = obj.AddComponent<BoxCollider>();
                                //bc.center = Vector3.zero;
                            }
                            else
                            {
                                bc = obj.collider as BoxCollider;
                            }
                            if (obj.name.StartsWith("stair"))
                            {
                                bc.size = new Vector3(bc.size.x, bc.size.y, bc.size.z * 0.2f);
                            }

                            obj.layer = PhysicsLayer.WALL;
                        }

                        if (obj.name.StartsWith("ash-bin")
                            || obj.name.StartsWith("billboard")
                            || obj.name.StartsWith("car_")
                            || obj.name.StartsWith("foodstuff")
                            || obj.name.StartsWith("fire_hydrant")
                            || obj.name.StartsWith("guidepost")
                            || obj.name.StartsWith("lamp")
                            || obj.name.StartsWith("light")
                            || obj.name.StartsWith("telephone")
                        )
                        {
                            //Rigidbody body = obj.AddComponent<Rigidbody>();
                            //body.mass = 1000;
                            obj.layer = PhysicsLayer.SCENE_OBJECT;
                        }

                        if (obj.name.StartsWith("floor") || obj.name.StartsWith("stairs") || obj.name.StartsWith("wall_"))
                        {
                            obj.layer = PhysicsLayer.FLOOR;
                            obj.AddComponent<MeshCollider>();

                        }

                        if (obj.layer == PhysicsLayer.WALL)
                        {
                            // bc.size = new Vector3(bc.size.x, bc.size.y, bc.size.z * 3.0f);
                        }
                    }
                    */


                }
            }

        }


    }

}