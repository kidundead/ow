using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Zombie3D
{
    public class TankGraphPathFinding : IPathFinding
    {

        protected WayPointScript currentWayPoint;
        protected Stack<WayPointScript> openStack = new Stack<WayPointScript>();
        protected Stack<WayPointScript> closeStack = new Stack<WayPointScript>();
        protected Stack<Transform> path;

        public Stack<Transform> FindPath(Vector3 enemyPos, Vector3 playerPos)
        {
            GameObject[] points = GameObject.FindGameObjectsWithTag(TagName.WAYPOINT);
            float minDisSqrEnemy = 99999.0f;
            float minDisSqrPlayer = 99999.0f;
            WayPointScript from = null;
            WayPointScript to = null;

            foreach (GameObject wObj in points)
            {
                WayPointScript w = wObj.GetComponent<WayPointScript>();
                w.parent = null;
                float disSqrEnemy = (w.transform.position - enemyPos).magnitude;
                if (disSqrEnemy < minDisSqrEnemy)
                {
                    Ray ray = new Ray(enemyPos + new Vector3(0, 0.5f, 0), w.transform.position - enemyPos);

                    RaycastHit hit;
                    if (!Physics.Raycast(ray, out hit, disSqrEnemy, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR))
                    {
                        from = w;
                        minDisSqrEnemy = disSqrEnemy;
                    }


                }

                to = GameApp.GetInstance().GetGameScene().GetPlayer().NearestWayPoint;

            }
            if (from != null && to != null)
            {
                path = SearchPath(from, to);
            }
            if (to == null)
            {
                Debug.Log("to null");
            }
            return path;
        }

        public Transform GetNextWayPoint(Vector3 enemyPos, Vector3 playerPos)
        {
            if (path != null && path.Count > 0)
            {
                return path.Peek();
            }
            else
            {
                path = FindPath(enemyPos, playerPos);
                if (path != null && path.Count > 0)
                {
                    return path.Peek();
                }
                else
                {
                    return null;
                }

            }
        }

        public void PopNode()
        {
            if (path != null && path.Count > 0)
            {
                path.Pop();
            }
        }

        public bool HavePath()
        {
            if (path != null && path.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearPath()
        {
            if (path != null)
            {
                path.Clear();
            }
        }

        public WayPointScript GetWayPointByName(string name)
        {
                GameObject[] points = GameObject.FindGameObjectsWithTag(TagName.WAYPOINT);
                foreach(GameObject obj in points)
                {
                    if (obj.name == name)
                    {
                        return obj.GetComponent<WayPointScript>();
                    }
                }
                return null;
        }

        public Stack<Transform> SearchPath(WayPointScript from, WayPointScript to)
        {
            Stack<Transform> path = new Stack<Transform>();
            if (to.name == "WayPoint7")
            {
                Debug.Log("To small house");
                WayPointScript wayPoint6 = GetWayPointByName("WayPoint6");
                to = wayPoint6;
            }
            if (from == to)
            {
                path.Push(to.transform);
                return path;
            }


            //Debug.Log("search Path: from " + from.transform.name + " to " + to.transform.name);
            openStack.Push(from);

            while (openStack.Count > 0)
            {
                if (openStack.Count > 100)
                {
                    Debug.Log("Memeroy Explode! To many nodes in open stack..");
                    Debug.Break();
                    break;
                }
                WayPointScript currentWayPoint = openStack.Pop();
                closeStack.Push(currentWayPoint);
                WayPointScript[] nodes = currentWayPoint.nodes;
                foreach (WayPointScript w in nodes)
                {
                    if (w == to)
                    {
                        w.parent = currentWayPoint;
                        break;
                    }
                    if (!openStack.Contains(w) && !closeStack.Contains(w))
                    {
                        w.parent = currentWayPoint;
                        openStack.Push(w);
                    }
                }
            }
            openStack.Clear();
            closeStack.Clear();


            WayPointScript wayPoint = to;
            path.Push(to.transform);
            //Debug.Log("Find Path " + to.transform.name);
            while (wayPoint.parent != null)
            {
                if (wayPoint.parent.name == "WayPoint7")
                {
                    //WayPointScript wayPoint6 = GetWayPointByName("WayPoint6");
                    wayPoint.parent = null;
                    break;
 
                }

                wayPoint = wayPoint.parent;
                //Debug.Log("-> " + wayPoint.transform.name);
                if (path.Count > 30)
                {
                    Debug.Log("Memeroy Explode! Parent Forever..");
                    Debug.Break();
                    break;
                }
                path.Push(wayPoint.transform);
            }


            return path;
        }


    }

}
