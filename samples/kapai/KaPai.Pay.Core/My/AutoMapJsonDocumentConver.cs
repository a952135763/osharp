using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using AutoMapper;
using OSharp.Json;

namespace KaPai.Pay.My
{
    public static class JsonHelp
    {

        public static JsonDocument DictionaryToJsonDocument(IDictionary<string, object> source)
        {
            if (source == null)
                return null;
            var json = source.ToJsonString();
            return JsonDocument.Parse(json);
        }


        public static IDictionary<string, object> JsonDocumentToDictionary(JsonDocument source)
        {
            if (source == null) return null;
            var dictionary = new Dictionary<string, object>();
            foreach (JsonProperty jsonProperty in source.RootElement.EnumerateObject())
            {
                switch (jsonProperty.Value.ValueKind)
                {
                    case JsonValueKind.Undefined:
                        break;
                    case JsonValueKind.Object:
                        dictionary.TryAdd(jsonProperty.Name, JsonElementToObj(jsonProperty.Value));
                        break;
                    case JsonValueKind.Array:
                        dictionary.TryAdd(jsonProperty.Name, JsonElementToList(jsonProperty.Value));
                        break;
                    case JsonValueKind.String:
                        dictionary.TryAdd(jsonProperty.Name, jsonProperty.Value.GetString());
                        break;
                    case JsonValueKind.Number:
                        var str = jsonProperty.Value.ToString();
                        var arrSplit = str.Split('.');
                        dictionary.TryAdd(jsonProperty.Name,
                            arrSplit.Length == 2 ? jsonProperty.Value.GetDecimal() : jsonProperty.Value.GetInt64());
                        break;
                    case JsonValueKind.True:
                        dictionary.TryAdd(jsonProperty.Name, true);
                        break;
                    case JsonValueKind.False:
                        dictionary.TryAdd(jsonProperty.Name, false);
                        break;
                    case JsonValueKind.Null:
                        dictionary.TryAdd(jsonProperty.Name, null);
                        break;
                    default:
                        dictionary.TryAdd(jsonProperty.Name, jsonProperty.Value.GetRawText());
                        break;
                }
            }


            return dictionary;
        }

        private static object JsonElementToObj(JsonElement element, int depth = 64)
        {
            depth--;
            if (depth < 0)
            {
                return null;
            }
            MyDynamic info = new MyDynamic();
            foreach (JsonProperty jsonProperty in element.EnumerateObject())
            {
                switch (jsonProperty.Value.ValueKind)
                {
                    case JsonValueKind.Undefined:
                        break;
                    case JsonValueKind.Object:
                        info[jsonProperty.Name] = JsonElementToObj(jsonProperty.Value, depth);
                        break;
                    case JsonValueKind.Array:
                        info[jsonProperty.Name] = JsonElementToList(jsonProperty.Value);
                        break;
                    case JsonValueKind.String:
                        info[jsonProperty.Name] = jsonProperty.Value.GetString();
                        break;
                    case JsonValueKind.Number:
                        var str = jsonProperty.Value.ToString();
                        var arrSplit = str.Split('.');
                        info[jsonProperty.Name] = arrSplit.Length == 2
                            ? jsonProperty.Value.GetDecimal()
                            : jsonProperty.Value.GetInt64();
                        break;
                    case JsonValueKind.True:
                        info[jsonProperty.Name] = true;
                        break;
                    case JsonValueKind.False:
                        info[jsonProperty.Name] = false;
                        break;
                    case JsonValueKind.Null:
                        info[jsonProperty.Name] = null;
                        break;
                    default:
                        info[jsonProperty.Name] = jsonProperty.Value.GetRawText();
                        break;
                }
            }
            return info;

        }

        private static IList<object> JsonElementToList(JsonElement element, int depth = 64)
        {
            depth--;
            if (depth < 0)
            {
                return null;
            }
            var list = new List<object>();
            foreach (JsonElement jsonElement in element.EnumerateArray())
            {
                switch (jsonElement.ValueKind)
                {
                    case JsonValueKind.Undefined:
                        break;
                    case JsonValueKind.Object:
                        list.Add(JsonElementToObj(jsonElement));
                        break;
                    case JsonValueKind.Array:
                        list.Add(JsonElementToList(jsonElement, depth));
                        break;
                    case JsonValueKind.String:
                        list.Add(jsonElement.GetString());
                        break;
                    case JsonValueKind.Number:
                        var str = jsonElement.ToString();
                        var arrSplit = str.Split('.');
                        list.Add(arrSplit.Length == 2 ? jsonElement.GetDecimal() : jsonElement.GetInt64());
                        break;
                    case JsonValueKind.True:
                        list.Add(true);
                        break;
                    case JsonValueKind.False:
                        list.Add(false);
                        break;
                    case JsonValueKind.Null:
                        list.Add(null);
                        break;
                    default:
                        list.Add(jsonElement.GetRawText());
                        break;
                }
            }
            return list;
        }

        public static String JsonDocumentToString(JsonDocument source)
        {
            return source != null ? source.RootElement.GetRawText() : "";
        }

        public static JsonDocument StringToJsonDocument(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            var onjDocument = JsonDocument.Parse(str);
            return onjDocument;
        }
    }

    public class StringToJsonDocument : ITypeConverter<string, JsonDocument>
    {
        public JsonDocument Convert(string source, JsonDocument destination, ResolutionContext context)
        {
            return JsonHelp.StringToJsonDocument(source);
        }
    }


    public class JsonDocumentToString : ITypeConverter<JsonDocument, string>
    {
        public string Convert(JsonDocument source, string destination, ResolutionContext context)
        {
            return JsonHelp.JsonDocumentToString(source);
        }
    }

    public class JsonDocumentToDictionary : ITypeConverter<JsonDocument, IDictionary<string, object>>
    {
        public IDictionary<string, object> Convert(JsonDocument source, IDictionary<string, object> destination, ResolutionContext context)
        {

            return JsonHelp.JsonDocumentToDictionary(source);
        }
    }


    public class DictionaryToJsonDocument : ITypeConverter<IDictionary<string, object>, JsonDocument>
    {
        public JsonDocument Convert(IDictionary<string, object> source, JsonDocument destination, ResolutionContext context)
        {
            return JsonHelp.DictionaryToJsonDocument(source);
        }
    }

}
