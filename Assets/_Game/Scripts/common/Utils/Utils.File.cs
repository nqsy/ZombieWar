using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static partial class Utils
{
    public static string GetFolderParent(string path)
    {
        var info = new DirectoryInfo(path);
        return info.Parent.ToString();
    }

    public static string GetProjectPath()
    {
        return GetFolderParent(Application.dataPath);
    }

    public static bool IsExistFolder(string path)
    {
        return Directory.Exists(path);
    }

    public static void CreateFolder(string path)
    {
        Directory.CreateDirectory(path);
    }

#if UNITY_EDITOR
    public static void RemoveFolder(string path)
    {
        FileUtil.DeleteFileOrDirectory(path);
    }

    public static void CopyFolder(string pathBase, string pathCopy)
    {
        FileUtil.CopyFileOrDirectory(pathBase, pathCopy);
    }
#endif

    public static bool IsExistFile(string path)
    {
        return File.Exists(path);
    }

    public static void CreateFile(string path)
    {
        File.CreateText(path);
    }

    public static void WriteBinaryFile(string fileName, UnityAction<BinaryWriter> callback, bool isAppend = false)
    {
        try
        {
            using (BinaryWriter tw = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                callback?.Invoke(tw);
                tw.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void ReadBinaryFile(string fileName, UnityAction<BinaryReader> callback)
    {
        try
        {
            using (BinaryReader sr = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                callback?.Invoke(sr);
                sr.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void WriteFile(string fileName, UnityAction<StreamWriter> callback, bool isAppend = false)
    {
        try
        {
            using(StreamWriter tw = new StreamWriter(fileName, append: isAppend))
            {
                callback?.Invoke(tw);
                tw.Close();
            }
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void ReadFile(string fileName, UnityAction<StreamReader> callback)
    {
        try
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                callback?.Invoke(sr);
                sr.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void ReadFileToList(string fileName, UnityAction<List<string>> callback)
    {
        try
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                List<string> tempLines = new();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    tempLines.Add(line);
                }

                callback?.Invoke(tempLines);
                sr.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static string ReadTextFile(string path, bool isAbsolutePath = false)
    {
        var reader = new StreamReader(path);
        var fileContent = reader.ReadToEnd();
        reader.Close();
        return fileContent;
    }

    public static void WriteTextFile(string path, string jsonString)
    {
        WriteFile(path, (tw) =>
        {
            tw.Write(jsonString);
        });
    }
}
