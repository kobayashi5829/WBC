using System.Collections.Generic;
using UnityEngine;
using WBC.Engine;

namespace WBC.Management
{
    public class NormalSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _normalNPC; // ノーマルNPCプレハブ
        [SerializeField] private GameObject _npcCloud; // NPC管理オブジェクト
        [Header("Settings")]
        [SerializeField] private int _spawnNum = 1; // スポーン数
        [SerializeField] private List<GameObject> _markerList; // マーカーリスト

        void Start()
        {
            // マーカーから位置リストへ変換
            var waypoints = ConvertMarkersToPositions(_markerList);

            // NPCスポーン
            SpawnNPC(_spawnNum, waypoints);
        }

        /// <summary>
        /// マーカーから位置リストへ変換
        /// </summary>
        /// <returns></returns>
        private List<Vector3> ConvertMarkersToPositions(List<GameObject> markerList)
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (var marker in markerList)
            {
                positions.Add(marker.transform.position);
            }
            return positions;
        }

        /// <summary>
        /// NPCスポーン
        /// </summary>
        /// <param name="num"></param>
        private void SpawnNPC(int num, List<Vector3> waypoints)
        {
            for (int i = 0; i < num; i++)
            {
                float xPos = Random.Range(transform.localScale.x * -5, transform.localScale.x * 5);
                float zPos = Random.Range(transform.localScale.z * -5, transform.localScale.z * 5);
                Vector3 spawnPosition = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z + zPos);
                var npc = Instantiate(_normalNPC, spawnPosition, Quaternion.identity);
                if (npc.TryGetComponent<Activity>(out var activity))
                {
                    activity.waypoints = waypoints;
                }
            }
        }
    }
}
