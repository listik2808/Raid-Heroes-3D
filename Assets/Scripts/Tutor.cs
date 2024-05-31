using Scripts.Army.PlayerSquad;
using Scripts.Infrastructure.Services;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.Services.SaveLoad;
using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Infrastructure.UIWindows.Screens;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Source.Scripts.Logic
{
    public class Tutor : MonoBehaviour
    {
        [SerializeField] private WarSelectionScreen _warSelectionScreen;
        [SerializeField] private Button _buttonOption;
        [SerializeField] private List<BookmarkButton> _bookmarkButtons;
        [SerializeField] private Image _imageFinger;
        [SerializeField] private Image _imageFingerCard;
        [SerializeField] private Image _imageFingerCardSuqd;
        [SerializeField] private Image _imageFingerHire;
        [SerializeField] private Squad _squad;
        [SerializeField] private BookmarkButton _buttonCloseUpgrade;
        [SerializeField] private List<BookmarkButton> _bookmarkButtonsButtonTutor = new List<BookmarkButton>();
        [SerializeField] private BookmarkButton _buttonUpSpec;
        [SerializeField] private Image _imageFingerSpecAttacupdgadeButton;
        [SerializeField] private Image _fingerimageClose;
        [SerializeField] private Image _raidFinger;
        [SerializeField] private Image _fingerButtonattack;
        [SerializeField] private ButtonMultiplier _buttonMultiplier;
        [SerializeField] private List<BookmarkButton> _bookmarkButtonsWallet = new List<BookmarkButton>();
        [SerializeField] private Heroes _heroes;
        [SerializeField] private Button _pawer;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _backgraund;
        [SerializeField] private BookmarkButton _hierButton;
        [SerializeField] private List<BookmarkButton> _arrowOrButtonRemuveSoldier;

        private IPersistenProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private bool _isActiveTutor = false;

        public bool IsActiveTutor => _isActiveTutor;
        public IPersistenProgressService ProgressService => _progressService;

        //Тутор
        private void Awake()
        {
            _progressService = AllServices.Container.Single<IPersistenProgressService>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _isActiveTutor = _progressService.Progress.Training.Tutor;
            if (YandexGame.SDKEnabled == true)
            {
                ActivateOrDiactivate();
            }
           
        }

        private void ButtonScenDiactivate()
        {
            foreach (BookmarkButton bookmarkButton in _bookmarkButtons)
            {
                bookmarkButton.Button.enabled = false;
            }
        }

        private void ActivateOrDiactivate()
        {
            if (YandexGame.savesData.SelectionWindow == false)
            {
                _warSelectionScreen.gameObject.SetActive(true);
            }
            if (_isActiveTutor)
            {
                enabled = false;
            }
            else
            {
                StartTutor();
            }
        }

        private void StartTutor()
        {
            if (_progressService.Progress.PointSpawn.IdSpawnerEnemy == 1)
            {
                ButtonScenDiactivate();
            }
            else if (_progressService.Progress.PointSpawn.IdSpawnerEnemy == 2
                && _progressService.Progress.Training.Tutor == false
                && _progressService.Progress.Training.UpLevelSpecSkill == false)
            {
                ActivateHeroesFinger();
            }
            else if (_progressService.Progress.PointSpawn.IdSpawnerEnemy == 2
                && _progressService.Progress.Training.Tutor == false
                && _progressService.Progress.Training.UpLevelSpecSkill == true)
            {
                _fingerButtonattack.gameObject.SetActive(true);
                _attackButton.enabled = true;
                ButtonScenDiactivate();
            }
            else if (_progressService.Progress.Training.Tutor == false &&
                _progressService.Progress.Training.CountCard == 5 && _progressService.Progress.Training.Finish == false)
            {
                ActivateHeroesFinger();
            }
        }

        private void DiactivateFinger()
        {
            _bookmarkButtons[1].ButtonOnClic -= DiactivateFinger;
            _pawer.enabled = false;
            _imageFinger.gameObject.SetActive(false);

            foreach (BookmarkButton item in _bookmarkButtonsWallet)
            {
                item.Button.enabled = false;
            }

            if (_progressService.Progress.Training.CountCard == 5)
            {
                _imageFingerCardSuqd.gameObject.SetActive(true);
                foreach (SoldierCard item in _squad.SoldierСards)
                {
                    item.SoldierCardViewer.BookmarkButton.Button.enabled = false;
                }

                AddLisinerButtonSoldierCardViewwer();
            }
            else
            {
                LessFiveCards();
            }
        }

        private void AddLisinerButtonSoldierCardViewwer()
        {
            foreach (SoldierCard item in _heroes.SoldierСards)
            {
                item.SoldierCardViewer.BookmarkButton.ButtonOnClic += HireHero;
            }
        }

        private void RemuveLisinerButtonSoldierCardViewwer()
        {
            foreach (SoldierCard item in _heroes.SoldierСards)
            {
                item.SoldierCardViewer.BookmarkButton.ButtonOnClic -= HireHero;
            }
        }

        private void HireHero()
        {
            _buttonMultiplier.Button.enabled = false;
            _backgraund.enabled = false;
            RemuveLisinerButtonSoldierCardViewwer();
            foreach (var item in _bookmarkButtonsButtonTutor)
            {
                item.Button.enabled = false;
            }
            _buttonCloseUpgrade.enabled = false;
            _attackButton.enabled = true;
            _imageFingerCardSuqd.gameObject.SetActive(false);
            _imageFingerHire.gameObject.SetActive(true);
            _hierButton.ButtonOnClic += DiactivateHier;
        }

        private void DiactivateHier()
        {
            foreach (var item in _arrowOrButtonRemuveSoldier)
            {
                item.Button.enabled = false;
            }
            _buttonUpSpec.Button.enabled = false;
            _buttonCloseUpgrade.enabled = true;
            _imageFingerHire.gameObject.SetActive(false);
            _progressService.Progress.Training.Finish = true;
            _fingerButtonattack.gameObject.SetActive(true);
            _fingerimageClose.gameObject.SetActive(true);
            _buttonCloseUpgrade.Button.onClick.AddListener(DiactivateHeroFinger);

        }

        private void DiactivateFingercardHero()
        {
            _buttonMultiplier.Button.enabled = false;
            foreach (var item in _squad.SoldierСards)
            {
                item.SoldierCardViewer.BookmarkButton.ButtonOnClic -= DiactivateFingercardHero;
            }
            _imageFingerCard.gameObject.SetActive(false);
            _buttonCloseUpgrade.Button.enabled = false;
            _backgraund.enabled = false;
            foreach (var item in _bookmarkButtonsButtonTutor)
            {
                item.Button.enabled = false;
            }
            _imageFingerSpecAttacupdgadeButton.gameObject.SetActive(true);
            _buttonUpSpec.ButtonOnClic += UpgradeSpecTutor;
        }

        private void UpgradeSpecTutor()
        {
            _buttonUpSpec.ButtonOnClic -= UpgradeSpecTutor;
            _progressService.Progress.Training.UpLevelSpecSkill = true;
            _imageFingerSpecAttacupdgadeButton.gameObject.SetActive(false);
            _buttonUpSpec.Button.enabled = false;
            _fingerimageClose.gameObject.SetActive(true);
            _buttonCloseUpgrade.Button.enabled = true;
            _buttonCloseUpgrade.ButtonOnClic += CloseScreenupgrade;
        }

        private void CloseScreenupgrade()
        {
            _buttonCloseUpgrade.ButtonOnClic -= CloseScreenupgrade;
            _squad.SoldierСards[0].SoldierCardViewer.BookmarkButton.Button.enabled = false;
            for (int i = 0; i < _bookmarkButtons.Count; i++)
            {
                if(i == 2)
                {
                    _bookmarkButtons[i].Button.enabled = true;
                    _bookmarkButtons[i].ButtonOnClic += ActivateTransiteTutorRaid;
                    _raidFinger.gameObject.SetActive(true);
                }
                else
                {
                    _bookmarkButtons[i].Button.enabled = false;
                }
            }
        }

        private void ActivateTransiteTutorRaid()
        {
            _bookmarkButtons[2].ButtonOnClic -= ActivateTransiteTutorRaid;
            _raidFinger.gameObject.SetActive(false);
            _fingerButtonattack.gameObject.SetActive(true);
            _attackButton.enabled = true;
            _saveLoadService.SaveProgress();
        }

        private void ActivateHeroesFinger()
        {
            _attackButton.enabled = false;
            for (int i = 0; i < _bookmarkButtons.Count; i++)
            {
                if (i == 1)
                {
                    _bookmarkButtons[i].Button.enabled = true;
                    _bookmarkButtons[i].ButtonOnClic += DiactivateFinger;
                    _imageFinger.gameObject.SetActive(true);
                }
                else
                {
                    _bookmarkButtons[i].Button.enabled = false;
                }
            }
        }

        private void DiactivateHeroFinger()
        {
            foreach (SoldierCard item in _squad.SoldierСards)
            {
                item.SoldierCardViewer.BookmarkButton.Button.enabled = false;
            }
            foreach (BookmarkButton item in _bookmarkButtonsWallet)
            {
                item.Button.enabled = false;
            }
            foreach (var item in _bookmarkButtonsButtonTutor)
            {
                item.Button.enabled = false;
            }
            _buttonCloseUpgrade.Button.onClick.RemoveListener(DiactivateHeroFinger);
           
            _attackButton.enabled = true;
            //_pawer.enabled = true;
            _raidFinger.gameObject.SetActive(true);
            for (int i = 0; i < _bookmarkButtons.Count; i++)
            {
                _bookmarkButtons[i].ButtonOnClic -= DiactivateFinger;
                if(i == 2)
                {
                    _raidFinger.enabled = true;
                    _bookmarkButtons[i].Button.enabled = true;
                    _bookmarkButtons[i].Button.onClick.AddListener(CloseFinishTutor);
                }
            }
            
        }

        private void CloseFinishTutor()
        {
            _bookmarkButtons[1].enabled = false;
            _bookmarkButtons[2].Button.onClick.RemoveListener(CloseFinishTutor);
            _raidFinger.gameObject.SetActive(false);
            _saveLoadService.SaveProgress();
        }

        private void LessFiveCards()
        {
            _imageFingerCard.gameObject.SetActive(true);
            foreach (SoldierCard item in _squad.SoldierСards)
            {
                item.SoldierCardViewer.BookmarkButton.ButtonOnClic += DiactivateFingercardHero;
            }

            foreach (SoldierCard item in _heroes.SoldierСards)
            {
                item.SoldierCardViewer.BookmarkButton.Button.enabled = false;
            }
        }
    }
}
