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
                // �������е�ʵ��
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    // �����µ�GameObject�����ӵ������
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";

                    // ȷ���������󲻻��ڼ����³���ʱ������
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
                // ���ʵ���Ѿ������Ҳ��ǵ�ǰʵ���������ٵ�ǰ����
                Destroy(gameObject);
            }
        }
    }
}
