using Code.Services.Invocations.StaticData;
using Code.Services.UniqueId;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.Services.Invocations.CreatorDTO
{
    public class InvocationCreatorDTOService : IInvocationCreatorDTOService
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        private readonly IUniqueIdService _uniqueIdService;

        public InvocationCreatorDTOService(
            IInvocationStaticDataService invocationStaticDataService,
            IUniqueIdService uniqueIdService)
        {
            _invocationStaticDataService = invocationStaticDataService;
            _uniqueIdService = uniqueIdService;
        }

        public InvocationDTO GetInvocationDTO(string invocationId)
        {
            var staticData = _invocationStaticDataService.GetInvocationData(invocationId);
            if (staticData == null)
            {
                Debug.LogWarning($"InvocationStaticData not found for ID: {invocationId}");
                return null;
            }

            string uniqueId = _uniqueIdService.GenerateUniqueId(staticData.InvocationType.ToString());

            return staticData.InvocationType switch
            {
                InvocationType.Unit => CreateUnitDTO(staticData as UnitStaticData, uniqueId),
                InvocationType.Build => CreateBuildDTO(staticData as BuildStaticData, uniqueId),
                InvocationType.Skill => CreateSkillDTO(staticData as SkillStaticData, uniqueId),
                _ => new InvocationDTO(staticData.Id, uniqueId, staticData.Prefab, staticData.Rank, 
                    staticData.CardDefinition, staticData.InvocationType, 1)
            };
        }

        public UnitDTO GetUnitDTO(string unitId)
        {
            var unitStaticData = _invocationStaticDataService.GetInvocationData(unitId) as UnitStaticData;
            if (unitStaticData == null)
            {
                Debug.LogWarning($"UnitStaticData not found for ID: {unitId}");
                return null;
            }

            string uniqueId = _uniqueIdService.GenerateUniqueId("Unit");
            return CreateUnitDTO(unitStaticData, uniqueId);
        }

        public BuildDTO GetBuildingDTO(string buildingId)
        {
            var buildingStaticData = _invocationStaticDataService.GetInvocationData(buildingId) as BuildStaticData;
            if (buildingStaticData == null)
            {
                Debug.LogWarning($"BuildStaticData not found for ID: {buildingId}");
                return null;
            }

            string uniqueId = _uniqueIdService.GenerateUniqueId("Building");
            return CreateBuildDTO(buildingStaticData, uniqueId);
        }

        public SkillDTO GetSkillDTO(string skillId)
        {
            var skillStaticData = _invocationStaticDataService.GetInvocationData(skillId) as SkillStaticData;
            if (skillStaticData == null)
            {
                Debug.LogWarning($"SkillStaticData not found for ID: {skillId}");
                return null;
            }

            string uniqueId = _uniqueIdService.GenerateUniqueId("Skill");
            return CreateSkillDTO(skillStaticData, uniqueId);
        }
        
        private UnitDTO CreateUnitDTO(UnitStaticData unitStaticData, string uniqueId)
        {
            return new UnitDTO(unitStaticData.Id, uniqueId, unitStaticData.Prefab, unitStaticData.Rank, 
                unitStaticData.CardDefinition, unitStaticData.InvocationType, 1,
                unitStaticData.Health, unitStaticData.Damage, unitStaticData.Speed);
        }
        
        private BuildDTO CreateBuildDTO(BuildStaticData buildStaticData, string uniqueId)
        {
            return new BuildDTO(buildStaticData.Id, uniqueId, buildStaticData.Prefab, buildStaticData.Rank, 
                buildStaticData.CardDefinition, buildStaticData.InvocationType, 1,
                buildStaticData.Defense, buildStaticData.Damage, buildStaticData.Skill);
        }
        
        private SkillDTO CreateSkillDTO(SkillStaticData skillStaticData, string uniqueId)
        {
            return new SkillDTO(skillStaticData.Id, uniqueId, skillStaticData.Prefab, skillStaticData.Rank, 
                skillStaticData.CardDefinition, skillStaticData.InvocationType, 1,
                skillStaticData.Skill);
        }
    }
}
