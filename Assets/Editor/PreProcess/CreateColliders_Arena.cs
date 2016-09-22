using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Zombie3D
{
    class CreateColliders_Arena
    {
        [MenuItem("PreProcess/CreateColliders_Arena")]
        static void Execute()
        {
            GameObject c1 = GameObject.Find("arena");
            if (c1 != null)
            {
                for (int i = 0; i < c1.transform.GetChildCount(); i++)
                {
                    GameObject obj = c1.transform.GetChild(i).gameObject;
                    obj.layer = PhysicsLayer.Default;
                    if (obj.GetComponent<Collider>() != null)
                    {
                        GameObject.DestroyImmediate(obj.GetComponent<Collider>());
                    }


                    if (!obj.name.StartsWith("lamp_") && !obj.name.StartsWith("tree") && !obj.name.StartsWith("sky"))
                    {

                        if (obj.name == "house_01"
                        || obj.name == "house_03"
                        || obj.name == "house_08"
                        || obj.name == "house_10"
                        || obj.name == "house_07"
                        || obj.name == "house_02"
                        || obj.name == "door_05"
                        || obj.name == "door_03"
                            || obj.name.StartsWith("stair_")
                        )
                        {
                            obj.AddComponent<MeshCollider>();
                        }
                        else
                        {
                            obj.AddComponent<BoxCollider>();
                        }

                        obj.layer = PhysicsLayer.WALL;
                    }
                    if (obj.name.StartsWith("floor_"))
                    {
                        obj.layer = PhysicsLayer.FLOOR;
                    }
                    
                    /*
                    if (!obj.name.Contains("sky") && !obj.name.Contains("lamp") && !obj.name.Contains("light") && !obj.name.Contains("rail") && !obj.name.Contains("tree"))
                    {
                        BoxCollider bc = null;

                        if (obj.collider == null)
                        {
                            bc = obj.AddComponent<BoxCollider>();
                            //bc.center = Vector3.zero;
                        }
                        else
                        {
                            bc = obj.collider as BoxCollider;
                        }
                        obj.layer = PhysicsLayer.WALL;


                       
                        if (obj.layer == PhysicsLayer.WALL)
                        {
                            // bc.size = new Vector3(bc.size.x, bc.size.y, bc.size.z * 3.0f);
                        }
                    }
                    **/
                }
            }

        }


    }

}