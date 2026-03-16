using UnityEngine;

namespace WBC.Engine
{
    public class Controller : MonoBehaviour
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
        [Header("Statet Provabilities")]
        [SerializeField] private float _provWalk = 0.1f; // 歩行状態への遷移確率割合
        [SerializeField] private float _provSearch = 0.1f; // 探索状態への遷移確率割合
        [Header("Debug")]
        [SerializeField] private Renderer _debugCube;
        [HideInInspector] public int id;// NPC識別ID
        public InteractionLock interactionLock = new InteractionLock();
        private State _state = State.Idle; // 現在の状態
        private float _timerCounter = 0f; // 時間計測用カウンター
        private Action[] _actions; // アクションコンポーネントの配列
        private int _actionIndex = 0; // 現在のアクションインデックス

        private void Start()
        {
            _actions = GetComponents<Action>();
            _actionIndex = Random.Range(0, _actions.Length);
        }

        private void Update()
        {
            //行動を実行
            if (_actions.Length != 0)
            {
                // 現在のアクションを実行し、完了したら次のアクションをランダムに選択
                if (_actions[_actionIndex].Act() is true)
                {
                    _actionIndex = Random.Range(0, _actions.Length);
                }
            }
        }

        /// <summary>
        /// 状態制御
        /// </summary>
        /// <param name="state"></param>
        private void Control(State state)
        {
            switch (state)
            {
                /*case State.Idle: // 待機状態
                    _debugCube.material.color = Color.green;
                    if (Timer(_idleDuration))
                    {
                        _state = JudgeNextState();
                    }
                    break;
                case State.Walk_Start: // 歩行準備状態
                    _activity.MoveToNextWaypoint();
                    _state = State.Walk_WalkToWaypoint;
                    break;
                case State.Walk_WalkToWaypoint: // 歩行状態
                    _debugCube.material.color = Color.blue;
                    if (_activity.IsArrivedPoint(_targetRadius)) { _state = State.Idle; }
                    break;
                case State.Search_Searching: // 探索状態
                    var hits = _activity.CheckNearNPC(_npcDetectRadius);
                    NormalController target = GetInteractionTarget(hits);
                    if (target != null) // 近くのNPCを発見
                    {
                        interactionLock.npc = true; // インタラクションロックを設定
                        _activity.MoveToNearNPC(target.gameObject); // 近くのNPCへ移動
                        _state = State.Search_WalkToNPC;
                    }
                    else { _state = State.Idle; } // 発見できなかった場合は待機状態に戻る
                    break;
                case State.Search_WalkToNPC: // 近くのNPCへ移動
                    _debugCube.material.color = Color.yellow;
                    if (_activity.IsArrivedPoint(_targetRadius)) { _state = State.Debug; }
                    break;
                case State.Debug: // デバッグ状態
                    _debugCube.material.color = Color.gray;
                    Debug.Log($"{_state} is Debug State.");
                    break;
                default:
                    break;*/
            }
        }

        /// <summary>
        /// インタラクション対象取得
        /// </summary>
        /// <param name="hits"></param>
        /// <returns></returns>
        /*public NormalController GetInteractionTarget(Collider[] hits)
        {
            foreach (var hit in hits)
            {
                if (hit.gameObject.TryGetComponent<NormalController>(out var npcController))
                {
                    if (!npcController.interactionLock.npc) { return npcController; }
                }
            }
            return null;
        }*/

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
        /// 次の状態判定
        /// </summary>
        /// <returns></returns>
        private State JudgeNextState()
        {
            float prov = Random.Range(0f, 1f);
            if (prov < _provWalk) { return State.Walk_Start; }
            else if (prov < _provSearch) { return State.Search_Searching; }
            else { return State.Idle; }
        }
    }
}
