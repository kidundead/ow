using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Zombie3D
{
    class CreateColliders_Village
    {
        [MenuItem("PreProcess/CreateColliders_Village")]
        static void Execute()
        {
            GameObject c1 = GameObject.Find("village");
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

                    if (!obj.name.Contains("sky") && !obj.name.Contains("lamp") && !obj.name.Contains("light") && !obj.name.Contains("rail") && !obj.name.Contains("tree") && !obj.name.Contains("arbour"))
                    {
                        BoxCollider bc = null;

                        if (obj.GetComponent<Collider>() == null)
                        {
                            bc = obj.AddComponent<BoxCollider>();
                            bc.center = Vector3.zero;
                        }
                        else
                        {
                            bc = obj.GetComponent<Collider>() as BoxCollider;
                        }
                        obj.layer = PhysicsLayer.WALL;


                        if (obj.name.Equals("floor_03"))
                        {
                            obj.layer = PhysicsLayer.FLOOR;
                        }

                        if (obj.layer == PhysicsLayer.WALL)
                        {
                            // bc.size = new Vector3(bc.size.x, bc.size.y, bc.size.z * 3.0f);
                        }
                    }
                    if (obj.name.Contains("rail") || obj.name.Contains("arbour"))
                    {
                        obj.AddComponent<MeshCollider>();
                        obj.layer = PhysicsLayer.WALL;
                        if (obj.name.Contains("rail"))
                        {
                            obj.layer = PhysicsLayer.FLOOR;
                        }
                    }

                    if (obj.name.Contains("door"))
                    {
                        obj.layer = PhysicsLayer.FLOOR;
                    }

                }
            }

        }


    }

}