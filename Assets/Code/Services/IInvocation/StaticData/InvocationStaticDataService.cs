using System.Linq;
using System.Collections.Generic;
using Code.StaticData.Invocation.Data;
using Code.StaticData.Invocation;
using UnityEngine;

namespace Code.Services.IInvocation.StaticData
{
    public class InvocationStaticDataService : IInvocationStaticDataService
    {
        private Dictionary<string, InvocationStaticData> _invocationData;
        private InvocationCollectionStaticData _collectionData;

        public void LoadData()
        {
            _collectionData = Resources.Load<InvocationCollectionStaticData>("StaticData/Invocation/InvocationCollectionStaticData");
            if (_collectionData == null)
            {
                Debug.LogError("InvocationCollectionStaticData not found at path: StaticData/Invocation/InvocationCollectionStaticData");
                return;
            }

            _invocationData = new Dictionary<string, InvocationStaticData>();
            
            // Добавляем все Unit данные
            foreach (var unitData in _collectionData.UnitCollectionData)
            {
                _invocationData[unitData.Id] = unitData;
            }
            
            // Добавляем все Build данные
            foreach (var buildData in _collectionData.BuildCollectionData)
            {
                _invocationData[buildData.Id] = buildData;
            }
            
            // Добавляем все Skill данные
            foreach (var skillData in _collectionData.SkillCollectionData)
            {
                _invocationData[skillData.Id] = skillData;
            }
        }

        public InvocationStaticData GetInvocationData(string id)
        {
            return _invocationData.TryGetValue(id, out var data) ? data : null;
        }

        public List<InvocationStaticData> GetAllInvocations()
        {
            return _invocationData.Values.ToList();
        }

        public InvocationStaticData GetRandomInvocation()
        {
            List<InvocationStaticData> allInvocations = GetAllInvocations();
            if (allInvocations.Count == 0) 
                return null;
            return allInvocations[UnityEngine.Random.Range(0, allInvocations.Count)];
        }

        public List<UnitStaticData> GetUnitCollection()
        {
            return _collectionData?.UnitCollectionData ?? new List<UnitStaticData>();
        }

        public List<BuildStaticData> GetBuildCollection()
        {
            return _collectionData?.BuildCollectionData ?? new List<BuildStaticData>();
        }

        public List<SkillStaticData> GetSkillCollection()
        {
            return _collectionData?.SkillCollectionData ?? new List<SkillStaticData>();
        }
    }
}