namespace Scripts.Infrastructure.UIWindows.ButtonUI
{
    public class LargeSet : ExchangingCrystalsGold
    {
        private void OnEnable()
        {
            _rewardButton.ButtonOnClic += TryExchangeCurrency;
            _buttonOpenCanvas.ButtonOnClic += OpenCanvas;
            _buttonOpenCanvasInfo.ButtonOnClic += OpenCanvas;
            _buttonClose.ButtonOnClic += CloseCanvas;

            ExchangeSetL();
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
            ExchangeSetL();
            _canvas.gameObject.SetActive(true);
        }
    }
}