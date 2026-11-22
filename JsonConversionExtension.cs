using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

// don't forget that Jil can do all this out of the box (I think - RSB, 20171029)

namespace RaiUtils
{

	public static class JsonConversionExtensions
	{
		public static IDictionary<string, object> ToDictionary(this JObject json)
		{
			var propertyValuePairs = json.ToObject<Dictionary<string, object>>();
			ProcessJObjectProperties(propertyValuePairs);
			ProcessJArrayProperties(propertyValuePairs);
			return propertyValuePairs;
		}

		private static void ProcessJObjectProperties(IDictionary<string, object> propertyValuePairs)
		{
			var objectPropertyNames = (from property in propertyValuePairs
												let propertyName = property.Key
												let value = property.Value
												where value is JObject
												select propertyName).ToList();

			objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject)propertyValuePairs[propertyName]));
		}

		private static void ProcessJArrayProperties(IDictionary<string, object> propertyValuePairs)
		{
			var arrayPropertyNames = (from property in propertyValuePairs
												let propertyName = property.Key
												let value = property.Value
												where value is JArray
												select propertyName).ToList();

			arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToArray((JArray)propertyValuePairs[propertyName]));
		}

		public static object[] ToArray(this JArray array)
		{
			return array.ToObject<object[]>().Select(ProcessArrayEntry).ToArray();
		}

		private static object ProcessArrayEntry(object value)
		{
			if (value is JObject)
			{
				return ToDictionary((JObject)value);
			}
			if (value is JArray)
			{
				return ToArray((JArray)value);
			}
			return value;
		}

		#region  from a 2008 publication of James Newton King
		//public static string FormatWith(this string format, object source)
		//{
		//	return FormatWith(format, null, source);
		//}

		//public static string FormatWith(this string format, IFormatProvider provider, object source)
		//{
		//	if (format == null)
		//		throw new ArgumentNullException("format");

		//	Regex r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
		//	  RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

		//	List<object> values = new List<object>();
		//	string rewrittenFormat = r.Replace(format, delegate (Match m)
		//	{
		//		Group startGroup = m.Groups["start"];
		//		Group propertyGroup = m.Groups["property"];
		//		Group formatGroup = m.Groups["format"];
		//		Group endGroup = m.Groups["end"];

		//		values.Add((propertyGroup.Value == "0")
		//		  ? source
		//		  : System.Web.UI.DataBinder.Eval(source, propertyGroup.Value));

		//		return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value
		//		  + new string('}', endGroup.Captures.Count);
		//	});

		//	return string.Format(provider, rewrittenFormat, values.ToArray());
		//}

		#endregion
	}
}
