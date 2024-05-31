namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class AverageSet : ExchangingCrystalsGold
    {
        private void OnEnable()
        {
            _rewardButton.ButtonOnClic += TryExchangeCurrency;
            _buttonOpenCanvas.ButtonOnClic += OpenCanvas;
            _buttonOpenCanvasInfo.ButtonOnClic += OpenCanvas;
            _buttonClose.ButtonOnClic += CloseCanvas;
            ExchangeSetM();
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
            ExchangeSetM();
            _canvas.gameObject.SetActive(true);
        }
    }
}