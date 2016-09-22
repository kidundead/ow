using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Zombie3D
{
    public interface IPathFinding
    {
        Stack<Transform> FindPath(Vector3 enemyPos, Vector3 playerPos);
        Transform GetNextWayPoint(Vector3 enemyPos, Vector3 playerPos);
        void ClearPath();
        bool HavePath();
        void PopNode();
    }
}
