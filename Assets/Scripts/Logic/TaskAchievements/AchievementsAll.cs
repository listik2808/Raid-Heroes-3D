using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.ScreenNavigation;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.Infrastructure.UIWindows.UIProgressReid;
using Scripts.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Logic.TaskAchievements
{
    public abstract class AchievementsAll : MonoBehaviour
    {
        public TMP_Text TextTaskClose;
        public Slider Slider;
        public Button Button;
        public TMP_Text SladerText;
        public TMP_Text TextRequirements;
        public TMP_Text Reward;
        protected List<int> Id;
        protected List<float> RevardCrystals;
        protected List<float> Requirements;
        protected float CurrentIndexId;
        protected float CurrentReward;
        protected float CurrentRequirement;
        protected int MaxID;
        protected int Marker;
        protected bool TasksClosed = false;
        protected IPersistenProgressService ProgressService;
        protected MainScreen MainScreen;
        protected ScreenTask ScreenTask;
        protected bool OpenBatton;
        public event Action ChangetMarker;
        private string _resultOne;
        private string _resultTwo;
        private int _index = 0;
        private string _reward;
        private float _currentFillSlider = 0;

        public float CurrentFillSlider => _currentFillSlider;
        public int MArkers => Marker;
        public bool IsOpenBatton => OpenBatton;
        public IPersistenProgressService PersistenProgressService => ProgressService;

        private void OnEnable()
        {
            Button.onClick.AddListener(GetRewarded);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(GetRewarded);
        }

        public abstract void Construct(MainScreen mainScreen,ScreenTask screenTask);
        protected abstract void FillCard();
        protected abstract void GetRewarded();

        public void ActivatedCard()
        {
            CheckingAvailableTasks(CurrentIndexId, MaxID);
        }

        protected void MaxId()
        {
            var i = Id.Count;
            MaxID = Id[i-1];
        }

        protected void SetTextRequirements(float value, string textOne, string textTwo)
        {
            SetIndex(value);
            string res =  AbbreviationsNumbers.ShortNumber(Requirements[_index]);
            TextRequirements.text = textOne + res + textTwo;
        }

        protected void SetIndex(float value)
        {
            for (int i = 0; i < Requirements.Count; i++)
            {
                if (Id[i] == value)
                {
                    _index = i;
                }
            }
        }

        protected void Preparation(float currentIndex)
        {
            SetIndex(currentIndex);
            CurrentRequirement = Requirements[_index];
            CurrentReward = RevardCrystals[_index];
            _reward = AbbreviationsNumbers.ShortNumber(CurrentReward);
            Reward.text = _reward;
        }

        protected bool FillSlider(float currentValue,float maxValue)
        {
            Slider.value = (float)currentValue / (float)maxValue;
            _currentFillSlider = Slider.value;
            _resultOne = AbbreviationsNumbers.ShortNumber(currentValue);
            _resultTwo = AbbreviationsNumbers.ShortNumber(maxValue);
            SladerText.text = _resultOne + "/" + _resultTwo; 
            if(currentValue >= maxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void CheckingAvailableTasks(float currentIndex, int maxId)
        {
            if (currentIndex <= maxId)
            {
                FillCard();
            }
            else
            {
                _currentFillSlider = -1;
                TasksClosed = true;
                Marker = 0;
                Slider.gameObject.SetActive(false);
                Button.gameObject.SetActive(false);
                TextTaskClose.gameObject.SetActive(true);
                ChangetMarker?.Invoke();
            }
        }

        protected void ActivateSliderOrButton()
        {
            if (OpenBatton)
            {
                Slider.gameObject.SetActive(false);
                Button.gameObject.SetActive(true);
                Marker = 1;
            }
            else
            {
                Button.gameObject.SetActive(false);
                Slider.gameObject.SetActive(true);
                Marker = 0;
            }
            ChangetMarker?.Invoke();
        }

        protected void SetNewId()
        {
            if(CurrentIndexId < MaxID)
            {
                for (int i = 0; i < Id.Count; i++)
                {
                    var result = Id[i];
                    if (result == CurrentIndexId)
                    {
                        result = Id[i + 1];
                        CurrentIndexId = result;
                        break;
                    }
                }
            }
            else if(CurrentIndexId == MaxID)
            {
                CurrentIndexId++;
            }
            CheckingAvailableTasks(CurrentIndexId, MaxID);
        }
    }
}
