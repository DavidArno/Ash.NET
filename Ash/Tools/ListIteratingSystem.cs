using System;
using Net.RichardLord.Ash.Core;

namespace Net.RichardLord.Ash.Tools
{
    
	/**
	 * A useful class for systems which simply iterate over a set of nodes, performing the same action on each node. This
	 * class removes the need for a lot of boilerplate code in such systems. Extend this class and pass the node type and
	 * a node update method into the constructor. The node update method will be called once per node on the update cycle
	 * with the node instance and the frame time as parameters. e.g.
	 * 
	 * <code>using Net.RichardLord.Ash.Tools;
	 * 
	 *   public class MySystem : ListIteratingSystem  
	 *   {
	 *     public MySystem():base(typeof(MySystemsNode))
	 *     {
	 *			this.NodeUpdate += HandleNodeUpdate;
	 *     }
	 *     
	 *     private void HandleNodeUpdate (Node node, double time)
	 *     {
	 *       // process the node here
	 *     }
	 *   }
	 * }</code>
	 */
    public class ListIteratingSystem : Net.RichardLord.Ash.Core.SystemBase
    {
        protected NodeList nodeList;
		protected Type nodeClass;
		
		/**
		 * A signal that is dispatched whenever a node is added to the node list.
		 * 
		 * <p>The signal will pass a single parameter to the listeners - the node that was added.
		 */
	    protected event Action<Node> NodeAdded;

		/**
		 * A signal that is dispatched whenever a node is removed from the node list.
		 * 
		 * <p>The signal will pass a single parameter to the listeners - the node that was removed.
		 */
	    protected event Action<Node> NodeRemoved;
		
		protected event Action<Node, double> NodeUpdate;
		
		
		public ListIteratingSystem( Type nodeClass)
		{
			this.nodeClass = nodeClass;
		}
		
        /**
         * Called just after the system is added to the game, before any calls to the update method.
         * Override this method to add your own functionality.
         * 
         * @param game The game the system was added to.
         */
        override public void AddToGame(IGame game)
		{
			nodeList = game.GetNodeList(nodeClass);
			
			if(NodeAdded != null)
			{
				for(Node node = nodeList.Head; node != null; node = node.Next)
				{
					NodeAdded(node);
				}
			}
		}

        /**
         * Called just after the system is removed from the game, after all calls to the update method.
         * Override this method to add your own functionality.
         * 
         * @param game The game the system was removed from.
         */
        override public void RemoveFromGame(IGame game)
		{
			// I'm not actually sure if calling the 'NodeRemoved' action is required here. 
			if(NodeRemoved != null)
			{
				for(var node = nodeList.Head; node != null; node = node.Next)
				{
					NodeRemoved(node);
				}
			}
			nodeList = null;
		}
		
		override public void Update(double time)
		{
			for(Node node = nodeList.Head; node != null; node = node.Next)
			{
				if(NodeUpdate != null)
				{
					NodeUpdate(node, time);
				}
			}
		}
		
    }
}
