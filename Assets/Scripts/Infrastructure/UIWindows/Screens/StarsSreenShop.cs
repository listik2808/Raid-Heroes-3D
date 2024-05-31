using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class StarsSreenShop : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _buttonStars;

        public BookmarkButton ButtonStars => _buttonStars;

        private void OnEnable()
        {
            _buttonStars.Button.Select();
        }
    }
}
