using Game.Code.StageCreation;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomPropertyDrawer(typeof(StageData.StageItemData))]
    public class StageItemDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            EditorGUI.LabelField(position,
                property.FindPropertyRelative("id").stringValue
                + " "
                + property.FindPropertyRelative("coord").vector2IntValue.ToString()
            );
        }
    }
}