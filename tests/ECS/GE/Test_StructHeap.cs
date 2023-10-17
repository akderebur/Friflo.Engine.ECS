using System.Collections.Generic;
using Friflo.Fliox.Engine.ECS;
using NUnit.Framework;
using static NUnit.Framework.Assert;

// ReSharper disable InconsistentNaming
namespace Tests.ECS.GE;

public static class Test_StructHeap
{
    [Test]
    public static void Test_StructHeap_increase_entity_capacity_raw()
    {
        var store       = new RawEntityStore();
        var arch1       = store.GetArchetype(Signature.Get<Position>());
        var ids         = new List<int>();
        int count       = 2; 
        for (int n = 0; n < count; n++)
        {
            var id  = store.CreateEntity(arch1);
            ids.Add(id);
            AreEqual(n + 1, arch1.EntityCount);
            AreEqual(new Position(), store.GetEntityComponentValue<Position>(id)); // Position is present & default
            store.GetEntityComponentValue<Position>(id).x = n;  
        }
        
        for (int n = 0; n < count; n++) {
            AreEqual(n, store.GetEntityComponentValue<Position>(ids[n]).x);
        }
    }
    
    [Test]
    public static void Test_StructHeap_increase_entity_capacity()
    {
        var store       = new GameEntityStore(PidType.UsePidAsId);
        var arch1       = store.GetArchetype(Signature.Get<Position>());
        var entities    = new List<GameEntity>();
        int count       = 2; 
        for (int n = 0; n < count; n++)
        {
            var entity = store.CreateEntity(arch1);
            entities.Add(entity);
            AreSame(arch1,              entity.Archetype);
            AreEqual(n + 1,             arch1.EntityCount);
            AreEqual(new Position(),    entity.Position); // // Position is present & default
            entity.Position.x = n;
        }
        for (int n = 0; n < count; n++) {
            AreEqual(n, entities[n].Position.x);
        }
    }
}

