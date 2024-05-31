using Scripts.Infrastructure.UIWindows.ButtonUI;
using Scripts.Logic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Army.TypesSoldiers.TypeCardClass
{
    public abstract class SoldierCardViewer : MonoBehaviour
    {
        public CardType CardType;
        public Card Card;
        //public Image Icon;
        public TMP_Text Text;
        public Image Type;
        public Image Background;
        public BookmarkButton BookmarkButton;
        public BookmarkButton CloseScreen;
        public CameraParent CameraView;
        public Image InfoMarker;
        public TMP_Text InfoText;
        protected int _allMarker = 0;

        public int AllMarker => _allMarker;

        //public event Action ChangedCountCardADD;
        private void OnEnable()
        {
            CloseScreen.ButtonOnClic += CloseUpgradeSoldier;
            BookmarkButton.ButtonOnClic += OpenSoldierUpgradeScreen;
        }

        private void OnDisable()
        {
            BookmarkButton.ButtonOnClic -= OpenSoldierUpgradeScreen;
            CloseScreen.ButtonOnClic -= CloseUpgradeSoldier;
        }

        public abstract void SetComponent();

        public abstract void OpenSoldierUpgradeScreen();

        public abstract void CloseUpgradeSoldier();

        public abstract void SaveDataHero();

        public abstract void SetComponentSevices();

        public abstract void Start();

        public void SetbaseComponent()
        {
            //Icon.gameObject.SetActive(false);
           // Icon.sprite = Card.Soldier.DataSoldier.IconSoldier;
            Type.sprite = Card.BaseCard.TypeIcons[(int)CardType];
            Background.sprite = Card.BaseCard.TypeSprites[(int)CardType];
            //Background.color = Card.BaseCard.TypeColors[(int)CardType];
        }

        public void SetBaseComponentZoro()
        {
            //Icon.sprite = Card.Soldier.DataSoldier.IconSoldier;
            Type.sprite = Card.BaseCard.TypeIcons[(int)CardType];
        }

        public void AddCountCard(int value)
        {
            Card.Soldier.Rank.AddCountCard(value);
            //ChangedCountCardADD?.Invoke();
            //Start();
            //SetComponent();
        }

        public void RenderinfTextCountCard(int value)
        {
            Text.text = Card.Soldier.Rank.CurrentCountCard.ToString() + " / " + Card.Soldier.Rank.MaxCountCard.ToString();
        }
    }
}
