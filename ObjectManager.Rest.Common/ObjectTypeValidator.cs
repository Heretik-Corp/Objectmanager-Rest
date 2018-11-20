using System;
using ObjectManager.Rest.Interfaces;

namespace ObjectManager.Rest
{
    public static class ObjectTypeValidator
    {
        public static void ValidateObjectTypeForCreate(RelativityObject obj)
        {
            if (obj.ObjectType == null || obj.ObjectType.ArtifactTypeId == 0)
            {
                throw new ArgumentException(ObjectManager.Rest.Properties.Messages.Object_Type_Missing);
            }
            if (obj.ObjectType.ArtifactTypeId == 10)
            {
                throw new NotSupportedException(ObjectManager.Rest.Properties.Messages.Document_Type_Not_Supported_For_Create);
            }
        }
    }
}
