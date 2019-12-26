//!!!USAGE!!!!

//SINGLETON CLASS
//public class TestManager : Singleton<TestManager>

//OTHER CLASS
//TestManager.Instance.DoSomething();

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance => instance;

    public static bool IsInitialized => instance != null;

    protected void Awake()
    {
        if (instance != null)
            Debug.LogError ($"[Singleton] Trying to instantiate a second instance of a singleton class {typeof(T).FullName}");
        if (instance == null)
            instance = (T) this;
    }
    
    protected void OnDestroy()
    {
        if (Instance == this)
            instance = null;
    }
}