namespace ModularBehaviourTree
{
    
    //[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    /// <summary>
    ///     A behaviour tree is made up of several types of nodes, however some core functionality is common to any type of
    ///     node in a behaviour tree. This is that they can return one of three statuses. (Depending on the implementation of
    ///     the behaviour tree, there may be more than three return statuses, however I've yet to use one of these in practice
    ///     and they are not pertinent to any introduction to the subject) The three common statuses are as follows:
    ///     Success
    ///     Failure
    ///     Running
    ///     The first two, as their names suggest, inform their parent that their operation was a success or a failure. The
    ///     third means that success or failure is not yet determined, and the node is still running. The node will be ticked
    ///     again next time the tree is ticked, at which point it will again have the opportunity to succeed, fail or continue
    ///     running.
    ///     This functionality is key to the power of behaviour trees, since it allows a node's processing to persist for many
    ///     ticks of the game. For example a Walk node would offer up the Running status during the time it attempts to
    ///     calculate a path, as well as the time it takes the character to walk to the specified location. If the pathfinding
    ///     failed for whatever reason, or some other complication arisen during the walk to stop the character reaching the
    ///     target location, then the node returns failure to the parent. If at any point the character's current location
    ///     equals the target location, then it returns success indicating the Walk command executed successfully.
    ///     This means that this node in isolation has a cast iron contract defined for success and failure, and any tree
    ///     utilizing this node can be assured of the result it received from this node. These statuses then propagate and
    ///     define the flow of the tree, to provide a sequence of events and different execution paths down the tree to make
    ///     sure the AI behaves as desired.
    /// </summary>
    public abstract class Node
    {
        public enum NodeState
        {
            Failure,
            Running,
            Success
        }

        private NodeState state;

        /// <summary>
        ///     Called exactly once each tree tick until returns Failure or Success.
        /// </summary>
        public NodeState Tick(Blackboard blackboard)
        {
            if (state != NodeState.Running)
                Initialise(blackboard);

            state = Continue(blackboard);

            if (state != NodeState.Running)
                Terminate(blackboard);

            return state;
        }

        /// <summary>
        ///     Called once immediately before first tick.
        /// </summary>
        protected abstract void Initialise(Blackboard blackboard);

        /// <summary>
        ///     Called exactly once each tree tick until returns not Running.
        /// </summary>
        protected abstract NodeState Continue(Blackboard blackboard);

        /// <summary>
        ///     Called once immediately after Tick returns not Running.
        /// </summary>
        protected abstract void Terminate(Blackboard blackboard);
    }
}