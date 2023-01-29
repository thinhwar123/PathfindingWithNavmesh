using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class NavMeshMaskAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NavMeshMaskAttribute))]
public class NavMeshMaskDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
    {
        EditorGUI.PrefixLabel(position.AlignLeft(EditorGUIUtility.labelWidth), label);

        EditorGUI.BeginChangeCheck();

        string[] areaNames = GameObjectUtility.GetNavMeshAreaNames().Where(value => value != "").ToArray();
        string[] completeAreaNames = new string[areaNames.Length];

        foreach (string name in areaNames)
        {
            completeAreaNames[GameObjectUtility.GetNavMeshAreaFromName(name)] = name;
        }

        int mask = serializedProperty.intValue;

        mask = EditorGUI.MaskField(position.AlignRight(position.width - EditorGUIUtility.labelWidth), mask, completeAreaNames);
        if (EditorGUI.EndChangeCheck())
        {
            serializedProperty.intValue = mask;
        }
    }
}

public static class NavMeshMaskAttributeExtensions
{
    public static Rect AlignLeft(this Rect rect, float width)
    {
        rect.width = width;
        return rect;
    }
    public static Rect AlignRight(this Rect rect, float width)
    {
        rect.x = rect.x + rect.width - width;
        rect.width = width;
        return rect;
    }
}
#endif