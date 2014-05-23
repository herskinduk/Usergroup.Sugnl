using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Usergroup.Sugnl.JsonReadSpike;

namespace Website.JsonRead
{
    public class InstanceParser : JsonParser<Instance>
    {
        public InstanceParser(Stream json, string jsonPropertyName) : base(json, jsonPropertyName) { }

        protected override void Build(Instance parsable, string objectName, JsonTextReader reader)
        {
            parsable.ID = objectName;

            if (reader.Value.Equals("http://www.w3.org/2000/01/rdf-schema#label"))
            {
                reader.Read();
                parsable.Label = (string)reader.Value;
            }
            else if (reader.Value.Equals("http://www.w3.org/2000/01/rdf-schema#comment"))
            {
                reader.Read();
                parsable.Description = (string)reader.Value;
            }
            else if (reader.Value.Equals("http://www.georss.org/georss/point"))
            {
                reader.Read();
                parsable.Point = (string)reader.Value;
            }
            else if (reader.Value.Equals("http://www.w3.org/1999/02/22-rdf-syntax-ns#type_label"))
            {
                reader.Read();
                if (reader.TokenType == JsonToken.StartArray)
                {
                    do
                    {
                        reader.Read();
                        if (reader.TokenType == JsonToken.String)
                        {
                            parsable.Categories.Add((string)reader.Value);
                        }
                    }
                    while (reader.TokenType != JsonToken.EndArray && reader.TokenType != JsonToken.None);
                }
            }


        }

        protected override bool IsBuilt(Instance parsable, JsonTextReader reader)
        {
            var isBuilt = parsable.Label != null && parsable.Description != null && parsable.Point != null;
            return isBuilt || base.IsBuilt(parsable, reader);
        }
    }
}