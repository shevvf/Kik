using System.Linq;

using UnityEngine;

public static class ComponentExtensions
{
    public static T FindAnyComponent<T>() where T : Component
    {
        T[] allComponents = Resources.FindObjectsOfTypeAll<T>();
        return allComponents.FirstOrDefault();
    }
}
