using UnityEngine;

public interface IItem
{
    int ID { get; }
    EItemGrade Grade { get; }
    bool TypeEquals(IItem other);
}
