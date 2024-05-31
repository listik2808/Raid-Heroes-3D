using Assets.Scripts.Economics;
using Scripts.StaticData;
using System;
using UnityEngine;

namespace Scripts.Logic.CastleConstruction
{
    public class PassiveIncome : MonoBehaviour
    {
        public float GetPassiveIncome(ConstructionCastle constructionCastle , bool possible = false)
        {
            float result = 0;
            if (constructionCastle.Level > 0)
            {
                result = constructionCastle.IncomCoefficent;
                if (constructionCastle.CurrencyType == CurrencyType.Gold)
                    result *= (float)Math.Exp(constructionCastle.IncomCoefficentUpgrade * (constructionCastle.Level - 1));//* VipGetPassiveIncomeGoldMultipler(Player.instance.vip, possible);

                if (constructionCastle.CurrencyType == CurrencyType.Crystals)
                    result *= constructionCastle.Level;  //* VipGetPassiveIncomeGoldMultipler(Player.instance.vip, possible);
                if (constructionCastle.CurrencyType == CurrencyType.Stars)
                    result *= (float)constructionCastle.Level * (float)(0.5 + 1);//PVP.instance.getCurrentLeague() * 0.5 + 1); //* VipGetPassiveIncomeGoldMultipler(Player.instance.vip, possible);

                if (constructionCastle.CurrencyType == CurrencyType.GemsGreen)
                    result *= constructionCastle.Level; //* VipGetPassiveIncomeGoldMultipler(Player.instance.vip, possible);
                //result *= (1 / 100);  //+ Shop.instance.getRunesEffectValue(RuneModel.EFFECT_TYPE_BUILDING, b) / 100);
            }
            return (float)Math.Round(result);
        }

        public float GetPassiveIncomeVolume(ConstructionCastle constructionCastle)
	    {
            float result = 0;
            if (constructionCastle.CurrencyType == CurrencyType.Gold ||
            constructionCastle.CurrencyType == CurrencyType.Crystals ||
            constructionCastle.CurrencyType == CurrencyType.Stars ||
            constructionCastle.CurrencyType == CurrencyType.GemsGreen)
                result = constructionCastle.CountCurrency * 10;
            else
                result = constructionCastle.Level;

            return (float)Math.Round(result);
        }

        public float GetBuildingCost(ConstructionCastle constructionCastle)
	    {
            float result = 0;

            if (constructionCastle.Level == 0)
                result = constructionCastle.CostConstruction;
            else
                result = constructionCastle.BasicCostImprovement * (float)Math.Exp(constructionCastle.CoefficientPriceUpgrade * (constructionCastle.Level - 1));

            return (float)Math.Round(result);
        }

        //private void VipGetPassiveIncomeGoldMultipler(int vipType,bool possible)
        //{

        //}
    }
}
