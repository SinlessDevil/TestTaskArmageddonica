using Code.Services.IInvocation.DTO;
using Code.Services.StaticData;
using Code.StaticData.Cards;
using Code.StaticData.Cards.Definition;
using Code.StaticData.Cards.Rank;
using UnityEngine;

namespace Code.UI.Game.Cards.PM
{
    public class CardPM : ICardPM
    {
        private readonly IStaticDataService _staticDataService;
        private int _level = 1;

        public CardPM(InvocationDTO dto, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            DTO = dto;
        }
        
        public InvocationDTO DTO { get; private set; }
        
        public CardRankData? RankData => RankStaticData.GetCardDataByRank(DTO.Rank);
        public CardRankType RankType => DTO.Rank;
        
        public CardDefinitionStaticData? DefinitionData => DefinitionCollectionData.GetCardDefinitionByType(DTO.CardDefinition);
        public CardDefinitionType DefinitionType => DTO.CardDefinition;

        public string RankId => RankData?.RankText;
        public string CardName => DefinitionData?.Name ?? "Unknown Card";
        public string CardDescription => DefinitionData?.Description ?? "No description available";
        public Sprite CardIcon => DefinitionData?.Icon;
        public Color RankColor => RankData?.Color ?? Color.white;
        public Sprite BackgroundSprite => RankData?.BgSprite;
        public Sprite BodySprite => RankData?.BodySprite;
        public Sprite LevelBackgroundSprite => RankData?.BgLevelSprite;
        public string RankText => RankData?.RankText ?? DTO.Rank.ToString();
        public int Level => _level;
        
        private CardRankStaticData RankStaticData => _staticDataService.Balance.CardRankStaticData;
        private CardDefinitionCollectionStaticData DefinitionCollectionData => _staticDataService.Balance.CardDefinitionCollectionStaticData;
    }
}