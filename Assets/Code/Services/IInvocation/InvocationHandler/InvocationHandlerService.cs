using System;
using Code.Logic.Grid;
using Code.Logic.Invocation;
using Code.Services.Context;
using Code.Services.IInvocation.InvocationHandler;
using Code.Services.Input.Card;
using Code.Services.IInvocation.StaticData;
using Code.UI.Game.Cards.PM;
using Code.UI.Game.Cards.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services.IInvocation.InvocationHandle
{
    public class InvocationHandlerService : IInvocationHandlerService
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        private readonly IGameContext _gameContext;
        private readonly ICardInputService _cardInputService;

        public InvocationHandlerService(
            IInvocationStaticDataService invocationStaticDataService,
            IGameContext gameContext,
            ICardInputService cardInputService)
        {
            _invocationStaticDataService = invocationStaticDataService;
            _gameContext = gameContext;
            _cardInputService = cardInputService;
        }

        public event Action InvocationSpawnedEvent;

        public void Initialize()
        {
            _cardInputService.DroppedOnCell += OnCardDroppedOnCell;
        }

        public void Dispose()
        {
            _cardInputService.DroppedOnCell -= OnCardDroppedOnCell;
        }

        private Invocation GetInvocation(ICardPM cardPM, Cell targetCell)
        {
            Vector3 spawnPosition = GetSpawnPosition(targetCell);
            GameObject invocationObject = Object.Instantiate(cardPM.DTO.Prefab, spawnPosition, Quaternion.identity);
            Invocation invocation = invocationObject.GetComponent<Invocation>();
            invocation.transform.position = spawnPosition;
            return invocation;
        }

        private void OnCardDroppedOnCell(CardView cardView, ICardPM cardPM, Cell targetCell)
        {
            if (targetCell == null) 
                return;
            
            Invocation invocation = GetInvocation(cardPM, targetCell);
            targetCell.SetInvocation(invocation);
            
            InvocationSpawnedEvent?.Invoke();
        }

        private Vector3 GetSpawnPosition(Cell targetCell) => targetCell.transform.position + Vector3.up * 0.5f;
    }
}