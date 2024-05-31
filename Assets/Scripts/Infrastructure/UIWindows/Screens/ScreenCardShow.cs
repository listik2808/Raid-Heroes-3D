using Scripts.Army.AllCadsHeroes;
using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers;
using Scripts.Infrastructure.AssetManagement;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic;
using Scripts.Logic.CastleConstruction;
using Scripts.NextScene;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class ScreenCardShow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textTypeHero;
        [SerializeField] private Image _imageCars;
        [SerializeField] private Image _imageCardType;
        [SerializeField] private RawImage _imageRaw;
        [SerializeField] private TMP_Text _count;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private BookmarkButton _bookmarkButton;
        [SerializeField] private Squad _squad;
        [SerializeField] private Heroes _heroes;
        [SerializeField] private Canvas _canvasMain;
        [SerializeField] private CanvasScaler _canvasScaleMain;
       // [SerializeField] private Image _image;
        [SerializeField] private Canvas _mainCanvasHud;
        private CameraParent _cameraParents;
        private RenderTexture _mainTexture;
        private ConstructionCastle _construction;
        private SoldierCard _soldierСard;
        private Soldier _soldier;
        private ListAllHeroes _allHeroes;
        private MainStage _mainStage;
        private bool _isFound = false;
        private string _name;
        private int _countCardAdd =0;

        public BookmarkButton BookmarkButton => _bookmarkButton;

        public void SetComponentsCard(Soldier soldier , int count , ListAllHeroes listAllHeroes,ConstructionCastle constructionCastle = null,MainStage mainStage = null)
        {
            _bookmarkButton.ButtonOnClic += CloseWindowCard;
            _allHeroes = listAllHeroes;
            _countCardAdd = count;
            _soldier = soldier;
            _construction = constructionCastle;
            SetHeroCardObject(soldier,_squad.SoldierСards);
            if (_isFound == false)
            {
                SetHeroCardObject(soldier, _heroes.SoldierСards);
                if (_isFound == false)
                {
                    SetHeroCardObject(soldier, _heroes.HerosCards.SoldierСardsAll);
                }
            }
            _imageRaw.texture = _mainTexture;
            _imageCars.sprite = soldier.DataSoldier.CardSoldierType.BaseCard.TypeSprites[(int)soldier.DataSoldier.Type];
            _imageCardType.sprite = soldier.DataSoldier.CardSoldierType.BaseCard.TypeIcons[(int)soldier.DataSoldier.Type];
            //_name = soldier.HeroTypeId.ToString();
            //_name = Lean.Localization.LeanLocalization.GetTranslationText(_name);
            _name = CustomRuLocalization.GetRuHeroTypeId((int)soldier.HeroTypeId);
            _textTypeHero.text = _name;
            _count.text = count.ToString();
            if (mainStage != null)
            {
                _cameraParents.gameObject.transform.position = new Vector3(0, 11, 0);
                _cameraParents.gameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
                _mainCanvasHud.enabled = false;
                _mainStage = mainStage;
            }

            _isFound = false;
        }

        public void CloseWindowCard()
        {
            _allHeroes.SetCard(_countCardAdd, _soldier);
            _cameraParents.gameObject.SetActive(false);
            _mainCanvasHud.enabled = true;
            _bookmarkButton.ButtonOnClic -= CloseWindowCard;
            if(SceneManager.GetActiveScene().name == AssetPath.SceneMain)
            {
                if (_construction != null)
                    _construction.DiactivateCardShoeWindow();
                else
                    gameObject.SetActive(false);
            }
            else if(SceneManager.GetActiveScene().name == AssetPath.SceneBattle)
            {
                _mainStage.LoadScene(this);
            }
            
        }

        private void SetHeroCardObject(Soldier soldier,List<SoldierCard> soldierСard)
        {
            foreach (var item in soldierСard)
            {
                if(soldier.HeroTypeId == item.SoldierCardViewer.Card.Soldier.HeroTypeId)
                {
                    _isFound = true;
                    _soldierСard = item;
                    _mainTexture = item.SoldierCardViewer.CameraView.RenderTexture;
                    _cameraParents = item.SoldierCardViewer.CameraView;
                    item.SoldierCardViewer.CameraView.gameObject.SetActive(true);
                }
            }
        }
    }
}
