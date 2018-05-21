﻿using ObjectManager.Rest.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObjectManager.Rest.V2.Models
{
    internal class RestObject
    {
        public RestObject(int artifactId) => this.ArtifactId = artifactId;
        public int ArtifactId { get; set; }
    }

    internal class RelativityObjectRestReadPrep
    {
        internal class RequestObj
        {
            public RestObject Object { get; set; }
            public IEnumerable<object> Fields { get; set; }
        }


        public RequestObj Request { get; set; }

        public static RelativityObjectRestReadPrep Prep(RelativityObject obj)
        {
            var parser = new RestFieldParser();
            var ret = new RelativityObjectRestReadPrep();
            ret.Request = new RequestObj();
            ret.Request.Object = new RestObject(obj.ArtifactId);
            var fields = obj?.FieldValues?.Where(x => x.Field != null).Select(x => parser.Parse(x.Field)).ToList();
            if (!(fields?.Any() ?? false))
            {
                fields = new List<RField> { new RField.NameRestField("Artifact Id") };
            }
            ret.Request.Fields = fields;
            return ret;
        }
    }
}