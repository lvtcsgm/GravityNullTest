using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class INIWorker
{
    private static string path = Path.Combine("", "SCRAPS.ini");
    private static Dictionary<string, Dictionary<string, string>> IniDictionary = new Dictionary<string, Dictionary<string, string>>();
    private static bool Initialized = false;
    /// <summary>
    /// Sections list
    /// </summary>
    public enum Sections
    {
        Version,
        Credits
    }
    /// <summary>
    /// Keys list
    /// </summary>
    public enum Keys
    {
        value1,
        value2,
        value3,
        value4,
        value5,
        value6,
        value7,
        value8,
        value9,
        value10,
        value11,
        value12,
        value13,
        value14,
        value15,
        value16,
        value17,
        value18,
        value19,
        value20
    }

    private static bool FirstRead()
    {
        if (File.Exists(path))
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                string theSection = "";
                string theKey = "";
                string theValue = "";
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    line.Trim();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        theSection = line.Substring(1, line.Length - 2);
                    }
                    else
                    {
                        string[] ln = line.Split(new char[] { '=' });
                        theKey = ln[0].Trim();
                        theValue = ln[1].Trim();
                    }
                    if (theSection == "" || theKey == "" || theValue == "")
                        continue;
                    PopulateIni(theSection, theKey, theValue);
                }
            }
        }
        else
        {
            using (StreamWriter sw = new StreamWriter("SCRAPS.ini"))
            {
                sw.WriteLine("[Version]");
                sw.WriteLine("value1 = 5.3.0");
                sw.WriteLine("value2 = " + Application.unityVersion);
                sw.WriteLine("[Credits]");
                sw.WriteLine("value1 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value2 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value3 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value4 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value5 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value6 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value7 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value8 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value9 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value10 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value11 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value12 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value13 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value14 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value15 = FIRSTNAME LASTNAME - JOB TITLE");
                sw.WriteLine("value16 = FIRSTNAME LASTNAME - JOB TITLE");
            }

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                string theSection = "";
                string theKey = "";
                string theValue = "";
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    line.Trim();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        theSection = line.Substring(1, line.Length - 2);
                    }
                    else
                    {
                        string[] ln = line.Split(new char[] { '=' });
                        theKey = ln[0].Trim();
                        theValue = ln[1].Trim();
                    }
                    if (theSection == "" || theKey == "" || theValue == "")
                        continue;
                    PopulateIni(theSection, theKey, theValue);
                }
            }
        }
        return true;
    }

    private static void PopulateIni(string _Section, string _Key, string _Value)
    {
        if (IniDictionary.ContainsKey(_Section))
        {
            if (IniDictionary[_Section].ContainsKey(_Key))
                IniDictionary[_Section][_Key] = _Value;
            else
                IniDictionary[_Section].Add(_Key, _Value);
        }
        else {
            Dictionary<string, string> neuVal = new Dictionary<string, string>();
            neuVal.Add(_Key.ToString(), _Value);
            IniDictionary.Add(_Section.ToString(), neuVal);
        }
    }
    /// <summary>
    /// Write data to INI file. Section and Key no in enum.
    /// </summary>
    /// <param name="_Section"></param>
    /// <param name="_Key"></param>
    /// <param name="_Value"></param>
    public static void IniWriteValue(string _Section, string _Key, string _Value)
    {
        if (!Initialized)
            FirstRead();
        PopulateIni(_Section, _Key, _Value);
        //write ini
        WriteIni();
    }
    /// <summary>
    /// Write data to INI file. Section and Key bound by enum
    /// </summary>
    /// <param name="_Section"></param>
    /// <param name="_Key"></param>
    /// <param name="_Value"></param>
    public static void IniWriteValue(Sections _Section, Keys _Key, string _Value)
    {
        IniWriteValue(_Section.ToString(), _Key.ToString(), _Value);
    }

    private static void WriteIni()
    {
        using (StreamWriter sw = new StreamWriter(path))
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> sezioni in IniDictionary)
            {
                sw.WriteLine("[" + sezioni.Key.ToString() + "]");
                foreach (KeyValuePair<string, string> chiave in sezioni.Value)
                {
                    // value must be in one line
                    string vale = chiave.Value.ToString();
                    vale = vale.Replace(Environment.NewLine, " ");
                    vale = vale.Replace("\r\n", " ");
                    sw.WriteLine(chiave.Key.ToString() + " = " + vale);
                }
            }
        }
    }
    /// <summary>
    /// Read data from INI file. Section and Key bound by enum
    /// </summary>
    /// <param name="_Section"></param>
    /// <param name="_Key"></param>
    /// <returns></returns>
    public static string IniReadValue(Sections _Section, Keys _Key)
    {
        if (!Initialized)
            FirstRead();
        return IniReadValue(_Section.ToString(), _Key.ToString());
    }
    /// <summary>
    /// Read data from INI file. Section and Key no in enum.
    /// </summary>
    /// <param name="_Section"></param>
    /// <param name="_Key"></param>
    /// <returns></returns>
    public static string IniReadValue(string _Section, string _Key)
    {
        if (!Initialized)
            FirstRead();
        if (IniDictionary.ContainsKey(_Section))
            if (IniDictionary[_Section].ContainsKey(_Key))
                return IniDictionary[_Section][_Key];
        return null;
    }
}