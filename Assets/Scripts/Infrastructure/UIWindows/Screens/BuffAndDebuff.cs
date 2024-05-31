using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class BuffAndDebuff : MonoBehaviour
    {
        [SerializeField] private Image _imageStane;
        [SerializeField] private Image _imagePoisoner;
        [SerializeField] private Image _imageDysmoral;
        [SerializeField] private Image _imageMotivation;
        [SerializeField] private Image _imageHupnosis;
        [SerializeField] private Image _imageHealing;
        [SerializeField] private Image _imageFreezing;

        public Image Stane => _imageStane;
        public Image Poisoner => _imagePoisoner;
        public Image Dysmoral => _imageDysmoral;
        public Image Motivation => _imageMotivation;
        public Image Hyposis => _imageHupnosis;
        public Image Healing => _imageHealing;
        public Image Freezing => _imageFreezing;
    }
}
