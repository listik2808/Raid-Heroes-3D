using Scripts.Infrastructure.States;
using Scripts.Infrastructure.UIWindows.Screens;
using System.Collections;
using UnityEngine;
using YG;
#if !UNITY_EDITOR
using System.Collections;
#endif

namespace Scripts.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain CurtainPrefab;
        private Game _game;
        private void OnEnable()
        {
            YandexGame.GetDataEvent += GetData;
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= GetData;
        }


        private void Start()
        {
            GetData();
        }

        private void GetData()
        {
            if (YandexGame.SDKEnabled == true)
            {
                _game = new Game(this, CurtainPrefab);
                _game.StateMachine.Enter<BootstrapState>();
                DontDestroyOnLoad(this);
            }
        }

//#if !UNITY_EDITOR
//                //private IEnumerator Start()
//                //{
//                //    yield return YandexGamesSdk.Initialize();
//                //    //GameAnalytics.Initialize();
//                //    _game = new Game(this, CurtainPrefab);
//                //    _game.StateMachine.Enter<BootstrapState>();
//                //    DontDestroyOnLoad(this);
//                //}
//#else

//        private void Start()
//        {
//            _game = new Game(this, CurtainPrefab);
//            _game.StateMachine.Enter<BootstrapState>();
//            DontDestroyOnLoad(this);
//        }
//#endif
    }
}