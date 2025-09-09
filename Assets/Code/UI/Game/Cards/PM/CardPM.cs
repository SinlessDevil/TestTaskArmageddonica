using Code.Services.IInvocation.DTO;
using Code.Services.StaticData;
using Code.StaticData.Cards;
using Code.StaticData.Cards.Definition;

namespace Code.UI.Game.Cards.PM
{
    public class CardPM : ICardPM
    {
        private readonly IStaticDataService _staticDataService;

        public CardPM(InvocationDTO dto, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            DTO = dto;
        }
        
        public InvocationDTO DTO { get; private set; }

        private CardRankStaticData RankData => _staticDataService.Balance.CardRankStaticData;
        private CardDefinitionCollectionStaticData DefinitionData => _staticDataService.Balance.CardDefinitionCollectionStaticData;
    }
}