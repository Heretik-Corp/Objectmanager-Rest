using System;
using System.Threading.Tasks;

namespace ObjectManager
{
    public interface IRelativityVersionResolver
    {
        Task<Version> GetRelativityVersionAsync();
    }
}
