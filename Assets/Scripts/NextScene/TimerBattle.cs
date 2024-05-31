using Scripts.BattleLogic.GameResult;
using TMPro;
using UnityEngine;

namespace Scripts.NextScene
{
    public class TimerBattle : MonoBehaviour
    {
        public float Timer;
        private int _minute;
        private int _second;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private GameResultsWatcher _watcher;
        private bool _finish = false;

        private void OnEnable ()
        {
            _textTimer.gameObject.SetActive(true);
            _textTimer.text = Timer.ToString();
        }

        private void OnDisable()
        {
            _textTimer.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            BattleTime();
        }

        private void BattleTime()
        {
            if (Timer <= 0 && _finish == false)
            {
                _textTimer.gameObject.SetActive(false);
                _finish = true;
                _watcher.GameOver();
            }
            else if (Timer > 0 && _finish == false)
            {
                Timer -= 1 * Time.deltaTime;
                _minute = Mathf.FloorToInt(Timer / 60);
                _second = Mathf.FloorToInt(Timer - _minute * 60);
                _textTimer.text = _minute.ToString() + ":" + _second.ToString();
            }
        }
    }
}
