using System;
using System.Threading.Tasks;

namespace ObjectManager.Rest.Interfaces
{
    public interface IRelativityVersionResolver
    {
        Task<Version> GetRelativityVersionAsync();
    }
}
