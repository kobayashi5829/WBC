using UnityEngine;
using UnityEngine.AI;
using WBC.Management;

namespace WBC.Engine
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Walk : Action
    {
        [SerializeField] private float _insideUnitRadius = 1f; // ユニット内半径
        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override bool Act()
        {
            if (_navMeshAgent.hasPath) // 目的地に向かって移動中
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    _navMeshAgent.ResetPath();
                    return true; // 行動完了
                }
            }
            else // 目的地を設定
            {
                if (NpcCloud.Instance.markerPositions.Length == 0) // マーカーが存在しない場合はランダム行動
                {
                    Vector3 randamPosition = transform.position + Random.insideUnitSphere * _insideUnitRadius;
                    if (NavMesh.SamplePosition(randamPosition, out NavMeshHit hit, _insideUnitRadius, NavMesh.AllAreas))
                    {
                        _navMeshAgent.SetDestination(hit.position);
                    }
                    else
                    {
                        Debug.LogWarning("目的地の設定に失敗しました。");
                        return true; // エラーにより行動終了
                    }
                }
            }

            return false; // 行動継続
        }
    }
}
