using System;
using System.Globalization;
using UnityEngine;

public static partial class Utils
{
    public static float StringToFloat(this string val)
    {
        try
        {
            return float.Parse(val, CultureInfo.InvariantCulture);
        }
        catch
        {
            throw new Exception($"{GetStringException()} float '{val}'");
        }
    }

    public static double StringToDouble(this string val)
    {
        try
        {
            return double.Parse(val, CultureInfo.InvariantCulture);
        }
        catch
        {
            throw new Exception($"{GetStringException()} double '{val}'");
        }
    }

    public static int StringToInt(this string val, bool isIgoreEx = false)
    {
        if(isIgoreEx && IsCanIgore(val))
        {
            return 0;
        }

        try
        {
            return int.Parse(val, CultureInfo.InvariantCulture);
        }
        catch
        {
            throw new Exception($"{GetStringException()} int '{val}'");
        }
    }

    public static long StringToLong(this string val)
    {
        try
        {
            return long.Parse(val, CultureInfo.InvariantCulture);
        }
        catch
        {
            throw new Exception($"{GetStringException()} long '{val}'");
        }
    }

    public static bool StringToBool(this string val)
    {
        try
        {
            return bool.Parse(val);
        }
        catch
        {
            throw new Exception($"{GetStringException()} boolean '{val}'");
        }
    }

    public static Vector3 StringToVector3(this string str)
    {
        var c = str.Split("=");
        if (c.Length < 3)
            throw new Exception("Error convert string to vector");

        var x = StringToFloat(c[0]);
        var y = StringToFloat(c[1]);
        var z = StringToFloat(c[2]);

        return new Vector3(x, y, z);
    }

    public static T StringToEnum<T>(this string val) where T : struct
    {
        try
        {
            T ret;
            Enum.TryParse(val, out ret);
            return ret;
        }
        catch
        {
            throw new Exception($"{GetStringException()} enum '{val}'");
        }
    }

    public static T IntToEnum<T>(this int val) where T : struct
    {
        try
        {
            T ret = (T)Enum.ToObject(typeof(T), val);
            return ret;
        }
        catch
        {
            throw new Exception($"{GetIntException()} enum '{val}'");
        }
    }

    public static DateTime StringToDateTime(this string val)
    {
        try
        {
            return DateTime.Parse(val, CultureInfo.InvariantCulture);
        }
        catch
        {
            throw new Exception($"{GetStringException()} dateTime '{val}'");
        }
    }

    public static string GetStringException()
    {
        return $"[Parse Data] error parse string to";
    }

    public static string GetIntException()
    {
        return $"[Parse Data] error parse int to";
    }

    static bool IsCanIgore(string val)
    {
        if(string.IsNullOrEmpty(val)) return true;
        if(string.IsNullOrWhiteSpace(val)) return true;    

        return false;
    }

    public static string DoubleToString(this double val)
    {
        return val.ToString(CultureInfo.InvariantCulture);
    }

    public static string CharToString(this char val)
    {
        return val.ToString(CultureInfo.InvariantCulture);
    }

    public static string Vector3ToString(this Vector3 val)
    {
        return $"{val.x}={val.y}={val.z}";
    }

    public static string GetFormatTime(this TimeSpan val, bool isIncludeSecond = true)
    {
        var day = val.Days;
        var hour = val.Hours;
        var minute = val.Minutes;
        var second = val.Seconds;

        var strSecond = isIncludeSecond ? $"{second}s" : "";
        if (day > 0)
        {
            return $"{day}d {hour}h {minute}m {strSecond}";
        }

        if (hour > 0)
        {
            return $"{hour}h {minute}m {strSecond}";
        }

        return $"{minute}m {strSecond}";
    }

    public static Vector2 GetPosCenter(this Vector2 val1, Vector2 val2)
    {
        var x = (val1.x + val2.x) / 2f;
        var y = (val1.y + val2.y) / 2f;
        var ret = new Vector2(x, y);
        return ret;
    }

    public static Vector2 GetPosCenter(this Vector2Int val1, Vector2Int val2)
    {
        var x = (val1.x + val2.x) / 2f;
        var y = (val1.y + val2.y) / 2f;
        var ret = new Vector2(x, y);
        return ret;
    }

    public static bool IsAMax(this int a, int b)
    {
        var valMax = FindMax(a, b);
        return valMax == a;
    }

    public static bool IsAMin(this int a, int b)
    {
        var valMin = FindMin(a, b);
        return valMin == a;
    }

    public static int FindMax(int a, int b)
    {
        return (int)(((a + b) + Math.Abs(a - b)) / 2f);
    }

    public static int FindMin(int a, int b)
    {
        return (int)(((a + b) - Math.Abs(a - b)) / 2f);
    }
}
