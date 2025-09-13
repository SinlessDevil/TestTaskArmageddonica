using Code.StaticData.Cards;
using Code.StaticData.Cards.Definition;
using Code.StaticData.Cards.Rank;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.UI.Game.Cards.PM
{
    public interface ICardPM
    {
        InvocationDTO DTO { get; }
        
        CardDefinitionStaticData? DefinitionData { get; }
        CardDefinitionType DefinitionType { get; }
        
        string RankText { get; }
        string CardName { get; }
        string CardDescription { get; }
        string CardMainInfo { get;}
        Sprite CardIcon { get; }
        Color RankColor { get; }
        Sprite BackgroundSprite { get; }
        Sprite BodySprite { get; }
        Sprite LevelBackgroundSprite { get; }
    }
}