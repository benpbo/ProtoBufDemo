namespace ProtoBufDemo.Abstractions
{
    public interface ISerializer<TFrom, TTo>
    {
        TTo Serialize(TFrom value);

        TFrom Deserialize(TTo value);
    }
}
