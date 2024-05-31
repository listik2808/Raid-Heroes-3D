using Assets.Scripts.Economics;
using Scripts.Army.AllCadsHeroes;
using Scripts.Infrastructure.Services.PersistentProgress;
using Scripts.Infrastructure.UIWindows.Screens;
using Scripts.NextScene;
using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Scripts.Infrastructure.AssetManagement
{
    public class RandomChanceCard : MonoBehaviour
    {
        [SerializeField] private ListAllHeroes _listAllHeroes;
        [SerializeField] private ScreenCardShow _screenCardShow;
        private MainStage _mainStage;
        private IPersistenProgressService _progressService;
        public ScreenCardShow ScreenCardShow => _screenCardShow;
        public void Construct(MainStage mainStage)
        {
            _mainStage = mainStage;
        }

        public int TryGetCardRandom(int idBattle,IPersistenProgressService persistenProgressService)
        {
            _progressService = persistenProgressService;
            System.Random random = new System.Random();
            int numberCard;
            if(_progressService.Progress.Training.Tutor == false && _progressService.Progress.Training.CountCard < 5)
            {
                numberCard = _listAllHeroes.EnrollRandomHeroCard(false,1.2f, 1, 1, _mainStage);
                return numberCard;
            }
            else
            {
                if (idBattle == 0)
                {
                    numberCard = _listAllHeroes.EnrollRandomHeroCard(true,1.2f, 1, 0, _mainStage);
                    return numberCard;
                }
                else
                {
                    float r = (float)random.NextDouble();
                    if (r < AssetPath.ChanceCardPVEEpic)
                    {
                        numberCard = _listAllHeroes.EnrollRandomHeroCard(true, 1.2f, 1, 3, _mainStage);
                        return numberCard;
                    }
                    else if (r < AssetPath.ChanceCardPVEEpic + AssetPath.ChanceCardPVERare)
                    {
                        numberCard = _listAllHeroes.EnrollRandomHeroCard(true, 1.2f, 1, 2, _mainStage);
                        return numberCard;
                    }
                    else if (r < AssetPath.ChanceCardPVESimple + AssetPath.ChanceCardPVERare + AssetPath.ChanceCardPVEEpic)
                    {
                        numberCard = _listAllHeroes.EnrollRandomHeroCard(true, 1.2f, 1, 1, _mainStage);
                        return numberCard;
                    }
                }
                return -1;
            }
        }
    }
}