using System;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour    
{     
    private static T _instance; //Accessing "Instance"
    public static T Instance        
    {            
        get            
        {
            _instance = FindFirstObjectByType<T>();

            if (_instance == null)               
            {
                //Creates an instance if not existing (to avoid inspector references), more professional
                //GameObject singletonObject = new GameObject(typeof(T).Name);                    
                //_instance = singletonObject.AddComponent<T>();   
                Debug.LogWarning($"You're missing your singleton {typeof(T).Name}");
            }
                
            return _instance;
        }
    }    

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = this as T;

        else if (_instance != this)
            Destroy(gameObject);
        
        //DontDestroyOnLoad(this);
    }
}
