using System.Collections.Generic;
using System.Linq;
using Code.Services.Invocations.StaticData;
using Code.Services.LocalProgress;
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
        
        private readonly ILevelLocalProgressService _levelLocalProgressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        
        public InvocationInvocationPowerCalculationService(
            ILevelLocalProgressService levelLocalProgressService,
            IStaticDataService staticDataService,
            IInvocationStaticDataService invocationStaticDataService)
        {
            _levelLocalProgressService = levelLocalProgressService;
            _staticDataService = staticDataService;
            _invocationStaticDataService = invocationStaticDataService;
        }
        
        public float CalculatePlayerPower()
        {
            Dictionary<string, InvocationDTO> playerInvocations = _levelLocalProgressService.GetPlayerInvocations();
            return CalculateTotalPower(playerInvocations);
        }
        
        public float CalculateEnemyPower()
        {
            Dictionary<string, InvocationDTO> enemyInvocations = _levelLocalProgressService.GetEnemyInvocations();
            return CalculateTotalPower(enemyInvocations);
        }
        
        public float CalculateInvocationPower(InvocationDTO invocation)
        {
            BalanceStaticData balance = _staticDataService.Balance;
            
            return invocation.InvocationType switch
            {
                InvocationType.Unit => CalculateUnitPower(invocation, balance),
                InvocationType.Build => CalculateBuildingPower(invocation, balance),
                _ => 0f
            };
        }
        
        private float CalculateUnitPower(InvocationDTO invocation, BalanceStaticData balance)
        {
            if (invocation is not UnitDTO unitDTO)
                return 0f;
            
            UnitCharacteristicsMultiplier multipliers = balance.UnitCharacteristicsMultiplier;
            
            float power = (unitDTO.Damage * multipliers.AttackMultiplier + 
                          unitDTO.Speed * multipliers.SpeedMultiplier + 
                          unitDTO.Health * multipliers.HealthMultiplier) * invocation.Quantity;
            
            return power;
        }
        
        private float CalculateBuildingPower(InvocationDTO invocation, BalanceStaticData balance)
        {
            if (invocation is not BuildDTO buildDTO)
                return 0f;
            
            BuildingCharacteristicsMultiplier multipliers = balance.BuildingCharacteristicsMultiplier;
            
            float power = (buildDTO.Defense * multipliers.DefenseMultiplier + 
                          buildDTO.Damage * multipliers.AttackMultiplier) * invocation.Quantity;
            
            return power;
        }
        
        public BattlResult ComparePowers()
        {
            float playerPower = CalculatePlayerPower();
            float enemyPower = CalculateEnemyPower();
            
            if (System.Math.Abs(playerPower - enemyPower) < Tolerance)
                return BattlResult.Draw;
            
            return playerPower > enemyPower ? BattlResult.Player : BattlResult.Enemy;
        }
        
        private float CalculateTotalPower(Dictionary<string, InvocationDTO> invocations) => 
            invocations.Values.Sum(invocation => CalculateInvocationPower(invocation));

        private UnitStaticData GetUnitStaticData(string unitId)
        {
            InvocationStaticData invocationData = _invocationStaticDataService.GetInvocationData(unitId);
            return invocationData as UnitStaticData;
        }
        
        private BuildStaticData GetBuildStaticData(string buildId)
        {
            InvocationStaticData invocationData = _invocationStaticDataService.GetInvocationData(buildId);
            return invocationData as BuildStaticData;
        }
    }
}