using Assets.Scripts.Currency;

[System.Serializable]
public class Wallet
{
    public Coins Coins;
    public Diamonds Diamonds;
    public Stars Stars;

    public Wallet()
    {
        Coins = new Coins();
        Diamonds = new Diamonds();
        Stars = new Stars();
    }
}
