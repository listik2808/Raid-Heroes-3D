using Scripts.Army.PlayerSquad;
using Scripts.Army.TypesSoldiers.CharacteristicsSoldier;
using Scripts.StaticData;
using System;
using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class PowerSquadSoldier : MonoBehaviour
    {
        public const string Text = "Сила отряда: ";

        [SerializeField] private TMP_Text _textPowerSquad;
        [SerializeField] private Power _power;
        [SerializeField] private Squad _squad;

        private float _powerSquad;
        private string _resultPower;

        private void OnEnable()
        {
            _squad.ChangedSquad += SquadPowers;
            SquadPowers();
        }

        private void OnDisable()
        {
            _squad.ChangedSquad -= SquadPowers;
        }


        public void SquadPowers()
        {
            _powerSquad = _power.GetSquadPower(_squad.Soldiers);
            _powerSquad = (float)Math.Round(_powerSquad, 2);
            _resultPower = AbbreviationsNumbers.ShortNumber(_powerSquad);
            _textPowerSquad.text = Text + _resultPower;
        }
    }
}
