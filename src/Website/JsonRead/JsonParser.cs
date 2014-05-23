using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Usergroup.Sugnl.JsonReadSpike;
public abstract class JsonParser<TParsable> where TParsable : class, new()
{
    private readonly Stream json;
    private readonly string jsonPropertyName;

    public List<TParsable> Result { get; private set; }

    protected JsonParser(Stream json, string jsonPropertyName)
    {
        this.json = json;
        this.jsonPropertyName = jsonPropertyName;

        Result = new List<TParsable>();
    }

    protected abstract void Build(TParsable parsable, string objectName, JsonTextReader reader);

    protected virtual bool IsBuilt(TParsable parsable, JsonTextReader reader)
    {
        return reader.TokenType.Equals(JsonToken.None);
    }

    public void Parse(IHandler<TParsable> handler)
    {
        int count = 0;
        using (var streamReader = new StreamReader(json))
        {
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                do
                {
                    jsonReader.Read();
                    var objectName = jsonReader.Value;
                    if (objectName == null || !objectName.ToString().StartsWith(jsonPropertyName)) continue;

                    var parsable = new TParsable();

                    do
                    {
                        jsonReader.Read();
                    } while (!jsonReader.TokenType.Equals(JsonToken.PropertyName) && !jsonReader.TokenType.Equals(JsonToken.None));

                    do
                    {
                        if (jsonReader.Value != null) Build(parsable, objectName.ToString(), jsonReader);
                        jsonReader.Read();
                    } while (!IsBuilt(parsable, jsonReader));

                    count++;
                    //if (count % 100 == 0)
                    //Console.WriteLine(count);


                    handler.Process(parsable);

                } while (!jsonReader.TokenType.Equals(JsonToken.None));
            }
        }
    }
}
