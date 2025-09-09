using System.Collections.Generic;

namespace Code.Services.Providers.CardComposites
{
    public interface ICardCompositeProvider
    {
        /// <summary>
        /// Создает указанное количество CardComposite с рандомизированными юнитами
        /// </summary>
        /// <param name="count">Количество карт для создания</param>
        /// <returns>Список созданных CardComposite</returns>
        List<CardComposite> CreateRandomUnitCards(int count);
        
        /// <summary>
        /// Возвращает CardComposite в пул
        /// </summary>
        /// <param name="cardComposite">CardComposite для возврата</param>
        void ReturnCardComposite(CardComposite cardComposite);
        
        /// <summary>
        /// Возвращает список CardComposite в пул
        /// </summary>
        /// <param name="cardComposites">Список CardComposite для возврата</param>
        void ReturnCardComposites(List<CardComposite> cardComposites);
    }
}