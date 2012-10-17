using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using NUnit.Framework;
using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.AshTests.Core
{
    [TestFixture]
	public class ComponentMatchingFamilyTests
	{
		private IGame _game;
		private IFamily _family;
		
		[SetUp]
		public void CreateFamily()    
		{
			_game = new Game<ComponentMatchingFamily>();
		    _family = new ComponentMatchingFamily();
            _family.Setup(_game, typeof(MockNode));
		}

    	[TearDown]
		public void ClearFamily()    
		{
			_family = null;
			_game = null;
		}
		
		[Test]
		public void TestNodeListIsInitiallyEmpty()    
		{
			var nodes = _family.NodeList;
			Assert.IsNull(nodes.Head);
		}
		
		[Test]
		public void TestMatchingEntityIsAddedWhenAccessNodeListFirst()    
		{
			var nodes = _family.NodeList;
			var entity = new Entity();
			entity.Add(new Point());
			_family.NewEntity(entity);
			Assert.AreSame(entity, nodes.Head.Entity);
		}

        [Test]
        public void TestMatchingEntityIsAddedWhenAccessNodeListSecond()
        {
            var entity = new Entity();
            entity.Add(new Point());
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            Assert.AreSame(entity, nodes.Head.Entity);
        }
		
        [Test]
        public void TestNodeContainsEntityProperties()    
        {
            var entity = new Entity();
            var point = new Point(1,2);
            entity.Add(point);
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            Assert.AreEqual(point, ((MockNode)nodes.Head).Point);
        }

        [Test]
        public void TestMatchingEntityIsAddedWhenComponentAdded()
        {
            var nodes = _family.NodeList;
            var entity = new Entity();
            entity.Add(new Point());
            _family.ComponentAddedToEntity(entity, typeof(Point));
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestNonMatchingEntityIsNotAdded()
        {
            var entity = new Entity();
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestNonMatchingEntityIsNotAddedWhenComponentAdded()
        {
            var entity = new Entity();
            entity.Add(new Matrix());
            _family.ComponentAddedToEntity(entity, typeof(Matrix));
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityIsRemovedWhenAccessNodeListFirst()
        {
            var entity = new Entity();
            entity.Add(new Point());
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            _family.RemoveEntity(entity);
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityIsRemovedWhenAccessNodeListSecond()
        {
            var entity = new Entity();
            entity.Add(new Point());
            _family.NewEntity(entity);
            _family.RemoveEntity(entity);
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityIsRemovedWhenComponentRemoved()
        {
            var entity = new Entity();
            entity.Add(new Point());
            _family.NewEntity(entity);
            entity.Remove(typeof(Point));
            _family.ComponentRemovedFromEntity(entity, typeof(Point));
            var nodes = _family.NodeList;
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void NodeListContainsOnlyMatchingEntities()
        {
            var entities = new List<Entity>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new Entity();
                entity.Add(new Point());
                entities.Add(entity);
                _family.NewEntity(entity);
                _family.NewEntity(new Entity());
            }
			
            var nodes = _family.NodeList;
            var results = new List<bool>();
            for(var node = nodes.Head; node != null; node = node.Next)
            {
                results.Add(entities.Contains(node.Entity));
            }

            Assert.AreEqual(new List<bool> {true, true, true, true, true}, results);
        }

        [Test]
        public void NodeListContainsAllMatchingEntities()
        {
            var entities = new List<Entity>();
            for(var i = 0; i < 5; ++i)
            {
                var entity = new Entity();
                entity.Add(new Point());
                entities.Add(entity);
                _family.NewEntity(entity);
                _family.NewEntity(new Entity());
            }

            var nodes = _family.NodeList;
            for (var node = nodes.Head; node != null; node = node.Next)
            {
                entities.RemoveAt(entities.IndexOf(node.Entity));
            }
            Assert.AreEqual(0, entities.Count);
        }

        [Test]
        public void CleanUpEmptiesNodeList()
        {
            var entity = new Entity();
            entity.Add(new Point());
            _family.NewEntity(entity);
            var nodes = _family.NodeList;
            _family.CleanUp();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void CleanUpSetsNextNodeToNull()
        {
            var entities = new List<Entity>();
            for(var i = 0; i < 5; ++i)
            {
                var entity = new Entity();
                entity.Add(new Point());
                entities.Add(entity);
                _family.NewEntity(entity);
            }
			
            var nodes = _family.NodeList;
            var node = nodes.Head.Next;
            _family.CleanUp();
            Assert.IsNull(node.Next);
        }
    
        class MockNode : Node
        {
            public Point Point { get; set; }
        }
    }
}

