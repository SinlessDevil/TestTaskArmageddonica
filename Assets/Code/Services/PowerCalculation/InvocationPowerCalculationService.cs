using System.Collections.Generic;
using System.Linq;
using Code.Services.IInvocation.StaticData;
using Code.Services.LevelConductor;
using Code.Services.StaticData;
using Code.StaticData;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;

namespace Code.Services.PowerCalculation
{
    public class InvocationPowerCalculationService : IPowerCalculationService
    {
        private readonly ILevelConductor _levelConductor;
        private readonly IStaticDataService _staticDataService;
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        
        public InvocationPowerCalculationService(
            ILevelConductor levelConductor,
            IStaticDataService staticDataService,
            IInvocationStaticDataService invocationStaticDataService)
        {
            _levelConductor = levelConductor;
            _staticDataService = staticDataService;
            _invocationStaticDataService = invocationStaticDataService;
        }
        
        public float CalculatePlayerPower()
        {
            var playerInvocations = _levelConductor.GetPlayerInvocations();
            return CalculateTotalPower(playerInvocations);
        }
        
        public float CalculateEnemyPower()
        {
            var enemyInvocations = _levelConductor.GetEnemyInvocations();
            return CalculateTotalPower(enemyInvocations);
        }
        
        public float CalculateInvocationPower(InvocationDTO invocation)
        {
            // Пока считаем только для юнитов
            if (invocation.InvocationType != InvocationType.Unit)
                return 0f;
            
            // Получаем статические данные юнита
            var unitStaticData = GetUnitStaticData(invocation.Id);
            if (unitStaticData == null)
                return 0f;
            
            // Получаем константы из баланса
            var balance = _staticDataService.Balance;
            var multipliers = balance.UnitCharacteristicsMultiplier;
            
            // Рассчитываем силу: (атака * 3 + скорость * 1.5 + здоровье * 2) * количество
            float power = (unitStaticData.Damage * multipliers.AttackMultiplier + 
                          unitStaticData.Speed * multipliers.SpeedMultiplier + 
                          unitStaticData.Health * multipliers.HealthMultiplier) * invocation.Quantity;
            
            return power;
        }
        
        public BattleResult ComparePowers()
        {
            float playerPower = CalculatePlayerPower();
            float enemyPower = CalculateEnemyPower();
            
            const float tolerance = 0.01f; // Допуск для ничьи
            
            if (System.Math.Abs(playerPower - enemyPower) < tolerance)
                return BattleResult.Draw;
            
            return playerPower > enemyPower ? BattleResult.Player : BattleResult.Enemy;
        }
        
        private float CalculateTotalPower(Dictionary<string, InvocationDTO> invocations)
        {
            float totalPower = 0f;
            
            foreach (var invocation in invocations.Values)
            {
                totalPower += CalculateInvocationPower(invocation);
            }
            
            return totalPower;
        }
        
        private UnitStaticData GetUnitStaticData(string unitId)
        {
            var invocationData = _invocationStaticDataService.GetInvocationData(unitId);
            return invocationData as UnitStaticData;
        }
    }
}