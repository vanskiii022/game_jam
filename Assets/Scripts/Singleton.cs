using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 查找现有的实例
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    // 创建新的GameObject并附加单例组件
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";

                    // 确保单例对象不会在加载新场景时被销毁
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (_instance != this)
            {
                // 如果实例已经存在且不是当前实例，则销毁当前对象
                Destroy(gameObject);
            }
        }
    }
}
