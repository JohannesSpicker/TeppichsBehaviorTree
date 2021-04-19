using UnityEngine;

namespace ModularBehaviourTree
{
    //[UnityEngine.CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    /// <summary>
    ///     A composite node is a node that can have one or more children. They will process one or more of these children in
    ///     either a first to last sequence or random order depending on the particular composite node in question, and at some
    ///     stage will consider their processing complete and pass either success or failure to their parent, often determined
    ///     by the success or failure of the child nodes. During the time they are processing children, they will continue to
    ///     return Running to the parent.
    ///     The most commonly used composite node is the Sequence, which simply runs each child in sequence, returning failure
    ///     at the point any of the children fail, and returning success if every child returned a successful status.
    ///     https://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php
    /// </summary>
    internal abstract class Composite : Node
    {
        protected                  int    cursor;
        [SerializeField] protected Node[] nodes;
        public Composite(Node[] nodes) { this.nodes = nodes; }

        protected override void Initialise(Blackboard blackboard) => cursor = 0;
        protected override void Terminate(Blackboard  blackboard) { }
    }
}