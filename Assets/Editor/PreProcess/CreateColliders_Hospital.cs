using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Zombie3D
{
    class CreateColliders_Hospital
    {
        [MenuItem("PreProcess/CreateColliders_Hospital")]
        static void Execute()
        {
            GameObject c1 = GameObject.Find("zombie3d_hospital");
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
                        BoxCollider c = null;
                        if (obj.name == "wall_02")
                        {
                            int count = obj.transform.GetChildCount();
                            for (int j = 0; j < count; j++)
                            {
                                GameObject.DestroyImmediate(obj.transform.GetChild(0).gameObject);
                            }

                            GameObject co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f,5.1f,17.93f);
                            c.center = new Vector3(0,0,-6.618f);


                            co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 9.756f);
                            c.center = new Vector3(0, 0, 11.54f);

                            co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 7.50f);
                            c.center = new Vector3(0, 0, 24.88f);

                            co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 7.50f);
                            c.center = new Vector3(0, 0, -24.4f);


                        }
                        else if(obj.name == "wall_03")
                        {

                            int count = obj.transform.GetChildCount();
                            for (int j = 0; j < count; j++)
                            {
                                GameObject.DestroyImmediate(obj.transform.GetChild(0).gameObject);
                            }


                            GameObject co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(10.38f, 5.1f, 1.5f);
                            c.center = new Vector3(-5.58f, 0, 0f);

                            co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(6.21f, 5.1f, 1.5f);
                            c.center = new Vector3(7.56f, 0, 0f);

                        }
                        else if(obj.name == "wall_04")
                        {

                            int count = obj.transform.GetChildCount();
                            for (int j = 0; j < count; j++)
                            {
                                GameObject.DestroyImmediate(obj.transform.GetChild(0).gameObject);
                            }

                            GameObject co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 9.87f);
                            c.center = new Vector3(0, 0, -2.2f);

                        }
                        else if(obj.name == "wall_13")
                        {

                            int count = obj.transform.GetChildCount();
                            for (int j = 0; j < count; j++)
                            {
                                GameObject.DestroyImmediate(obj.transform.GetChild(0).gameObject);
                            }

                            GameObject co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 17.6f);
                            c.center = new Vector3(-0.31f, 0, -4.876f);

                            co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 9.58f);
                            c.center = new Vector3(-0.31f, 0, 13.66f);

                        }
                        else if( obj.name == "wall_24")
                        {

                            int count = obj.transform.GetChildCount();
                            for (int j = 0; j < count; j++)
                            {
                                GameObject.DestroyImmediate(obj.transform.GetChild(0).gameObject);
                            }

                            GameObject co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 7.5f);
                            c.center = new Vector3(0f, 0, 18.6f);

                            co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 18.32f);
                            c.center = new Vector3(0f, 0, 0.62f);

                            co = new GameObject();
                            co.layer = PhysicsLayer.WALL;
                            co.transform.position = obj.transform.position;
                            co.transform.parent = obj.transform;
                            c = co.AddComponent<BoxCollider>();
                            c.size = new Vector3(1.21f, 5.1f, 9.75f);
                            c.center = new Vector3(0f, 0, -18.2f);
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