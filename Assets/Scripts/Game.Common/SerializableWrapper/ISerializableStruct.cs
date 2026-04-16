namespace Game.Common.SerializableWrapper;

public interface ISerializableStruct
{
    public string ToSerializableKey();

    public void AssignFromSerializableKey(string serializableKey);
}