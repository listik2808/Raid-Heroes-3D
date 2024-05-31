using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.NextScene;
using Scripts.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Logic.CastleConstruction
{
    public class BuildingWindow : MonoBehaviour
    {
        public const string TextGold = "Производит золото";
        public const string TextCrystals = "Производит кристалы";
        public const string TextStars = "Дает доступ к сражениям с другими игроками и производит звезды";
        public const string TextRegularCard = "Производит карты простых героев";
        public const string TextRareCard = "Производит карты редких героев";
        public const string TextEpicCard = "Производит карты эпических геров";
        public const string TextPortals = "Производит порталы";
        public const string TextGemsGreen = "Производит зеленые самоцветы и позволяет перековать артефакты";
        public const string TextPalace = "Дает доступ к кланам. Производит золото.";
        public const string TextDescriptionGold = "Прибыль и обьем накоплений зависит\r\nот уровня здания.\r\nСбрасываеться при новом рейде.";
        public const string TextDescriptionCard = "Максимальное количество\r\nнесобранных карт героев зависит \r\nот уровня здания";
        public const string TextDesciptionStars = "Прибыль и обьем накопления зависит от лиги на \r\nарене и уровня здания.";
        public const string TextDescriptionCrystals = "Прибыль и обьем накоплений зависит от уровня\r\nздания.";
        public const string TextDescriptionPortal = "Максимальное количество несобранных порталов\r\nзависит от уровня здания.";

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _textTypeCreate;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private GameObject _upgradeBaner;
        [SerializeField] private BookmarkButton _buttonClose;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _curentGold;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _reward;
        [SerializeField] private BookmarkButton _buttonUpLevel;
        [SerializeField] private TMP_Text _costUpgrade;
        [SerializeField] private GameObject _baner;
        [SerializeField] private TMP_Text _textCountBattle;
        [SerializeField] private BookmarkButton _payButton;
        [SerializeField] private TMP_Text _textPrice;
        [SerializeField] private BookmarkButton _leftButton;
        [SerializeField] private BookmarkButton _rightButton;
        [SerializeField] private Image _infoPay;
        [SerializeField] private Image _impruvmentInfo;
        private ConstructionCastle _constructionCastle;
        private CastleScene _castleScene;

        private void OnEnable()
        {
            _buttonClose.ButtonOnClic += Close;
            _leftButton.ButtonOnClic += TurningPagesLeft;
            _rightButton.ButtonOnClic += TurningPagesRight;
        }

        private void OnDisable()
        {
            _buttonClose.ButtonOnClic -= Close;
            _buttonUpLevel.ButtonOnClic -= UpLevel;
            _payButton.ButtonOnClic -= OpenCastle;
            _leftButton.ButtonOnClic -= TurningPagesLeft;
            _rightButton.ButtonOnClic -= TurningPagesRight;
        }

        public void SetComponent(ConstructionCastle constructionCastle, CastleScene castleScene = null)
        {
            if(castleScene != null)
                _castleScene = castleScene;
            _constructionCastle = constructionCastle;
            _name.text = constructionCastle.Name.text;
            _textTypeCreate.text = GetTypeBuiding(constructionCastle);
            _constructionCastle.PersistenProgressService.Progress.Wallet.Coins.Count = Mathf.Round(_constructionCastle.PersistenProgressService.Progress.Wallet.Coins.Count);
            string result = AbbreviationsNumbers.ShortNumber(_constructionCastle.PersistenProgressService.Progress.Wallet.Coins.Count);
            _curentGold.text = result;
            SetDescription();

            if (constructionCastle.IsOpen)
            {
                _upgradeBaner.SetActive(true);
                SetValuesFields(constructionCastle);
                _baner.SetActive(false);
                _payButton.gameObject.SetActive(false);
                _infoPay.gameObject.SetActive(false);
            }
            else
            {
                _upgradeBaner.SetActive(false);
                if (_constructionCastle.NumberBattlesFought > _constructionCastle.CurrentBattles || _constructionCastle.NumberBattlesFought > _constructionCastle.PersistenProgressService.Progress.OptionData.WonRaids)
                {
                    _payButton.gameObject.SetActive(false);
                    int value = _constructionCastle.NumberBattlesFought - _constructionCastle.PersistenProgressService.Progress.OptionData.WonRaids;
                    _textCountBattle.text = "Одержать еще " + value +" " +"побед в рейдах";
                    _baner.SetActive(true);
                }
                else
                {
                    if (_constructionCastle.Castle != null)
                    {
                        if (_constructionCastle.Castle.IsOpen)
                        {
                            _baner.SetActive(false);
                            _payButton.gameObject.SetActive(true);
                            if(constructionCastle.PersistenProgressService.Progress.Wallet.Coins.Count > _constructionCastle.CostConstruction)
                            {
                                _infoPay.gameObject.SetActive(true);
                            }
                            string result1 = AbbreviationsNumbers.ShortNumber(_constructionCastle.CostConstruction);
                            _textPrice.text = result1;
                            _payButton.ButtonOnClic += OpenCastle;
                        }
                        else
                        {
                            _baner.SetActive(true);
                            _payButton.gameObject.SetActive(false);
                            _infoPay.gameObject.SetActive(false);
                            _textCountBattle.text = "Для открытия покупки требуеться купить " + _constructionCastle.Castle.Name.text;
                        }
                    }
                    else if(_constructionCastle.Castle == null)
                    {
                        _baner.gameObject.SetActive(false);
                        _payButton.gameObject.SetActive(true);
                        if (constructionCastle.PersistenProgressService.Progress.Wallet.Coins.Count > _constructionCastle.CostConstruction)
                        {
                            _infoPay.gameObject.SetActive(true);
                        }
                        _payButton.ButtonOnClic += OpenCastle;
                        string result2 = AbbreviationsNumbers.ShortNumber(_constructionCastle.CostConstruction);
                        _textPrice.text = result2;
                    }
                }
            }
        }

        private void TurningPagesLeft()
        {
            _castleScene.SetNextCastleLeft(_constructionCastle);
        }

        private void TurningPagesRight()
        {
            _castleScene.SetNextCastleRight(_constructionCastle);
        }

        private void OpenCastle()
        {
            _constructionCastle.TryPay();
            SetComponent(_constructionCastle);
            _payButton.ButtonOnClic -= OpenCastle;
        }

        private void SetValuesFields(ConstructionCastle constructionCastle)
        {
            _buttonUpLevel.ButtonOnClic += UpLevel;
            if(constructionCastle.PersistenProgressService.Progress.Wallet.Coins.Count >= _constructionCastle.CountCurrency)
            {
                _impruvmentInfo.gameObject.SetActive(true);
            }
            else
            {
                _impruvmentInfo.gameObject.SetActive(false);
            }
            _icon.sprite = constructionCastle.Sprite;
            var newlevel = constructionCastle.Level + 1;
            string strRewardCount = SetCountQuantity(_constructionCastle.CountCurrency);
            string strNewRewardCount = SetCountQuantity(constructionCastle.NewCurrency);
            _level.text = $"Уровень + {constructionCastle.Level}<color=green> > {newlevel}</color>";
            _reward.text = $"+{strRewardCount}/{constructionCastle.GenerationTime}мин.<color=green> >" +
                $" +{strNewRewardCount}/{constructionCastle.GenerationTime}мин.</color>";
            
            string strResult = SetCountQuantity(constructionCastle.CurrentСostСonstruction);
            _costUpgrade.text = strResult;
        }

        private string SetCountQuantity(float quantity)
        {
            float rewardCount = Mathf.Round(quantity);
            string result = AbbreviationsNumbers.ShortNumber(rewardCount);
            return result;
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void UpLevel()
        {
            _constructionCastle.TryBuyImprovements();
            SetComponent(_constructionCastle);
            _buttonUpLevel.ButtonOnClic -= UpLevel;
        }

        private string GetTypeBuiding(ConstructionCastle constructionCastle)
        {
            if (constructionCastle.Building == TypeBuilding.TownHall || constructionCastle.Building == TypeBuilding.Market || constructionCastle.Building == TypeBuilding.GoldMine)
                return TextGold;
            else if (constructionCastle.Building == TypeBuilding.CrystalMine)
                return TextCrystals;
            else if (constructionCastle.Building == TypeBuilding.Arena)
                return TextStars;
            else if (constructionCastle.Building == TypeBuilding.Barracks)
                return TextRegularCard;
            else if (constructionCastle.Building == TypeBuilding.Tavern)
                return TextRareCard;
            else if (constructionCastle.Building == TypeBuilding.HallHeroes)
                return TextEpicCard;
            else if (constructionCastle.Building == TypeBuilding.MagicTower)
                return TextPortals;
            else if (constructionCastle.Building == TypeBuilding.BlacksmithShop)
                return TextGemsGreen;
            else if (constructionCastle.Building == TypeBuilding.Palace)
                return TextPalace;

            return null;
        }

        private void SetDescription()
        {
            if (_constructionCastle.Building == TypeBuilding.TownHall || _constructionCastle.Building == TypeBuilding.Market)
                _description.text = TextDescriptionGold;
            else if (_constructionCastle.CurrencyType == CurrencyType.RegularCard ||
                _constructionCastle.CurrencyType == CurrencyType.RareCard ||
                _constructionCastle.CurrencyType == CurrencyType.EpicCard)
                _description.text = TextDescriptionCard;
            else if (_constructionCastle.Building == TypeBuilding.Arena)
                _description.text = TextDesciptionStars;
            else if (_constructionCastle.Building == TypeBuilding.CrystalMine)
                _description.text = TextDescriptionCrystals;
            else if (_constructionCastle.Building == TypeBuilding.MagicTower)
                _description.text = TextDescriptionPortal;
            else if (_constructionCastle.Building == TypeBuilding.BlacksmithShop)
                _description.text = TextDescriptionCrystals;
            else if (_constructionCastle.Building == TypeBuilding.Palace)
                _description.text = TextDescriptionGold;
        }
    }
}
