using Code.Logic.Grid;
using Code.Services.IInvocation.DTO;
using Code.Services.IInvocation.StaticData;
using UnityEngine;

namespace Code.Services.IInvocation.Factories
{
    public class InvocationFactory : IInvocationFactory
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;

        public InvocationFactory(IInvocationStaticDataService invocationStaticDataService)
        {
            _invocationStaticDataService = invocationStaticDataService;
        }

        public GameObject CreateInvocation(InvocationDTO invocationDTO, Cell targetCell)
        {
            var staticData = _invocationStaticDataService.GetInvocationData(invocationDTO.Id);
            if (staticData?.Prefab == null) return CreateFallbackObject(invocationDTO.Name, targetCell);
            
            var spawnPosition = GetSpawnPosition(targetCell);
            return Object.Instantiate(staticData.Prefab, spawnPosition, Quaternion.identity);
        }

        public GameObject CreateUnit(UnitDTO unitDTO, Cell targetCell)
        {
            return CreateInvocation(unitDTO, targetCell);
        }

        public GameObject CreateBuilding(BuildingDTO buildingDTO, Cell targetCell)
        {
            return CreateInvocation(buildingDTO, targetCell);
        }

        public GameObject CreateSkill(SkillDTO skillDTO, Cell targetCell)
        {
            return CreateInvocation(skillDTO, targetCell);
        }

        private GameObject CreateFallbackObject(string name, Cell targetCell)
        {
            var spawnPosition = GetSpawnPosition(targetCell);
            var fallbackObject = new GameObject($"Invocation_{name}");
            fallbackObject.transform.position = spawnPosition;
            return fallbackObject;
        }

        private Vector3 GetSpawnPosition(Cell targetCell)
        {
            return targetCell.transform.position + Vector3.up * 0.5f;
        }
    }
}