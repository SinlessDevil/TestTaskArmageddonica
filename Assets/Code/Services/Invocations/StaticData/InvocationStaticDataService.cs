using System.Collections.Generic;
using System.Linq;
using Code.StaticData.Invocation;
using Code.StaticData.Invocation.Data;
using UnityEngine;

namespace Code.Services.Invocations.StaticData
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
            
            foreach (var unitData in _collectionData.UnitCollectionData) 
                _invocationData[unitData.Id] = unitData;
            
            foreach (var buildData in _collectionData.BuildCollectionData) 
                _invocationData[buildData.Id] = buildData;
            
            foreach (var skillData in _collectionData.SkillCollectionData) 
                _invocationData[skillData.Id] = skillData;
        }

        public InvocationStaticData GetInvocationData(string id) => 
            _invocationData.TryGetValue(id, out var data) ? data : null;

        public List<InvocationStaticData> GetAllInvocations() => 
            _invocationData.Values.ToList();

        public InvocationStaticData GetRandomInvocation()
        {
            List<InvocationStaticData> allInvocations = GetAllInvocations();
            if (allInvocations.Count == 0) 
                return null;
            return allInvocations[UnityEngine.Random.Range(0, allInvocations.Count)];
        }

        public List<UnitStaticData> GetUnitCollection() => 
            _collectionData?.UnitCollectionData ?? new List<UnitStaticData>();

        public List<BuildStaticData> GetBuildCollection() => 
            _collectionData?.BuildCollectionData ?? new List<BuildStaticData>();

        public List<SkillStaticData> GetSkillCollection() => 
            _collectionData?.SkillCollectionData ?? new List<SkillStaticData>();
    }
}