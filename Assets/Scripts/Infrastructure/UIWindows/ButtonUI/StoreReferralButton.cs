using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class StoreReferralButton : MonoBehaviour
    {
        [SerializeField] private RedirectButtonShop _shop;

        private void OnEnable()
        {
            _shop.Subscription();
        }

        private void OnDisable()
        {
            _shop.Unsubscribe();
        }
    }
}