using Scripts.Army.TypesSoldiers;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroCard", menuName = "Card/NewCard")]
public partial class Card : ScriptableObject
{
    public string Name;
    public BaseCard BaseCard;
    public Soldier Soldier;

    public Card(string name, BaseCard baseCard = null,Soldier soldier =null)
    {
        Name = name;
        BaseCard = baseCard;
        Soldier = soldier;
    }

    public override bool Equals(object obj)
    {
        return obj is Card card &&
               Name == card.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}
