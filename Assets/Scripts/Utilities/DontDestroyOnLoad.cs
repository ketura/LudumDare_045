using UnityEngine;
using System.Collections;

namespace Utilities
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        // Use this for initialization
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}