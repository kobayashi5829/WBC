using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace WBC.Engine
{
    public class Activity : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [Header("Settings")]
        [SerializeField] private float _waypointsRadius = 1f; // ウェイポイントの半径
        [SerializeField] private float _npcDetectRadius = 1f; // NPC検出半径
        public List<Vector3> waypoints = new List<Vector3>(); // ウェイポイントリスト
        private int _prevWaypointIndex = -1; // 前回のウェイポイントインデックス

        /// <summary>
        /// 次のウェイポイントへ移動
        /// </summary>
        /// <returns></returns>
        public bool MoveToNextWaypoint()
        {
            if (_navMeshAgent.isStopped)
            {
                int waypointIndex = Random.Range(0, waypoints.Count);
                if (waypointIndex == _prevWaypointIndex)
                {
                    waypointIndex = (waypointIndex + 1) % waypoints.Count;
                }
                _prevWaypointIndex = waypointIndex;

                Vector3 targetPosition = waypoints[waypointIndex];
                _navMeshAgent.SetDestination(targetPosition);
                _navMeshAgent.isStopped = false;
            }
            else
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + _waypointsRadius)
                {
                    _navMeshAgent.isStopped = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 近くのNPCを検出
        /// </summary>
        /// <returns></returns>
        public Collider[] CheckNearNPC()
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                _npcDetectRadius,
                LayerMask.GetMask("NPC"),
                QueryTriggerInteraction.Ignore
            );

            return hits;
        }
    }
}
