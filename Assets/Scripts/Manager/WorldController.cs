using System;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private static WorldController instance;

    private void Awake()
    {
        instance = this;
    }
    
    public PlayerController GetPlayerController()
    {
        return playerController;
    }
    
    public static WorldController GetInstance()
    {
        return instance;
    }
}
