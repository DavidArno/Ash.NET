using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using NUnit.Framework;
using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.AshTests.Core
{
	/**
	 * Tests the family class through the game class. Left over from a previous 
	 * architecture but retained because all tests shoudl still pass.
	 */
    [TestFixture]
	public class GameAndFamilyIntegrationTests
    {
        private Game<ComponentMatchingFamily> _game;
		
		[SetUp]
		public void CreateEntity()
		{
		    _game = new Game<ComponentMatchingFamily>();
		}

		[TearDown]
		public void ClearEntity()
		{
			_game = null;
		}

		[Test]
		public void TestFamilyIsInitiallyEmpty()
		{
			var nodes = _game.GetNodeList<MockNode>();
			Assert.IsNull(nodes.Head);
		}

        [Test]
        public void TestNodeContainsEntityProperties()
        {
            var entity = new Entity();
            var point = new Point();
            var matrix = new Matrix();
            entity.Add(point);
            entity.Add(matrix);
			
            var nodes = _game.GetNodeList<MockNode>();
            _game.AddEntity(entity);
            var head = (MockNode) nodes.Head;
            Assert.AreEqual(new List<object> {point, matrix}, new List<object> {head.Point, head.Matrix});
        }

        [Test]
        public void TestCorrectEntityAddedToFamilyWhenAccessFamilyFirst()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            var nodes = _game.GetNodeList<MockNode>();
            _game.AddEntity(entity);
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestCorrectEntityAddedToFamilyWhenAccessFamilySecond()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            Assert.AreSame(entity, nodes.Head.Entity);
        }

        [Test]
        public void TestCorrectEntityAddedToFamilyWhenComponentsAdded()
        {
            var entity = new Entity();
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            entity.Add(new Point());
            entity.Add(new Matrix());
            Assert.AreSame(entity, nodes.Head.Entity);
        }
		
        [Test]
        public void TestIncorrectEntityNotAddedToFamilyWhenAccessFamilyFirst()
        {
            var entity = new Entity();
            var nodes = _game.GetNodeList<MockNode>();
            _game.AddEntity(entity);
            Assert.IsNull(nodes.Head);
        }
		
        [Test]
        public void TestIncorrectEntityNotAddedToFamilyWhenAccessFamilySecond()
        {
            var entity = new Entity();
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenComponentRemovedAndFamilyAlreadyAccessed()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            entity.Remove<Point>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenComponentRemovedAndFamilyNotAlreadyAccessed()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            entity.Remove<Point>();
            var nodes = _game.GetNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenRemovedFromGameAndFamilyAlreadyAccessed()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            _game.RemoveEntity(entity);
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void TestEntityRemovedFromFamilyWhenRemovedFromGameAndFamilyNotAlreadyAccessed()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            _game.RemoveEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void FamilyContainsOnlyMatchingEntities()
        {
            var entities = new List<Entity>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new Entity();
                entity.Add(new Point());
                entity.Add(new Matrix());
                entities.Add(entity);
                _game.AddEntity(entity);
            }
			
            var nodes = _game.GetNodeList<MockNode>();
            var results = new List<bool>();
            for (var node = nodes.Head; node != null; node = node.Next)
            {
                results.Add(entities.Contains(node.Entity));
            }

            Assert.AreEqual(new List<bool> {true, true, true, true, true}, results);
        }

        [Test]
        public void FamilyContainsAllMatchingEntities()
        {
            var entities = new List<Entity>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new Entity();
                entity.Add(new Point());
                entity.Add(new Matrix());
                entities.Add(entity);
                _game.AddEntity(entity);
            }
			
            var nodes = _game.GetNodeList<MockNode>();
            for (var node = nodes.Head; node != null; node = node.Next)
            {
                var index = entities.IndexOf(node.Entity);
                entities.RemoveAt(index);
            }
            Assert.AreEqual(0, entities.Count);
        }
		
        [Test]
        public void ReleaseFamilyEmptiesNodeList()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            _game.ReleaseNodeList<MockNode>();
            Assert.IsNull(nodes.Head);
        }

        [Test]
        public void ReleaseFamilySetsNextNodeToNull()
        {
            var entities = new List<Entity>();
            for (var i = 0; i < 5; ++i)
            {
                var entity = new Entity();
                entity.Add(new Point());
                entity.Add(new Matrix());
                entities.Add(entity);
                _game.AddEntity(entity);
            }
			
            var nodes = _game.GetNodeList<MockNode>();
            var node = nodes.Head.Next;
            _game.ReleaseNodeList<MockNode>();
            Assert.IsNull(node.Next);
        }
		
        [Test]
        public void RemoveAllEntitiesDoesWhatItSays()
        {
            var entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            entity = new Entity();
            entity.Add(new Point());
            entity.Add(new Matrix());
            _game.AddEntity(entity);
            var nodes = _game.GetNodeList<MockNode>();
            _game.RemoveAllEntities();
            Assert.IsNull(nodes.Head);
        }

        private class MockNode : Node
        {
            public Point Point { get; set; }
            public Matrix Matrix { get; set; }
        }
    }
}

