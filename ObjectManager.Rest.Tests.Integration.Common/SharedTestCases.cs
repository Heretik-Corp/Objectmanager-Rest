using ObjectManager.Rest.Interfaces;
using ObjectManager.Rest.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjectManager.Rest.Tests.Integration.Common
{
    public static class SharedTestCases
    {
        public static RelativityObject CreateTestObject(int artifactId, FieldRef field, object value)
        {
            var obj = new Interfaces.RelativityObject
            {
                ArtifactId = artifactId,
                FieldValues = new List<FieldValuePair>
                {
                    new FieldValuePair
                    {
                        Field = field,
                        Value = value
                    }
                }
            };
            return obj;
        }
        public static async Task<(ObjectUpdateResult, RelativityObject)> RunUpdateTestAsync(IObjectManager manager, int workspaceId, int documentId, FieldRef field, object value)
        {
            var obj = CreateTestObject(documentId, field, value);
            var uResult = await manager.UpdateAsync(workspaceId, obj, null);
            var result = await manager.ReadAsync(workspaceId, obj, null);
            return (uResult, result);
        }
    }
}
