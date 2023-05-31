using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonDataExtension
{
    /// <summary>
    /// Get the string value of this json. Returns null if data is null.
    /// </summary>
    /// <param name="jsonData"></param>
    /// <returns>Returns string value or null if data is null.</returns>
    public static string GetString(this JsonData jsonData)
    {
        if (jsonData == null)
        {
            // print error
            UnityEngine.Debug.LogWarning("Data is null");
            return null;
        }
        else
        {
            return (string)jsonData;
        }
    }

    /// <summary>
    /// Get the int value of this json. Returns -1 if data is null.
    /// </summary>
    /// <param name="jsonData"></param>
    /// <returns> Returns int value or -1 if data is null.</returns>
    public static int GetInt(this JsonData jsonData)
    {
        if (jsonData == null)
        {
            // print error
            UnityEngine.Debug.LogWarning("Data is null");
            return -1;
        }
        else
        {
            //int val = Convert.ToInt32(jsonData.ToString());

            //if (Int32.TryParse((string)jsonData, out int value))
            //{
            //    return value;
            //}
            //else
            //{
            //    UnityEngine.Debug.LogWarning("Unable to parse data");
            //    return -1;
            //}
            return (int)jsonData;
        }
    }

    /// <summary>
    /// Gets the value of the jsonData object at the specified key
    /// </summary>
    /// <param name="jsonData"></param>
    /// <param name="key"></param>
    /// <returns>Returns value else returns null.</returns>
    public static JsonData Get(this JsonData jsonData, string key)
    {
        if (jsonData == null || !jsonData.IsObject)
        { 
            return null;
        }
        
        return jsonData.TryGetValue(key);
    }
}

