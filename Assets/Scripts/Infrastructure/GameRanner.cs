using UnityEngine;
using YG;

namespace Scripts.Infrastructure
{
    public class GameRanner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;
        private void Awake()
        {
            GameBootstrapper bootstrapper = FindObjectOfType<GameBootstrapper>();

            if(bootstrapper == null )
            {
                GameBootstrapper bootstrapperGame = Instantiate(BootstrapperPrefab);
            }
        }
    }
}