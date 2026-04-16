using SaintsField;
using SaintsField.Playa;
using System;
using UnityEditor;
using UnityEngine;

namespace Game.Common.SerializableWrapper;

[Serializable]
public sealed class SerializableEnumWrapper<TEnum>
    where TEnum : Enum
{
    [SerializeField, ReadOnly] private string _key;

    [ShowInInspector]
    public TEnum Value
    {
        get
        {
            if (string.IsNullOrEmpty(_key))
                _key = default(TEnum).ToString();
            return (TEnum)Enum.Parse(typeof(TEnum), _key);
        }
        set
        {
            _key = value.ToString();
#if UNITY_EDITOR
            EditorUtility.SetDirty(Selection.activeObject);
#endif
        }
    }
}