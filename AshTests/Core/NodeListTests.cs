using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using NUnit.Framework;
using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.AshTests.Core
{
    [TestFixture]
	public class NodeListTests
	{
		private NodeList _nodes;
        private Node _tempNode;

		[SetUp]
		public void CreateEntity()
		{
			_nodes = new NodeList();
            _tempNode = new Node();
		}

		[TearDown]
		public void ClearEntity()
		{
			_nodes = null;
            _tempNode = null;
		}

		[Test]
		public void AddingNodeTriggersAddedSignal()
		{
            var eventFired = false;
			var mockNode = new MockNode();
			_nodes.NodeAdded += (node) => eventFired = true;
			_nodes.Add(mockNode);
            Assert.IsTrue(eventFired);
		}
		
        [Test]
        public void RemovingNodeTriggersRemovedSignal()
        {
            var eventFired = false;
            var mockNode = new MockNode();
            _nodes.Add(mockNode);
            _nodes.NodeRemoved += (node) => eventFired = true;
            _nodes.Remove(mockNode);
            Assert.IsTrue(eventFired);
        }
		
        [Test]
        public void AllNodesAreCoveredDuringIteration()
        {
            var nodeArray = new List<Node>();
            for (var i = 0; i < 5; ++i)
            {
                var node = new MockNode();
                nodeArray.Add(node);
                _nodes.Add(node);
            }
			
            for(var node = _nodes.Head; node != null; node = node.Next)
            {
                var index = nodeArray.IndexOf(node);
                nodeArray.RemoveAt(index);
            }
            Assert.AreEqual(0, nodeArray.Count);
        }
		
        [Test]
        public void RemovingCurrentNodeDuringIterationIsValid()
        {
            var nodeArray = new List<Node>();
            for (var i = 0; i < 5; ++i)
            {
                var node = new MockNode();
                nodeArray.Add(node);
                _nodes.Add(node);
            }
			
            var count = 0;
            for (var node = _nodes.Head; node != null; node = node.Next)
            {
                var index = nodeArray.IndexOf(node);
                nodeArray.RemoveAt(index);
                if (++count == 2)
                {
                    _nodes.Remove(node);
                }
            }
            Assert.AreEqual(0, nodeArray.Count);
        }
		
        [Test]
        public void RemovingNextNodeDuringIterationIsValid()
        {
            var nodeArray = new List<Node>();
            for (var i = 0; i < 5; ++i)
            {
                var node = new MockNode();
                nodeArray.Add(node);
                _nodes.Add(node);
            }
			
            var count = 0;
            for (var node = _nodes.Head; node != null; node = node.Next)
            {
                var index = nodeArray.IndexOf(node);
                nodeArray.RemoveAt(index);
                if (++count == 2)
                {
                    _nodes.Remove(node.Next);
                }
            }
            Assert.AreEqual(1, nodeArray.Count);
        }

        [Test]
        public void componentAddedSignalContainsCorrectParameters()
        {
            Node signalNode = null;;
            _nodes.NodeAdded += (node) => signalNode = node;
            _nodes.Add(_tempNode);
            Assert.AreSame(_tempNode, signalNode);
        }
		
        [Test]
        public void componentRemovedSignalContainsCorrectParameters()
        {
            Node signalNode = null;
            _nodes.Add(_tempNode);
            _nodes.NodeRemoved += (node) => signalNode = node;
            _nodes.Remove(_tempNode);
            Assert.AreSame(_tempNode, signalNode);
        }
		
        [Test]
        public void nodesInitiallySortedInOrderOfAddition()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            var node3 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Add(node3);
            var expected = new List<Node> { node1, node2, node3 };
            var actual = new List<Node> { _nodes.Head, _nodes.Head.Next, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }
		
        [Test]
        public void SwappingOnlyTwoNodesChangesTheirOrder()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Swap(node1, node2);
            var expected = new List<Node> { node2, node1 };
            var actual = new List<Node> { _nodes.Head, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }
		
        [Test]
        public void swappingAdjacentNodesChangesTheirPositions()
        {
            var node1 = new MockNode();
            var node2 = new MockNode();
            var node3 = new MockNode();
            var node4 = new MockNode();
            _nodes.Add(node1);
            _nodes.Add(node2);
            _nodes.Add(node3);
            _nodes.Add(node4);
            _nodes.Swap(node2, node3);
            var expected = new List<Node> { node1, node3, node2, node4 };
            var actual = new List<Node> { _nodes.Head, _nodes.Head.Next, _nodes.Tail.Previous, _nodes.Tail };
            Assert.AreEqual(expected, actual);
        }
		
        //[Test]
        //public function swappingNonAdjacentNodesChangesTheirPositions() : void
        //{
        //    var node1 : MockNode = new MockNode();
        //    var node2 : MockNode = new MockNode();
        //    var node3 : MockNode = new MockNode();
        //    var node4 : MockNode = new MockNode();
        //    var node5 : MockNode = new MockNode();
        //    nodes.add( node1 );
        //    nodes.add( node2 );
        //    nodes.add( node3 );
        //    nodes.add( node4 );
        //    nodes.add( node5 );
        //    nodes.swap( node2, node4 );
        //    assertThat( nodes, nodeList( node1, node4, node3, node2, node5 ) );
        //}
		
        //[Test]
        //public function swappingEndNodesChangesTheirPositions() : void
        //{
        //    var node1 : MockNode = new MockNode();
        //    var node2 : MockNode = new MockNode();
        //    var node3 : MockNode = new MockNode();
        //    nodes.add( node1 );
        //    nodes.add( node2 );
        //    nodes.add( node3 );
        //    nodes.swap( node1, node3 );
        //    assertThat( nodes, nodeList( node3, node2, node1 ) );
        //}

        class MockNode : Node
        {
	        public Point Point { get; set; }
            public Matrix Matrix { get; set; }
        }	
    }
}

