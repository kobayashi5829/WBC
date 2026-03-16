using System.Collections.Generic;
using UnityEngine;
using WBC.Engine;

namespace WBC.Management
{
    public class NpcCloud : MonoBehaviour
    {
        [SerializeField] private GameObject _normalNPC; // ノーマルNPCプレハブ
        [HideInInspector] public List<Controller> npcList = new List<Controller>(); // 管理するNPCのリスト

        private void Start()
        {
            int npcId = 0;
            var spawners = FindObjectsByType<Spawner>(FindObjectsSortMode.None);
            foreach (var spawner in spawners)
            {
                var npcs = spawner.SpawnNpc(_normalNPC);
                foreach (var npc in npcs)
                {
                    var controller = npc.GetComponent<Controller>();
                    controller.id = npcId++; // NPC識別IDを設定
                    npcList.Add(controller);
                }
            }
        }
    }
}
