using UnityEngine;

namespace Scripts.Logic
{
    public class CameraParentEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyView _enemuView;
        [SerializeField] private RenderTexture _renderTexture;

        public RenderTexture RenderTexture => _renderTexture;
        public EnemyView EnemyView => _enemuView;
    }
}