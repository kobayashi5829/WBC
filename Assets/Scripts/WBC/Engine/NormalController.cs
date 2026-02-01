using UnityEngine;

namespace WBC.Engine
{
    [RequireComponent(typeof(Activity))]
    public class NormalController : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Walk_Start,
            Walk_WalkToWaypoint,
            Search_Searching,
            Search_WalkToNPC,
            Debug,
        }

        /// <summary>
        /// インタラクションロック
        /// </summary>
        public class InteractionLock
        {
            public bool player = false;
            public bool npc = false;
        }

        [Header("WBC Engine")]
        [SerializeField] private Conversation _conversation;
        [SerializeField] private Activity _activity;
        [Header("Settings")]
        [SerializeField] private float _idleDuration = 1f; // 待機時間
        [SerializeField] private float _targetRadius = 1f; // ウェイポイントの半径
        [SerializeField] private float _npcDetectRadius = 1f; // NPC検出半径
        public InteractionLock interactionLock = new InteractionLock();
        private State _state = State.Idle; // 現在の状態
        private float _timerCounter = 0f; // 時間計測用カウンター

        void Update() { Control(_state); }

        /// <summary>
        /// 状態制御
        /// </summary>
        /// <param name="state"></param>
        private void Control(State state)
        {
            switch (state)
            {
                case State.Idle: // 待機状態
                    if (Timer(_idleDuration))
                    {
                        _state = State.Walk_Start;
                    }
                    break;
                case State.Walk_Start: // 歩行準備状態
                    _activity.MoveToNextWaypoint();
                    _state = State.Walk_WalkToWaypoint;
                    break;
                case State.Walk_WalkToWaypoint: // 歩行状態
                    if (_activity.IsArrivedPoint(_targetRadius)) { _state = State.Idle; }
                    break;
                case State.Search_Searching: // 探索状態
                    var hits = _activity.CheckNearNPC(_npcDetectRadius);
                    NormalController target = GetInteractionTarget(hits);
                    if (target != null) // 近くのNPCを発見
                    {
                        interactionLock.npc = false; // インタラクションロックを設定
                        _activity.MoveToNearNPC(target.gameObject); // 近くのNPCへ移動
                        _state = State.Search_WalkToNPC;
                    }
                    break;
                case State.Search_WalkToNPC: // 近くのNPCへ移動
                    if (_activity.IsArrivedPoint(_targetRadius)) { _state = State.Debug; }
                    break;
                case State.Debug: // デバッグ状態
                    Debug.Log($"{gameObject.name} is Debug State.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 時間計測
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private bool Timer(float duration)
        {
            _timerCounter += Time.deltaTime;
            if (_timerCounter >= duration)
            {
                _timerCounter = 0f;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// インタラクション対象取得
        /// </summary>
        /// <param name="hits"></param>
        /// <returns></returns>
        public NormalController GetInteractionTarget(Collider[] hits)
        {
            foreach (var hit in hits)
            {
                if (hit.gameObject.TryGetComponent<NormalController>(out var npcController))
                {
                    if (!npcController.interactionLock.npc) { return npcController; }
                }
            }
            return null;
        }
    }
}
