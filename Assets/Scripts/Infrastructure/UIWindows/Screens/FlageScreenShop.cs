using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class FlageScreenShop : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _buttonFlage;

        public BookmarkButton ButtonFlage => _buttonFlage;

        private void OnEnable()
        {
            _buttonFlage.Button.Select();
        }
    }
}
