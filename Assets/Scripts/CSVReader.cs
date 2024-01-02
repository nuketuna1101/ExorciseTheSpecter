using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CSVReader
{
    // CSV Parsing REGEX 관련
    static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static readonly char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        // Resources에 저장된 CSV 파일로부터 파싱. 
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    public static int GetLinesLength(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        return lines.Length;
    }
    public static int GetIntValue(List<Dictionary<string, object>> _list, int index, string name)
    {
        return int.Parse(_list[index][name].ToString());
    }
    public static float GetFloatValue(List<Dictionary<string, object>> _list, int index, string name)
    {
        return float.Parse(_list[index][name].ToString());
    }
    public static string GetString(List<Dictionary<string, object>> _list, int index, string name)
    {
        return (_list[index][name].ToString());
    }
}