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
                //Debug.Log("인스턴스가 널입니다.");
                T[] Find = FindObjectsOfType<T>();
                if(Find.Length > 0 )
                {
                    instance = Find[0];
                    DontDestroyOnLoad( instance );
                }
                //Debug.Log(Find.Length + "찾아본 결과 개수 확인");


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
                    //Debug.Log("생성 하겠습니다.");
                    GameObject Obj = new GameObject(typeof(T).Name);
                    instance = Obj.AddComponent<T>();
                    DontDestroyOnLoad ( instance );
                }

                //Debug.Log(instance.gameObject + "현재 인스턴스입니다.");
            }
            return instance;
        }
    }
    protected void Setting()
    {
        //Debug.Log("인스턴스가 널입니다.");
        T[] Find = FindObjectsOfType<T>();
        if (Find.Length > 0)
        {
            instance = Find[0];
            DontDestroyOnLoad(instance);
        }
        //Debug.Log(Find.Length + "찾아본 결과 개수 확인");


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
            Debug.Log("생성 하겠습니다.");
            GameObject Obj = new GameObject(typeof(T).Name);
            instance = Obj.AddComponent<T>();
            DontDestroyOnLoad(instance);
        }

        string text =  instance == null ? typeof(T).Name + "인스턴스가 없습니다." : typeof(T).Name + "인스턴스가 있습니다.";
        Debug.Log(text);
    }
    public virtual void Init()
    {

    }
}
