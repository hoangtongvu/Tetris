using Game.Common.SerializableWrapper;
using System;
using UnityEditor;
using UnityEngine;

namespace Game.UI.Common;

public partial struct UIAssetId
{
    [Serializable]
    public sealed class SerializableWrapper : SerializableWrapper<UIAssetId> { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UIAssetId.SerializableWrapper))]
    internal sealed class SerializableWrapperDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var guiStyle = new GUIStyle(EditorStyles.foldout)
            {
                richText = true,
            };
            var keyProp = property.FindPropertyRelative("_key");
            label.text = $"{label.text}: [<i><color=#888888>{keyProp.stringValue}</color></i>]";

            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true, guiStyle);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                UIAssetId value = default;
                value.AssignFromSerializableKey(keyProp.stringValue);

                var line = position;
                line.y += line.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();

                value.Type = (UIType)EditorGUI.EnumPopup(line, nameof(value.Type), value.Type);
                line.y += line.height + EditorGUIUtility.standardVerticalSpacing;
                value.VariantIndex = (byte)EditorGUI.IntField(line, nameof(value.VariantIndex), value.VariantIndex);

                if (EditorGUI.EndChangeCheck())
                    keyProp.stringValue = value.ToSerializableKey();

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
        }
    }
#endif
}