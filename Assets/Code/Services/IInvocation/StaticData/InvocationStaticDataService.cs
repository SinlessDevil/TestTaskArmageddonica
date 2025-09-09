using System.Linq;
using System.Collections.Generic;
using Code.StaticData.Invocation.Data;
using UnityEngine;

namespace Code.Services.IInvocation.StaticData
{
    public class InvocationStaticDataService : IInvocationStaticDataService
    {
        private Dictionary<string, InvocationStaticData> _invocationData;

        public void LoadData()
        {
            _invocationData = Resources.LoadAll<InvocationStaticData>("StaticData/Invocation")
                .ToDictionary(x => x.Id, x => x);
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
    }
}