using Relativity.API;
using System;

namespace ObjectManager.Rest.Tests.Integration.Common.Extensions
{
    public static class HelperExtensions
    {
        public static string GetRestUrl(this IHelper helper)
        {
            return helper.GetServicesManager().GetRESTServiceUrl().GetLeftPart(UriPartial.Authority);
        }
    }
}
