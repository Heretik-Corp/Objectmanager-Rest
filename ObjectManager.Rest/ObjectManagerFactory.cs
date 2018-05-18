using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Authentication;
using ObjectManager.Rest.Legacy;
using ObjectManager.Rest.V1;
using ObjectManager.Rest.V2;
using Relativity.API;
using System;
using System.Threading.Tasks;

namespace ObjectManager
{
    public class ObjectManagerFactory : IObjectManagerFactory
    {
        private readonly IRelativityVersionResolver _versionResolver;
        private readonly IHelper _helper;

        public ObjectManagerFactory(IRelativityVersionResolver versionResolver, IHelper helper)
        {
            _versionResolver = versionResolver;
            _helper = helper;
        }
        public virtual async Task<IObjectManager> GetObjectManagerAsync(IAuthentication authentication)
        {
            var version = await _versionResolver.GetRelativityVersionAsync();
            if (version >= new Version("9.5.287.43") || version < new Version("9.6.50.31"))
            {
                var host = GetRestUrl(_helper);
                return new ObjectManagerV1(host, authentication);
            }
            else if (version >= new Version("9.6.50.31"))
            {
                var host = GetRestUrl(_helper);
                return new ObjectManagerV2(host, authentication);
            }
            else
            {
                return new RSAPIObjectManager(_helper);
            }
        }
        private static string GetRestUrl(IHelper helper)
        {
            return helper.GetServicesManager()
                .GetRESTServiceUrl()
                .GetLeftPart(UriPartial.Authority);
        }
    }
}
