using System;
using Code.Logic.Cell;
using Code.Logic.Invocations.Builds;
using Code.Logic.Invocations.Skills;
using Code.Logic.Invocations.Units;
using Code.Services.Invocations.StaticData;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation.DTO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Services.Invocations.Factories
{
    public class InvocationFactory : IInvocationFactory
    {
        private readonly IInvocationStaticDataService _invocationStaticDataService;

        public InvocationFactory(IInvocationStaticDataService invocationStaticDataService)
        {
            _invocationStaticDataService = invocationStaticDataService;
        }

        public Logic.Invocations.Invocation CreateInvocationByType(InvocationDTO dto, Cell targetCell, Quaternion rotation)
        {
            Logic.Invocations.Invocation invocation;
            
            switch (dto.InvocationType)
            {
                case InvocationType.Unit:
                    invocation = CreateUnit((UnitDTO)dto, targetCell, rotation);
                    break;
                case InvocationType.Build:
                    invocation = CreateBuilding((BuildDTO)dto, targetCell, rotation);
                    break;
                case InvocationType.Skill:
                    invocation = CreateSkill((SkillDTO)dto, targetCell, rotation);
                    break;
                case InvocationType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            invocation.Initialize(dto.UniqueId);

            return invocation;
        }
        
        public Logic.Invocations.Invocation CreateInvocationBase(InvocationDTO invocationDTO, Cell targetCell, Quaternion rotation)
        {
            InvocationStaticData staticData = _invocationStaticDataService.GetInvocationData(invocationDTO.Id);
            Vector3 spawnPosition = GetSpawnPosition(targetCell);
            return Object.Instantiate(staticData.Prefab, spawnPosition, rotation).GetComponent<Logic.Invocations.Invocation>();
        }

        public Unit CreateUnit(UnitDTO unitDTO, Cell targetCell, Quaternion rotation)
        {
            Unit unit = CreateInvocationBase(unitDTO, targetCell, rotation).GetComponent<Unit>();
            unit.Initialize();
            unit.transform.SetParent(targetCell.transform);
            return unit;
        }

        public Build CreateBuilding(BuildDTO buildDto, Cell targetCell, Quaternion rotation)
        {
            Build build = CreateInvocationBase(buildDto, targetCell, rotation).GetComponent<Build>();
            build.transform.SetParent(targetCell.transform);
            return build;
        }

        public Skill CreateSkill(SkillDTO skillDTO, Cell targetCell, Quaternion rotation) => 
            CreateInvocationBase(skillDTO, targetCell, rotation).GetComponent<Skill>();
        
        private Vector3 GetSpawnPosition(Cell targetCell) => targetCell.transform.position;
    }
}