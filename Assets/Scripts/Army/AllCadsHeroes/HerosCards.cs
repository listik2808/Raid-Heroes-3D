using Scripts.Army.PlayerSquad;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Army.AllCadsHeroes
{
    public class HerosCards :MonoBehaviour
    {
        [SerializeField] private List<SoldierCard> _soldierСards = new List<SoldierCard>();
        [SerializeField] private Transform _containerHeroe;

        public List<SoldierCard> SoldierСardsAll => _soldierСards;
        public Transform ContainerHeroe => _containerHeroe;

        public void RemoveCard(SoldierCard heroCard)
        {
            List<SoldierCard> сardsoldier = new List<SoldierCard>();
            foreach (SoldierCard soldierСard in _soldierСards)
            {
                if (heroCard.SoldierCardViewer.Card.Soldier.HeroTypeId != soldierСard.SoldierCardViewer.Card.Soldier.HeroTypeId)
                {
                    сardsoldier.Add(soldierСard);
                }
            }
            _soldierСards.Clear();
            _soldierСards = сardsoldier;
        }
    }
}
