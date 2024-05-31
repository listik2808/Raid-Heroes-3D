using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ResourceSetsShop : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _buttonCard;
        [SerializeField] private TextShading _textShading;
        public BookmarkButton ButtonCard => _buttonCard;

        private void OnEnable()
        {
            _buttonCard.Button.Select();
            _textShading.SetTransparency();
        }
    }
}
