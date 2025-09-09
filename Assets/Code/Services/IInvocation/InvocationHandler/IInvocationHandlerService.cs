using System;

namespace Code.Services.IInvocation.InvocationHandler
{
    public interface IInvocationHandlerService
    {
        event Action InvocationSpawnedEvent;
        void Initialize();
        void Dispose();
    }
}