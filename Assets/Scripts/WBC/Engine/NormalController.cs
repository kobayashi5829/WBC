using UnityEngine;

namespace WBC.Engine
{
    [RequireComponent(typeof(Activity))]
    public class NormalController : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Walk,
            Search,
        }

        [Header("WBC Engine")]
        [SerializeField] private Conversation _conversation;
        [SerializeField] private Activity _activity;
        [Header("Settings")]
        [SerializeField] private float _idleDuration = 1f; // 待機時間
        public bool isInteractionLock { get; private set; } = false; // インタラクションロック状態
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
                        _state = State.Walk;
                    }
                    break;
                case State.Walk: // 歩行状態
                    if (_activity.MoveToNextWaypoint())
                    {
                        _state = State.Idle;
                    }
                    break;
                case State.Search: // 探索状態
                    var hits = _activity.CheckNearNPC();
                    NormalController target = GetInteractionTarget(hits);
                    if (target != null) isInteractionLock = true;          
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
                    if (!npcController.isInteractionLock) { return npcController; }
                }
            }
            return null;
        }
    }
}
