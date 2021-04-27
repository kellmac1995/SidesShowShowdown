using UnityEngine;
/// <summary>
/// http://www.unitygeek.com/unity_c_singleton/
/// Set base.isPersistent to true if you wish the gameobject persists between scenes. (DontDestroyOnLoad)
/// </summary>
/// <typeparam name="T"></typeparam>
public class GenericSingletonClass<T> : MonoBehaviour where T : Component
{

    //[HideInInspector]
    public bool isPersistent = false;

    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();

                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if(isPersistent)
                DontDestroyOnLoad(this.gameObject);
        }
        else if(_instance != this)
        {
            Debug.Log("Duplicate Singleton");
            Destroy(gameObject);
        }
    }
}
