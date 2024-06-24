using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour// , new
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                //Debug.Log("�ν��Ͻ��� ���Դϴ�.");
                T[] Find = FindObjectsOfType<T>();
                if(Find.Length > 0 )
                {
                    instance = Find[0];
                    DontDestroyOnLoad( instance );
                }
                //Debug.Log(Find.Length + "ã�ƺ� ��� ���� Ȯ��");


                if ( Find.Length > 1 )
                {
                    for (int i = 1; i < Find.Length; i++)
                    {
                        Destroy(Find[i].gameObject);
                    }
                }
                //Debug.Log(Find);
                if (instance == null)
                {
                    //Debug.Log("���� �ϰڽ��ϴ�.");
                    GameObject Obj = new GameObject(typeof(T).Name);
                    instance = Obj.AddComponent<T>();
                    DontDestroyOnLoad ( instance );
                }

                //Debug.Log(instance.gameObject + "���� �ν��Ͻ��Դϴ�.");
            }
            return instance;
        }
    }
    protected void Setting()
    {
        //Debug.Log("�ν��Ͻ��� ���Դϴ�.");
        T[] Find = FindObjectsOfType<T>();
        if (Find.Length > 0)
        {
            instance = Find[0];
            DontDestroyOnLoad(instance);
        }
        //Debug.Log(Find.Length + "ã�ƺ� ��� ���� Ȯ��");


        if (Find.Length > 1)
        {
            for (int i = 1; i < Find.Length; i++)
            {
                Destroy(Find[i].gameObject);
            }
        }
        //Debug.Log(Find);
        if (instance == null)
        {
            Debug.Log("���� �ϰڽ��ϴ�.");
            GameObject Obj = new GameObject(typeof(T).Name);
            instance = Obj.AddComponent<T>();
            DontDestroyOnLoad(instance);
        }

        string text =  instance == null ? typeof(T).Name + "�ν��Ͻ��� �����ϴ�." : typeof(T).Name + "�ν��Ͻ��� �ֽ��ϴ�.";
        Debug.Log(text);
    }
    public virtual void Init()
    {

    }
}
