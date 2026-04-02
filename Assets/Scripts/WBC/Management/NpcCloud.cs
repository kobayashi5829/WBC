using System.Collections.Generic;
using UnityEngine;
using WBC.Engine;

namespace WBC.Management
{
    public class NpcCloud : MonoBehaviour
    {
        [SerializeField] private GameObject _normalNPC; // ノーマルNPCプレハブ
        [SerializeField] private List<GameObject> _markerList; // マーカーリスト
        public static NpcCloud Instance { get; private set; }
        [HideInInspector] public Vector3[] markerPositions; // マーカー位置リスト
        [HideInInspector] public Controller[] npcList; // 管理するNPCのリスト

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //マーカーから位置リストへ変換
            if (_markerList.Count > 0)
            {
                markerPositions = _markerList.ConvertAll(marker => marker.transform.position).ToArray();
            }

            //NPCスポーン
            int npcId = 0;
            var spawners = FindObjectsByType<Spawner>(FindObjectsSortMode.None);
            List<Controller> tmpNpcList = new List<Controller>();
            foreach (var spawner in spawners)
            {
                var npcs = spawner.SpawnNpc(_normalNPC);
                foreach (var npc in npcs)
                {
                    var controller = npc.GetComponent<Controller>();
                    controller.id = npcId++; // NPC識別IDを設定
                    tmpNpcList.Add(controller);
                }
            }
            if (tmpNpcList.Count > 0)
            {
                npcList = tmpNpcList.ToArray();
            }
        }
    }
}
