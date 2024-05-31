using Assets.Scripts.Economics.Banners;
using Assets.Scripts.Economics.Medals;
using UnityEngine;
using Newtonsoft.Json;
using Assets.Scripts.Economics.Cards.Scripts;
using System.Text;
using Scripts.Economics.Buildings;
using Scripts.Data.TypeHeroSoldier;
using System;

[Serializable]
public class PlayerData
{
    public Banners Banners;
    public Medals Medals;
    public Cards Cards;
    public Building Building;
    public TypeHero TypeHero;

    public PlayerData() 
    {
        Banners = new Banners();
        Medals = new Medals();
        Cards = new Cards();
        Building = new Building();
        TypeHero = new TypeHero();
    }

    //public static void GenerateInfo()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendLine("Wallet");
    //    //sb.AppendLine(JsonConvert.SerializeObject(Wallet));
    //    sb.AppendLine("Gems");
    //    //sb.AppendLine(JsonConvert.SerializeObject(Gems));
    //    //sb.AppendLine("ArenaPasses");
    //    //sb.AppendLine(JsonConvert.SerializeObject(ArenaPasses));
    //    //sb.AppendLine("Portals");
    //    //sb.AppendLine(JsonConvert.SerializeObject(Portals));
    //    //sb.AppendLine("Banners");
    //    //sb.AppendLine(JsonConvert.SerializeObject(Banners));
    //    //sb.AppendLine("Medals");
    //    //sb.AppendLine(JsonConvert.SerializeObject(Medals));
    //    //sb.AppendLine("Cards");
    //    //sb.AppendLine(JsonConvert.SerializeObject(Cards));

    //    Debug.Log(sb);
    //}
}
