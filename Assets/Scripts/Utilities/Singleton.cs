using UnityEngine;

namespace Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();

                    if (instance == null)
                    {
                        Debug.LogErrorFormat("Couldn't find an instance of {0} in the scene.", typeof(T).FullName);
                    }
                    else
                    {
                        instance.Initialize();
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                Initialize();
            }
            else if (instance != this)
            {
                Debug.LogFormat("An instance of {0} already exists in the scene.", typeof(T).FullName);
                Destroy(gameObject);
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}
