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
    public class InvocationInvocationPowerCalculationService : IInvocationPowerCalculationService
    {
        private const float Tolerance = 0.01f;
        
        private readonly ILevelConductor _levelConductor;
        private readonly IStaticDataService _staticDataService;
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        
        public InvocationInvocationPowerCalculationService(
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
            Dictionary<string, InvocationDTO> playerInvocations = _levelConductor.GetPlayerInvocations();
            return CalculateTotalPower(playerInvocations);
        }
        
        public float CalculateEnemyPower()
        {
            Dictionary<string, InvocationDTO> enemyInvocations = _levelConductor.GetEnemyInvocations();
            return CalculateTotalPower(enemyInvocations);
        }
        
        public float CalculateInvocationPower(InvocationDTO invocation)
        {
            if (invocation.InvocationType != InvocationType.Unit)
                return 0f;
            
            UnitStaticData unitStaticData = GetUnitStaticData(invocation.Id);
            if (unitStaticData == null)
                return 0f;
            
            BalanceStaticData balance = _staticDataService.Balance;
            UnitCharacteristicsMultiplier multipliers = balance.UnitCharacteristicsMultiplier;
            
            float power = (unitStaticData.Damage * multipliers.AttackMultiplier + 
                          unitStaticData.Speed * multipliers.SpeedMultiplier + 
                          unitStaticData.Health * multipliers.HealthMultiplier) * invocation.Quantity;
            
            return power;
        }
        
        public BattleResult ComparePowers()
        {
            float playerPower = CalculatePlayerPower();
            float enemyPower = CalculateEnemyPower();
            
            if (System.Math.Abs(playerPower - enemyPower) < Tolerance)
                return BattleResult.Draw;
            
            return playerPower > enemyPower ? BattleResult.Player : BattleResult.Enemy;
        }
        
        private float CalculateTotalPower(Dictionary<string, InvocationDTO> invocations) => invocations.Values.Sum(invocation => CalculateInvocationPower(invocation));

        private UnitStaticData GetUnitStaticData(string unitId)
        {
            InvocationStaticData invocationData = _invocationStaticDataService.GetInvocationData(unitId);
            return invocationData as UnitStaticData;
        }
    }
}