using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class CrystalScreenShop : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _buttonCrystal;

        public BookmarkButton ButtonCrystal => _buttonCrystal;

        private void OnEnable()
        {
            _buttonCrystal.Button.Select();
        }
    }
}
