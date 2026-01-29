using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace WBC.Engine
{
    public class Activity : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        List<Vector3> _waypoints = new List<Vector3>(); // ウェイポイントリスト
        private int _prevWaypointIndex = -1; // 前回のウェイポイントインデックス

        void Start()
        {
            //モック
            _waypoints.Add(new Vector3(-13.9224014f,1.00000048f,-6.20086002f));
            _waypoints.Add(new Vector3(6.68404436f,0.999999404f,-3.28747082f));
            _waypoints.Add(new Vector3(0f,1f,10f));
        }

        /// <summary>
        /// 次のウェイポイントへ移動
        /// </summary>
        /// <returns></returns>
        public bool MoveToNextWaypoint()
        {
            if (_navMeshAgent.isStopped)
            {
                int waypointIndex = Random.Range(0, _waypoints.Count);
                if (waypointIndex == _prevWaypointIndex)
                {
                    waypointIndex = (waypointIndex + 1) % _waypoints.Count;
                }
                _prevWaypointIndex = waypointIndex;

                Vector3 targetPosition = _waypoints[waypointIndex];
                _navMeshAgent.SetDestination(targetPosition);
                _navMeshAgent.isStopped = false;
            }
            else
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 0.1f)
                {
                    _navMeshAgent.isStopped = true;
                    return true;
                }
            }

            return false;
        }
    }
}
