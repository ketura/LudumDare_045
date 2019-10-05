using UnityEngine;
using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class ListUtils
    {
        public static List<U> Map<T, U>(this List<T> list, Func<T, U> func)
        {
            var result = new List<U>();

            foreach (T value in list)
            {
                result.Add(func(value));
            }

            return result;
        }

        public static float GetLength(this List<Vector2> list)
        {
            if (list.Count == 0)
            {
                return 0f;
            }
            else
            {
                float length = 0f;
                var point1 = list[0];

                for (int i = 1; i < list.Count; i++)
                {
                    var point2 = list[i];
                    length += Vector2.Distance(point1, point2);
                    point1 = point2;
                }

                return length;
            }
        }

        public static Vector2 GetPointAlong(this List<Vector2> list, float totalDistance)
        {
            if (list == null)
            {
                Debug.LogError("list == null");
                return Vector2.zero;
            }

            for (int index = 0; index < list.Count - 1; index++)
            {
                var point1 = list[index];
                var point2 = list[index + 1];
                var distance = Vector2.Distance(point1, point2);

                if (totalDistance < distance)
                {
                    return Vector2.Lerp(point1, point2, totalDistance / distance);
                }
                else
                {
                    totalDistance -= distance;
                }
            }

            return list[list.Count - 1];
        }

        public static Vector2 GetFinalEndpoint(this List<Vector2> list)
        {
            return list[list.Count - 1];
        }

        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
