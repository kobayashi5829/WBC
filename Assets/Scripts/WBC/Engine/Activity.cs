using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace WBC.Engine
{
    public class Activity : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        public List<Vector3> waypoints = new List<Vector3>(); // ウェイポイントリスト
        private int _prevWaypointIndex = -1; // 前回のウェイポイントインデックス

        /// <summary>
        /// 次のウェイポイントへ移動
        /// </summary>
        /// <returns></returns>
        public void MoveToNextWaypoint()
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
        }

        /// <summary>
        /// 近くのNPCへ移動
        /// </summary>
        /// <param name="target"></param>
        public void MoveToNearNPC(GameObject target)
        {
            _navMeshAgent.SetDestination(target.transform.position);
            _navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// 目的地到着判定
        /// </summary>
        /// <returns></returns>
        public bool IsArrivedPoint(float radius)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + radius)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.isStopped = true;
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// 近くのNPCを検出
        /// </summary>
        /// <returns></returns>
        public Collider[] CheckNearNPC(float radius)
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                radius,
                LayerMask.GetMask("NPC"),
                QueryTriggerInteraction.Ignore
            );

            return hits;
        }
    }
}
