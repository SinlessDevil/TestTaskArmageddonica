using System.Collections.Generic;
using Code.Services.Invocations.RandomizerDTO;
using Code.Services.StaticData;
using Code.StaticData.Invocation.DTO;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;
using UnityEngine;

namespace Code.Services.Providers.CardComposites
{
    public class CardCompositeProvider : ICardCompositeProvider
    {
        private readonly IPoolProvider<CardView> _cardViewProvider;
        private readonly IInvocationDTORandomizerService _invocationDtoRandomizerService;
        private readonly IStaticDataService _staticDataService;
        
        public CardCompositeProvider(
            IPoolProvider<CardView> cardViewProvider,
            IInvocationDTORandomizerService invocationDtoRandomizerService,
            IStaticDataService staticDataService)
        {
            _cardViewProvider = cardViewProvider;
            _invocationDtoRandomizerService = invocationDtoRandomizerService;
            _staticDataService = staticDataService;
        }
        
        public List<CardComposite> CreateRandomUnitCards(int count)
        {
            List<CardComposite> cardComposites = new List<CardComposite>();
            
            for (int i = 0; i < count; i++)
            {
                UnitDTO unitDTO = _invocationDtoRandomizerService.GenerateRandomUnitDTO();
                if (unitDTO == null)
                {
                    Debug.LogError($"[CardCompositeProvider] Failed to generate random unit DTO for card {i}");
                    continue;
                }
                
                CardPM cardPM = new CardPM(unitDTO, _staticDataService);
                CardView cardView = _cardViewProvider.Get(Vector3.zero, Quaternion.identity);
                cardView.Initialize(cardPM);
                CardComposite cardComposite = new CardComposite(cardView, cardPM);
                cardComposites.Add(cardComposite);
            }
            
            return cardComposites;
        }
        
        public List<CardComposite> CreateMixedTypeCards()
        {
            List<CardComposite> cardComposites = new List<CardComposite>();
            
            UnitDTO unitDTO = _invocationDtoRandomizerService.GenerateRandomUnitDTO();
            if (unitDTO != null)
            {
                CardPM unitCardPM = new CardPM(unitDTO, _staticDataService);
                CardView unitCardView = _cardViewProvider.Get(Vector3.zero, Quaternion.identity);
                unitCardView.Initialize(unitCardPM);
                CardComposite unitCardComposite = new CardComposite(unitCardView, unitCardPM);
                cardComposites.Add(unitCardComposite);
            }
            else
            {
                Debug.LogError("[CardCompositeProvider] Failed to generate random unit DTO");
            }
            
            BuildDTO buildDto = _invocationDtoRandomizerService.GenerateRandomBuildingDTO();
            if (buildDto != null)
            {
                CardPM buildingCardPM = new CardPM(buildDto, _staticDataService);
                CardView buildingCardView = _cardViewProvider.Get(Vector3.zero, Quaternion.identity);
                buildingCardView.Initialize(buildingCardPM);
                CardComposite buildingCardComposite = new CardComposite(buildingCardView, buildingCardPM);
                cardComposites.Add(buildingCardComposite);
            }
            else
            {
                Debug.LogError("[CardCompositeProvider] Failed to generate random building DTO");
            }
            
            SkillDTO skillDTO = _invocationDtoRandomizerService.GenerateRandomSkillDTO();
            if (skillDTO != null)
            {
                CardPM skillCardPM = new CardPM(skillDTO, _staticDataService);
                CardView skillCardView = _cardViewProvider.Get(Vector3.zero, Quaternion.identity);
                skillCardView.Initialize(skillCardPM);
                CardComposite skillCardComposite = new CardComposite(skillCardView, skillCardPM);
                cardComposites.Add(skillCardComposite);
            }
            else
            {
                Debug.LogError("[CardCompositeProvider] Failed to generate random skill DTO");
            }
            
            return cardComposites;
        }
        
        public void ReturnCardComposite(CardComposite cardComposite)
        {
            if (cardComposite == null) 
                return;
            
            cardComposite.View.Dispose();
            _cardViewProvider.Return(cardComposite.View);
        }
        
        public void ReturnCardComposite(CardView cardView)
        {
            if (cardView == null) 
                return;
            
            cardView.Dispose();
            _cardViewProvider.Return(cardView);
        }
        
        public void ReturnCardComposites(List<CardComposite> cardComposites)
        {
            if (cardComposites == null) 
                return;
            
            foreach (var cardComposite in cardComposites)
                ReturnCardComposite(cardComposite);
        }
    }
}