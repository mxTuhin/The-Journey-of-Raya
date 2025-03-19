using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    
    
    //NOTE: Save System
    private PreferenceData preferenceData;
    private EconomyData economyData;


    private int initialCoinAmount = 100;
    private int initialDiamondAmount = 100;
    
    [SerializeField] private bool isDebug;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
            return;
        }

        InitializeSavedData();

        //#if !UNITY_EDITOR
        //#endif
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.targetFrameRate = 60;
        }
    }

    private void InitializeSavedData()
    {
        //Load preference Data
        preferenceData = SaveLoadManager.LoadPreferenceData();
        if (preferenceData == null)
        {
            //Load data is NULL, initialize with Default data and save
            preferenceData = new PreferenceData(true, true, true);
            SaveLoadManager.SavePreference(preferenceData);
        }
        
        //Load Economy Data
        economyData = SaveLoadManager.LoadEconomyData();
        if (economyData == null)
        {
            //Load data is NULL, initialize with Default data and save
            if (isDebug)
            {
                initialCoinAmount = 1000000;
                initialDiamondAmount = 10000;
            }

            economyData = new EconomyData(initialCoinAmount, initialDiamondAmount);
            SaveLoadManager.SaveEconomyData(economyData);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region PreferenceData
    
    public PreferenceData GetPreferenceData()
    {
        return preferenceData;
    }
    
    public void SavePreferenceData(PreferenceData data)
    {
        preferenceData = data;
        SaveLoadManager.SavePreference(preferenceData);
    }
    
    #endregion
    
    #region EconomyData
    
    public EconomyData GetEconomyData()
    {
        return economyData;
    }
    
    public void UpdateCoinAmount(int amount)
    {
        economyData.coinCount += amount;
        SaveLoadManager.SaveEconomyData(economyData);
    }
    
    public void UpdateDiamondAmount(int amount)
    {
        economyData.diamondCount += amount;
        SaveLoadManager.SaveEconomyData(economyData);
    }
    
    public void SaveEconomyData(EconomyData data)
    {
        economyData = data;
        SaveLoadManager.SaveEconomyData(economyData);
    }
    
    #endregion
    
    public static GameManager GetInstance()
    {
        return instance;
    }
}
