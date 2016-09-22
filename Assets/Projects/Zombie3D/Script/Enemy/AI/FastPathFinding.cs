using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Zombie3D
{
    public class FastPathFinding : IPathFinding
    {
        public Stack<Transform> FindPath(Vector3 enemyPos, Vector3 playerPos)
        {

            //Path-finding AI

            /* Algorithem:   
             *      Make a route around the whole scene, the route is composed by a list of path points. 
             * Make sure 1. There is at least one point that could see player directly, no matter where player is.
             *           2. There is at least one point that could be walked to from any enemy, no matter where enemy is.
             *           3. Neighbor points could walk from one to another, without any obstacle.
             * 
             * Whenever the enemy could see player directly, run towards player. 
             * If the player is lost, go find the nearest path point, and walk along the path circle.
             * Choose the point which has the minimum distance between player and enemy as next point. 
             * 
             * 
             */
            /*
            public virtual void PathFinding()
            {
                Vector3 tpoint = target.position;
                //tpoint.y = enemyTransform.position.y;
                //enemyTransform.LookAt(tpoint);
                //enemyTransform.Translate(Vector3.forward * runSpeed * deltaTime);


 

                if (Time.time - lastPathFindingTime > 1.0f)
                {

                    lastPathFindingTime = Time.time;

                    tpoint.y = enemyTransform.position.y;

                    //1.Make player as lastTarget at first time
                    if (lastTarget == Vector3.zero)
                    {
                        lastTarget = target.position;
                    }


                    //2.Check if the player is behind the wall.
                    //  If the player is not reachable, follow the path.

                    //RaycastHit hit;

                    ray = new Ray(enemyTransform.position + new Vector3(0, 0.5f, 0), tpoint - (enemyTransform.position + new Vector3(0, 0.0f, 0)));

                    if (Physics.Raycast(ray, out rayhit, 100, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.SCENE_OBJECT | 1 << PhysicsLayer.PLAYER))
                    {
                        if (rayhit.collider.gameObject.name == "Player")
                        {
                            lastTarget = tpoint;
                            nextPoint = -1;
                        }
                        else
                        {

                            float minDis = 999999;


                            //No next path point set yet(could see player before)
                            if (nextPoint == -1)
                            {
                                //Find a nearest point that could be reached
                                for (int i = 0; i < path.Length; i++)
                                {

                                    float dis = (path[i] - enemyTransform.position).magnitude;
                                    if (dis < minDis)
                                    {
                                        Ray sray = new Ray(enemyTransform.position + new Vector3(0, 0.8f, 0), path[i] - enemyTransform.position);
                                        RaycastHit hit;
                                        if (!Physics.Raycast(sray, out hit, dis, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL))
                                        {
                                            lastTarget = path[i];
                                            minDis = dis;
                                            nextPoint = i;

                                        }
                                    }


                                }
                                //Debug.Log("Can't see Player, Find New Point");
                            }
                            else
                            {
                                //already on the path, reached on point, then compare previous with next point
                                if ((enemyTransform.position - lastTarget).sqrMagnitude < 1 * 1)
                                {
                                    int previous = nextPoint - 1;
                                    int next = nextPoint + 1;

                                    if (next == path.Length)
                                    {
                                        next = 0;
                                    }

                                    if (previous == -1)
                                    {
                                        previous = path.Length - 1;
                                    }


                                    //distance comparision
                                    if (((path[next] - player.GetTransform().position).magnitude + (path[next] - enemyTransform.position).magnitude) < ((path[previous] - player.GetTransform().position).magnitude + (path[previous] - enemyTransform.position).magnitude))
                                    {
                                        nextPoint = next;
                                        //Debug.Log("Next Point");
                                    }
                                    else
                                    {
                                        nextPoint = previous;
                                        //Debug.Log("Previous Point");
                                    }

                                    lastTarget = path[nextPoint];

                                }
                            }


                        }
                    }
                    else
                    {
                        //Debug.Log("nothing");
                    }
                }
                //100.Move towards lastTarget.
                lastTarget.y = enemyTransform.position.y;
                //targetObj.transform.position = lastTarget + new Vector3(0,2,0);
                enemyTransform.LookAt(lastTarget);
                //enemyTransform.Translate(Vector3.forward * runSpeed * deltaTime);
            }
            */

            return null;
        }

        public Transform GetNextWayPoint(Vector3 enemyPos, Vector3 playerPos)
        {
            return null;
        }

        public void ClearPath()
        {
        }

        public bool HavePath()
        {
            return false;
        }
        public void PopNode()
        {
        }
    }

}
        