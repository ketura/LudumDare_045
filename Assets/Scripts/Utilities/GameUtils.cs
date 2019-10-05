using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static GameObject GetGameObjectRoot(Transform transform)
    {
        while(transform.parent != null)
        {
            transform = transform.parent;
        }

        return transform.gameObject;
    }

    public static GameObject GetGameObjectRoot(Component component)
    {
        return GetGameObjectRoot(component.transform);
    }

    public static GameObject GetGameObjectRoot(GameObject gameObject)
    {
        return GetGameObjectRoot(gameObject.transform);
    }

    public static bool GameObjectExistsInParentChain(Transform transform, GameObject objectToCheckExistenceOf)
    {
        do
        {
            if (transform.gameObject == objectToCheckExistenceOf) return true;
            transform = transform.parent;
        }
        while (transform != null);

        return false;
    }

    public static bool GameObjectExistsInParentChain(Component component, GameObject objectToCheckExistenceOf)
    {
        return GameObjectExistsInParentChain(component.transform, objectToCheckExistenceOf);
    }

    public static bool GameObjectExistsInParentChain(GameObject baseGameObject, GameObject objectToCheckExistenceOf)
    {
       return GameObjectExistsInParentChain(baseGameObject.transform, objectToCheckExistenceOf);
    }

    public static bool AnyGameObjectExistsInParentChain(Transform transform, GameObject[] objectsToCheckExistenceOf)
    {
        do
        {
            foreach (GameObject gameObject in objectsToCheckExistenceOf)
            {
                if (transform.gameObject == gameObject) return true;
            }
            transform = transform.parent;
        }
        while (transform != null);

        return false;
    }

    public static bool AnyGameObjectExistsInParentChain(Component component, GameObject[] objectsToCheckExistenceOf)
    {
        return AnyGameObjectExistsInParentChain(component.transform, objectsToCheckExistenceOf);
    }

    public static bool AnyGameObjectExistsInParentChain(GameObject gameObject, GameObject[] objectsToCheckExistenceOf)
    {
        return AnyGameObjectExistsInParentChain(gameObject.transform, objectsToCheckExistenceOf);
    }

    public static Transform FindDeepChildWithName(Transform root, string name)
    {
        if (root.name == name) return root;

        foreach (Transform child in root)
        {
            Transform perfectChild = FindDeepChildWithName(child, name);
            if (perfectChild != null) return perfectChild;
        }

        return null;
    }

    public static void ShuffleGameObjects(GameObject[] array)
    {
        for (int i = array.Length-1; i > 0; i--)
        {
            int j = Random.Range(0, i);
            GameObject obj = array[i];
            array[i] = array[j];
            array[j] = obj;
        }
    }
}
