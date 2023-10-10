using Friflo.Fliox.Engine.ECS;
using NUnit.Framework;
using Tests.Utils;
using static NUnit.Framework.Assert;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable InconsistentNaming
namespace Tests.ECS;


public static class Test_ArchetypeMask
{
    [Test]
    public static void Test_ArchetypeMask_basics()
    {
        var twoStructs = ArchetypeMask.Get<Position, Rotation>();
        AreEqual("Mask: [Position, Rotation]", twoStructs.ToString());
        
        var structs    = new ArchetypeMask();
        AreEqual("Mask: []",                    structs.ToString());
        IsFalse(structs.Has<Position>());
        IsFalse(structs.HasAll(twoStructs));
        IsFalse(structs.HasAny(twoStructs));
        
        structs.Add<Position>();
        IsTrue (structs.Has<Position>());
        IsFalse(structs.HasAll(twoStructs));
        IsTrue (structs.HasAny(twoStructs));
        
        AreEqual("Mask: [Position]",            structs.ToString());
        
        structs.Add<Rotation>();
        AreEqual("Mask: [Position, Rotation]", structs.ToString());
        IsTrue (structs.Has<Position, Rotation>());
        IsTrue (structs.HasAll(twoStructs));
        IsTrue (structs.HasAny(twoStructs));

        var copy = new ArchetypeMask();
        copy.Add(structs);
        AreEqual("Mask: [Position, Rotation]", copy.ToString());
        
        copy.Remove<Position>();
        AreEqual("Mask: [Rotation]",           copy.ToString());
        
        copy.Remove(structs);
        AreEqual("Mask: []",                    copy.ToString());
        
        var store   = new EntityStore();
        var entity  = store.CreateEntity();
        
        // AreEqual("Mask: []", entity.Tags.ToString());
    }
    
    [Test]
    public static void Test_ArchetypeMask_Get()
    {
        var schema          = EntityStore.GetComponentSchema();
        var testStructType  = schema.ComponentTypeByType[typeof(Position)];
        var testStructType2  = schema.ComponentTypeByType[typeof(Rotation)];
        
        var struct1    = ArchetypeMask.Get<Position>();
        AreEqual("Mask: [Position]", struct1.ToString());
        int count1 = 0;
        foreach (var structType in struct1) {
            AreSame(testStructType, structType);
            count1++;
        }
        AreEqual(1, count1);
        
        var count2 = 0;
        var struct2 = ArchetypeMask.Get<Position, Rotation>();
        AreEqual("Mask: [Position, Rotation]", struct2.ToString());
        foreach (var _ in struct2) {
            count2++;
        }
        AreEqual(2, count2);
        
        AreEqual(struct2, ArchetypeMask.Get<Position, Rotation>());
    }
    
    [Test]
    public static void Test_ArchetypeMask_Get_Mem()
    {
        var struct1    = ArchetypeMask.Get<Position>();
        foreach (var _ in struct1) { }
        
        // --- 1 struct
        var start   = Mem.GetAllocatedBytes();
        int count1 = 0;
        foreach (var _ in struct1) {
            count1++;
        }
        Mem.AssertNoAlloc(start);
        AreEqual(1, count1);
        
        // --- 2 structs
        start       = Mem.GetAllocatedBytes();
        var struct2 = ArchetypeMask.Get<Position, Rotation>();
        var count2 = 0;
        foreach (var _ in struct2) {
            count2++;
        }
        Mem.AssertNoAlloc(start);
        AreEqual(2, count2);
    }
    
    [Test]
    public static void Test_ArchetypeMask_lookup_Perf()
    {
        var store = new EntityStore();
        var type1 = store.GetArchetype(Signature.Get<Position>());
        var type2 = store.GetArchetype(Signature.Get<Position, Rotation>());
        var type3 = store.GetArchetype(Signature.Get<Position, Rotation, Scale3>());
        
        var id   = type1.id;
        IsTrue(store.TryGetArchetypeId(id, out var archetypeId));
        AreSame(type1, archetypeId.type);
        
        var start   = Mem.GetAllocatedBytes();
        var count = 10; // 100_000_000 ~ 1.089 ms
        for (int n = 0; n < count; n++)
        {
            store.TryGetArchetypeId(id, out _);
        }
        Mem.AssertNoAlloc(start);
    }
}

