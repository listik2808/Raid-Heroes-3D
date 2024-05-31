namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class SmallSet : ExchangingCrystalsGold
    {
        private void OnEnable()
        {
            _rewardButton.ButtonOnClic += TryExchangeCurrency;
            _buttonOpenCanvas.ButtonOnClic += OpenCanvas;
            _buttonOpenCanvasInfo.ButtonOnClic += OpenCanvas;
            _buttonClose.ButtonOnClic += CloseCanvas;
            ExchangeSetS();
        }

        private void OnDisable()
        {
            _rewardButton.ButtonOnClic -= TryExchangeCurrency;
            _buttonOpenCanvas.ButtonOnClic -= OpenCanvas;
            _buttonOpenCanvasInfo.ButtonOnClic -= OpenCanvas;
            _buttonClose.ButtonOnClic -= CloseCanvas;
        }

        public override void OpenCanvas()
        {
            ExchangeSetS();
            _canvas.gameObject.SetActive(true);
        }
    }
}