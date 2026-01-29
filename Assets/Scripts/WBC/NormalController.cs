using UnityEngine;

namespace WBC
{
    public class NormalController : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Walking,
        }

        [Header("WBC Engine")]
        [SerializeField] private Engine.Conversation _conversation;
        [SerializeField] private Engine.Activity _activity;
        [Header("Settings")]
        [SerializeField] private float _idleDuration = 1f; // 待機時間
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
                        _state = State.Walking;
                    }
                    break;
                case State.Walking: // 歩行状態
                    if (_activity.MoveToNextWaypoint())
                    {
                        _state = State.Idle;
                    }
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
    }
}
