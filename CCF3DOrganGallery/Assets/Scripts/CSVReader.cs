// CSVReader to read csv data. Code taken from: https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class CsvReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static readonly char[] TrimChars = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();

        var lines = Regex.Split(file, LINE_SPLIT_RE);

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
                value = value.TrimStart(TrimChars).TrimEnd(TrimChars).Replace("\\", "");
                object finalValue = value;
                if (int.TryParse(value, out var n))
                {
                    finalValue = n;
                }
                else if (float.TryParse(value, out var f))
                {
                    finalValue = f;
                }

                entry[header[j]] = finalValue;
            }

            list.Add(entry);
        }

        return list;
    }
}