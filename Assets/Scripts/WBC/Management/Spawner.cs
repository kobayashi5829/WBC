using UnityEngine;

namespace WBC.Management
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int _spawnNum = 1; // スポーン数

        /// <summary>
        /// NPCスポーン
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public GameObject[] SpawnNpc(GameObject prefab)
        {
            var npcs = new GameObject[_spawnNum];
            for (int i = 0; i < _spawnNum; i++)
            {
                float xPos = Random.Range(transform.localScale.x * -5, transform.localScale.x * 5);
                float zPos = Random.Range(transform.localScale.z * -5, transform.localScale.z * 5);
                Vector3 spawnPosition = new Vector3(transform.position.x + xPos, transform.position.y, transform.position.z + zPos);
                npcs[i] = Instantiate(prefab, spawnPosition, Quaternion.identity);
            }

            return npcs;
        }
    }
}
