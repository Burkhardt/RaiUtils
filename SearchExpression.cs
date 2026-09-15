using System;
using System.Collections.Generic;
using System.Text;

// Copyright © 2026 Rainer Burkhardt
// Licensed under the Apache License, Version 2.0
namespace RaiUtils
{
    /// <summary>
    /// Simple search expression containing '*', '+' and ' ' and other whitespace characters
    /// </summary>
    public class SearchExpression
    {
        static char[] VariableSplitter = new char[] { '=' };
        static char[] ConditionSeperator = new char[] { ' ' };
        static char[] Wildcard = new char[] { '*' };
        static char[] AndSplitter = new char[] { '+' };
        private List<string> conditions;
        public string ConditionsAsString
        {
            get
            {
                var result = new StringBuilder();
                string[] varCond;
                foreach (var c in conditions)
                {
                    if (result.Length > 0)
                        result.Append(" ");
                    varCond = c.Split(VariableSplitter);
                    if (varCond.Length == 2)
                        if (c.Contains(" "))
                            result.Append($"{varCond[0]}=\"{varCond[1]}\"");
                        else result.Append($"{varCond[0]}={varCond[1]}");
                    else if (varCond.Length == 1)
                        if (c.Contains(" "))
                            result.Append($"\"{c}\"");
                        else result.Append(c);
                }
                return result.ToString();
            }
            set
            {
                // Problems debugging this!
                if (value == null)
                    conditions = new List<string>();
                else
                {
                    var array = value.ToCharArray();
                    var len = array.Length;
                    conditions = new List<string>();
                    var s = new StringBuilder();
                    for (int i = 0; i < len; i++)
                    {
                        if (array[i] == '"')
                            while (array[++i] != '"' && i < len)
                                s.Append(array[i]);
                        else if (char.IsWhiteSpace(array[i]) || array[i] == ',' || array[i] == '+')
                        {
                            if (s.Length > 0)
                                conditions.Add((string)s.ToString());
                            s = new StringBuilder();
                        }
                        else s.Append(array[i]);
                    }
                    if (s.Length > 0)
                        conditions.Add(s.ToString());
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="PlusSeperatedString"></param>
        /// <returns></returns>
        private static bool Contains(string Value, string PlusSeperatedString)
        {
            var allMatch = true;
            foreach (var s in PlusSeperatedString.Split(AndSplitter, StringSplitOptions.RemoveEmptyEntries))
                allMatch = allMatch && Value.Contains(s);
            return allMatch;
        }
        #region unused code
        /*
        /// <summary>a replacement for string.Contains</summary>
        /// <param name="value"></param>
        /// <param name="token"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <remarks>https://kevinmontrose.com An Optimization Exercise
        /// I don't think I really need it here - I just wanted to open up a path to speeding (unsafe/pointer)
        /// </remarks>
        public static bool ContainsTokenMonty(string value, string token, char delimiter = ';')
        {
        	const int charsPerLong = sizeof(long) / sizeof(char);
        	const int charsPerInt = sizeof(int) / sizeof(char);
        	const int bytesPerChar = sizeof(char) / sizeof(byte);

        	if (string.IsNullOrEmpty(token)) return false;
        	if (string.IsNullOrEmpty(value)) return false;

        	var delimiterTwice = (delimiter << 16) | delimiter;

        	var valueLength = value.Length;
        	var tokenLength = token.Length;

        	if (tokenLength > valueLength) return false;

        	int tokenLongs;
        	bool tokenTrailingInt, tokenTrailingChar;
        	{
        		var remainingChars = tokenLength;
        		tokenLongs = remainingChars / charsPerLong;
        		tokenTrailingInt = (tokenLength & 0x02) != 0;
        		tokenTrailingChar = (tokenLength & 0x01) != 0;
        	}

        	var tokenByteLength = tokenLength * bytesPerChar;

        	unsafe
        	{
        		fixed (char* valuePtr = value, tokenPtr = token)
        		{
        			var curValuePtr = valuePtr;
        			var endValuePtr = valuePtr + valueLength;

        			while (true)
        			{
        				long* tokenLongPtr = (long*)tokenPtr;
        				{
        					for (var i = 0; i < tokenLongs; i++)
        					{
        						var tokenLong = *tokenLongPtr;

        						var valueLong = *((long*)curValuePtr);

        						if (tokenLong == valueLong)
        						{
        							tokenLongPtr++;
        							curValuePtr += charsPerLong;
        							continue;
        						}
        						else
        						{
        							goto advanceToNextDelimiter;
        						}
        					}
        				}

        				int* tokenIntPtr = (int*)tokenLongPtr;
        				if (tokenTrailingInt)
        				{
        					var tokenInt = *tokenIntPtr;

        					var valueInt = *((int*)curValuePtr);

        					if (tokenInt == valueInt)
        					{
        						tokenIntPtr++;
        						curValuePtr += charsPerInt;
        					}
        					else
        					{
        						goto advanceToNextDelimiter;
        					}
        				}

        				char* tokenCharPtr = (char*)tokenIntPtr;
        				if (tokenTrailingChar)
        				{
        					var tokenChar = *tokenCharPtr;

        					var valueChar = *curValuePtr;

        					if (tokenChar == valueChar)
        					{
        						tokenCharPtr++;
        						curValuePtr++;
        					}
        					else
        					{
        						goto advanceToNextDelimiter;
        					}
        				}

        				if (curValuePtr == endValuePtr || *curValuePtr == delimiter)
        				{
        					return true;
        				}

        				advanceToNextDelimiter:

        				while (true)
        				{
        					var curVal = *((int*)curValuePtr);

        					var masked = curVal ^ delimiterTwice;
        					var temp = masked & 0x7FFF7FFF;
        					temp = temp + 0x7FFF7FFF;
        					temp = (int)(temp & 0x80008000);
        					temp = temp | masked;
        					temp = temp | 0x7FFF7FFF;
        					temp = ~temp;
        					var neitherMatch = temp == 0;

        					if (neitherMatch)
        					{
        						curValuePtr += charsPerInt;
        						if (curValuePtr >= endValuePtr)
        						{
        							return false;
        						}
        						continue;
        					}

        					var top16 = temp & 0xFFFF0000;
        					if (top16 != 0)
        					{
        						curValuePtr += charsPerInt;

        						break;
        					}

        					var bottom16 = temp & 0x0000FFFF;
        					if (bottom16 != 0)
        					{
        						curValuePtr += 1;
        					}
        				}

        				var remainingBytesInValue = ((byte*)endValuePtr) - ((byte*)curValuePtr);
        				if (remainingBytesInValue < tokenByteLength)
        				{
        					return false;
        				}
        			}
        		}
        	}
        }
        */
        #endregion
        private static bool IsMatch(string Value, string searchExpressionWithWildcards)
        {
            string[] symbols;
            var match = true;
            symbols = searchExpressionWithWildcards.Split(Wildcard);
            if (symbols.Length >= 3)
                match = match && Value.StartsWith(symbols[0]) && Contains(Value, symbols[1]) && Value.EndsWith(symbols[symbols.Length - 1]);
            else if (symbols.Length == 2)
                match = match && Value.StartsWith(symbols[0]) && Value.EndsWith(symbols[1]);
            else if (symbols.Length == 1)
                match = match && Contains(Value, symbols[0]);
            return match;
        }
        /// <summary>
        /// compares to a passed in object by trying to find the pattern in any property of the object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>search can be limited to a field by setting the correct field name, e.g. Email=*hse24*</remarks>
        public bool IsMatch(object obj)
        {
            var match = true;
            //PropertyInfo[] properties;
            string[] parts;
            var Value = "";
            object p;
            foreach (var condition in conditions)
            {
                #region extract field name if given
                parts = condition.Split(VariableSplitter, StringSplitOptions.RemoveEmptyEntries);
                #endregion
                #region try to find match in the one field given
                if (parts.Length == 2)
                {
                    try
                    {
                        Value = obj.GetType().GetProperty(parts[0]).GetValue(obj, null).ToString();
                        // parts[1] can contain wildcards and +
                        match = match && !string.IsNullOrEmpty(Value) && IsMatch(Value, parts[1]);
                    }
                    catch (Exception)
                    {
                        match = false;
                    }
                }
                #endregion
                #region try to find match in any field
                else if (parts.Length == 1)
                {
                    var matchesAnyField = false;
                    foreach (var property in obj.GetType().GetProperties())
                    {
                        try
                        {
                            if ((p = property.GetValue(obj, null)) != null)
                            {
                                Value = p.ToString();
                                if (!string.IsNullOrEmpty(Value) && IsMatch(Value, parts[0]))
                                {
                                    matchesAnyField = true;
                                    break;
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                    match = match && matchesAnyField;
                }
                #endregion
            }
            return match;
        }
        /// <summary>
        /// Parse the given expression or pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <example>name=Rainer* deleted+2012 deleted=false
        /// means: 
        /// a) find every object where the value of the property name starts with Rainer
        /// b) check if the JSON-Representation contains deleted and 2012
        /// c) check if the value of the field deleted contains the string false
        /// if all yes: IsMatch evaluates to true, else false</example>
        /// <remarks>known problems:
        /// Escaping of special characters like &lt;, &gt;, /, =, +
        /// uppercase/lowercase for fieldnames
        /// captions not necessarily match the field names (especially in localizations)
        /// 2-step-processing (ParsePattern, IsMatch) might not work with LinqDataSource
        /// </remarks>
        public SearchExpression(string pattern)
        {
            ConditionsAsString = pattern;
        }
    }
}