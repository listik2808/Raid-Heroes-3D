using Scripts.Army.PlayerSquad;
using Scripts.Artifacts;
using Scripts.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Army.TypesSoldiers.CharacteristicsSoldier
{
    public class DataSoldier : MonoBehaviour
    {
        [SerializeField] protected EnemyHeaith _enemyHealth;
        [SerializeField] protected CardType _type;
        //[SerializeField] protected Sprite _iconSoldier;
        [SerializeField] protected Card _typeSoldierCard;
        //[SerializeField] protected Infrastructure.Player.Avatar _avatar;
        [SerializeField] protected float _baseMeleeDamage;
        [SerializeField] protected float _durationRecoveryMeleeDamage;
        [SerializeField] protected float _baseHealthValue;
        //[SerializeField] protected float _baseSpeedValue;
        //[SerializeField] protected float _maxSpeedValue;
        [SerializeField] protected float _distanceFightMelle;
        protected float _rangeAttack;
        protected float _maxValueHeaith;
        protected float _maxBaseMeleeDamage;

        private SoldierCard _soldierСard;
        protected List<Artifact> _artifacts =new List<Artifact>();
        protected bool _hired = false;
        protected bool _inSquad = false;

        public SoldierCard SoldierСard => _soldierСard;
        public EnemyHeaith EnemyHealth => _enemyHealth;
        public bool InSquad => _inSquad;
        public bool Hired => _hired;
        public CardType Type => _type;
        public Card CardSoldierType => _typeSoldierCard;
        //public Sprite IconSoldier => _iconSoldier;
        //public float BaseSpeedValue => _baseSpeedValue;
        public float BaseMeleeDamage => _baseMeleeDamage;
        public float MaxBaseMaleeDamage => _maxBaseMeleeDamage;
        public float BaseHealthValue => _baseHealthValue;
        public float MaxHealthValue => _maxValueHeaith;
        public float RangeAttack => _rangeAttack;
        public float DistanceFigtMelle => _distanceFightMelle;
        public float DurationRecoveryMeleeDamage => _durationRecoveryMeleeDamage;


        public void SetRange(float range)
        {
            _rangeAttack = range;
        }
        public void SetCardSoldier(SoldierCard soldierСard)
        {
            _soldierСard = soldierСard;
        }

        public void AddSquad() => 
            _inSquad = true;

        public void RemuveAquad() => 
            _inSquad = false;

        public void SetCardActivation() =>
            _hired = true;

        public void RemoveCardActivation() =>
            _hired = false;

        public void LoadHired(bool hired)
        {
            _hired = hired;
        }

        //public void SetCard(SoldierСard pointCard) =>
        //    _soldierСard = pointCard;
    }
}
