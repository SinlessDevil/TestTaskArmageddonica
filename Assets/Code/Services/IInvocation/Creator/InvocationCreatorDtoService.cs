using Code.Services.IInvocation.StaticData;
using Code.Services.UniqueId;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;
using UnityEngine;

namespace Code.Services.IInvocation.Creator
{
    public class InvocationCreatorDtoService : IInvocationCreatorDTOService
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;
        private readonly IUniqueIdService _uniqueIdService;

        public InvocationCreatorDtoService(
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
                InvocationType.Unit => new UnitDTO(staticData.Id, uniqueId, staticData.Prefab, staticData.Rank, staticData.CardDefinition, staticData.InvocationType, 1),
                InvocationType.Build => new BuildingDTO(staticData.Id, uniqueId, staticData.Prefab, staticData.Rank, staticData.CardDefinition, staticData.InvocationType, 1),
                InvocationType.Skill => new SkillDTO(staticData.Id, uniqueId, staticData.Prefab, staticData.Rank, staticData.CardDefinition, staticData.InvocationType, 1),
                _ => new InvocationDTO(staticData.Id, uniqueId, staticData.Prefab, staticData.Rank, staticData.CardDefinition, staticData.InvocationType, 1)
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
            return new UnitDTO(unitStaticData.Id, uniqueId, unitStaticData.Prefab, unitStaticData.Rank, unitStaticData.CardDefinition, unitStaticData.InvocationType, 1);
        }

        public BuildingDTO GetBuildingDTO(string buildingId)
        {
            var buildingStaticData = _invocationStaticDataService.GetInvocationData(buildingId) as BuildStaticData;
            if (buildingStaticData == null)
            {
                Debug.LogWarning($"BuildStaticData not found for ID: {buildingId}");
                return null;
            }

            string uniqueId = _uniqueIdService.GenerateUniqueId("Building");
            return new BuildingDTO(buildingStaticData.Id, uniqueId, buildingStaticData.Prefab, buildingStaticData.Rank, buildingStaticData.CardDefinition, buildingStaticData.InvocationType, 1);
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
            return new SkillDTO(skillStaticData.Id, uniqueId, skillStaticData.Prefab, skillStaticData.Rank, skillStaticData.CardDefinition, skillStaticData.InvocationType, 1);
        }
    }
}
