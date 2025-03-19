using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public static class SaveLoadManager
{
    private const string preferenceDatafileName = "/preference.dat";
    private const string economyDatafileName = "/economy.dat";


    #region PREFERENCE_DATA
    public static void SavePreference(PreferenceData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + preferenceDatafileName, FileMode.Create);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static PreferenceData LoadPreferenceData()
    {
        if (File.Exists(Application.persistentDataPath + preferenceDatafileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + preferenceDatafileName, FileMode.Open);
            PreferenceData data = bf.Deserialize(stream) as PreferenceData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.LogError("Saved PreferenceData Not Found! Returning NULL!!");
            return null;
        }

    }
    #endregion

    #region ECONOMY_DATA
    public static void SaveEconomyData(EconomyData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + economyDatafileName, FileMode.Create);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static EconomyData LoadEconomyData()
    {
        if (File.Exists(Application.persistentDataPath + economyDatafileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + economyDatafileName, FileMode.Open);
            EconomyData data = bf.Deserialize(stream) as EconomyData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.LogError("Saved EconomyData Not Found! Returning NULL!!");
            return null;
        }

    }
    #endregion

}