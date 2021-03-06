﻿using System.Collections.Generic;
using Net.RichardLord.Ash.Core;

namespace  Net.RichardLord.AshTests.Core
{
    class MockFamily : IFamily
    {
        public static List<MockFamily> Instances = new List<MockFamily>();

        public static void Reset()
        {
            Instances = new List<MockFamily>();
        }

        public int NewEntityCalls { get; set; }
        public int RemoveEntityCalls { get; set; }
        public int ComponentAddedCalls { get; set; }
        public int ComponentRemovedCalls { get; set; }
        public int CleanUpCalls { get; set; }
    
        public void Setup(IGame game, System.Type nodeType)
        {
            Instances.Add(this);
        }

        public NodeList NodeList { get { return null; } }

        public void NewEntity(Entity entity) { NewEntityCalls++; }

        public void RemoveEntity(Entity entity) { RemoveEntityCalls++; }

        public void ComponentAddedToEntity(Entity entity, System.Type componentClass)
        {
            ComponentAddedCalls ++;
        }

        public void ComponentRemovedFromEntity(Entity entity, System.Type componentClass)
        {
            ComponentRemovedCalls++;
        }

        public void CleanUp() { CleanUpCalls++; }
    }
}

