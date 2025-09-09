using Code.Logic.Grid;
using Code.Services.Input.Card;
using Code.Services.IInvocation.StaticData;
using Code.UI.Game.Cards;
using Code.UI.Game.Cards.View;
using Services.Contex;
using UnityEngine;
using Zenject;

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

        public void Initialize()
        {
            _cardInputService.DroppedOnCell += OnCardDroppedOnCell;
        }

        public void Cleanup()
        {
            _cardInputService.DroppedOnCell -= OnCardDroppedOnCell;
        }

        private void OnCardDroppedOnCell(CardView cardView, Cell targetCell)
        {
            if (targetCell == null) return;
            SpawnInvocation(cardView, targetCell);
        }

        public void SpawnInvocation(CardView cardView, Cell targetCell)
        {
            var spawnPosition = GetSpawnPosition(targetCell);
            var spawnedObject = new GameObject($"Invocation_{cardView.name}");
            spawnedObject.transform.position = spawnPosition;
        }

        private Vector3 GetSpawnPosition(Cell targetCell)
        {
            return targetCell.transform.position + Vector3.up * 0.5f;
        }
    }
}