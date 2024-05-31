using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Infrastructure.Player
{
    public class Avatar : MonoBehaviour
    {
        // тут будем знать об активных ячейках и ечейках которые заняты рунами
        private List<Rune> _runes = new List<Rune>();

        public List<Rune> Runes => _runes;
    }
}