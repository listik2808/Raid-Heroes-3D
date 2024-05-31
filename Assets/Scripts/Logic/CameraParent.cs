using UnityEngine;

namespace Scripts.Logic
{
    public class CameraParent : MonoBehaviour
    {
        [SerializeField] private HeroView _heroView;
        [SerializeField] private RenderTexture _renderTexture;

        public RenderTexture RenderTexture => _renderTexture;
    }
}