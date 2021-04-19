using UnityEngine;

namespace ModularBehaviourTree
{
    //[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    /// <summary>
    ///     A decorator node, like a composite node, can have a child node. Unlike a composite node, they can specifically only
    ///     have a single child. Their function is either to transform the result they receive from their child node's status,
    ///     to terminate the child, or repeat processing of the child, depending on the type of decorator node.
    ///     A commonly used example of a decorator is the Inverter, which will simply invert the result of the child. A child
    ///     fails and it will return success to its parent, or a child succeeds and it will return failure to the parent.
    /// </summary>
    internal abstract class Decorator : Node
    {
        protected Node node;
        protected Decorator(Node                      node) { this.node = node; }
        protected override NodeState Continue(Blackboard blackboard) => node.Tick(blackboard);
    }
}