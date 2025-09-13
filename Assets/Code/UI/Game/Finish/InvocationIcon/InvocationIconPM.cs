using Code.Services.StaticData;
using Code.StaticData.Cards;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.UI.Game.Finish.InvocationIcon
{
    public class InvocationIconPM : IInvocationIconPM
    {
        private readonly IStaticDataService _staticDataService;
        private readonly InvocationDTO _invocationDTO;
        
        public InvocationIconPM(InvocationDTO invocationDTO, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _invocationDTO = invocationDTO;
        }

        public string GetName() => CardDefinitionData?.Name;

        public Sprite GetSprite() => CardDefinitionData?.Icon;

        public int GetQuantity() => _invocationDTO.Quantity;

        private CardDefinitionStaticData CardDefinitionData => _staticDataService.Balance.CardDefinitionCollectionStaticData
            .GetCardDefinitionByType(_invocationDTO.CardDefinition);
    }
}