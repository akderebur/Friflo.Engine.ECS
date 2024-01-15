﻿// Copyright (c) Ullrich Praetz. All rights reserved.
// See LICENSE file in the project root for full license information.

// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Friflo.Engine.ECS;

public readonly struct Signal<TEvent> where TEvent : struct 
{
    public readonly     EntityStore Store;
    public readonly     int         EntityId;
    public readonly     TEvent      Event;
    
    public              Entity      Entity => new Entity(Store, EntityId);
    
    internal Signal(EntityStore store, int id, in TEvent ev) {
        Store       = store;
        EntityId    = id;
        Event       = ev;
    }

    // e.g. "entity: 1 - signal > MyEvent"
    public override string ToString() => $"entity: {Entity.Id} - signal > {typeof(TEvent).Name}";
}