using Scripts.Infrastructure.UIWindows.ButtonUI;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class CoinsScreenShop : MonoBehaviour
    {
        [SerializeField] private BookmarkButton _bottonCoins;

        public BookmarkButton BottonCoins => _bottonCoins;

        private void OnEnable()
        {
            _bottonCoins.Button.Select();
        }
    }
}
