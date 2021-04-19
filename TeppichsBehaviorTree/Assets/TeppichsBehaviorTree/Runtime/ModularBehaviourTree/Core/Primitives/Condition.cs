namespace ModularBehaviourTree
{
    /// <summary>
    ///     Conditions are also leaf nodes in the tree and are the tree’s primary way of checking for information in the world.
    ///     For example, conditions would be used to check if there’s cover nearby,  if  an  enemy  is  in  range,  or  if  an
    ///     object  is  visible.  All  conditions  are  effectively Boolean, since they rely on the return statuses of
    ///     behaviors (success and failure) to express True and False.In practice, conditions are used in two particular
    ///     cases:
    ///     •Instant Check Mode—See if the condition is true given the current state of the world at this point in time.
    ///     The check is run once immediately and the condition terminates.
    ///     •Monitoring Mode—Keep checking a condition over time, and keep running every frame as long as it is True. If it
    ///     becomes False, then exit with a FAILURE code.
    /// </summary>
    public abstract class Condition : Node
    {
        protected abstract bool Check(Blackboard blackboard);

        protected override NodeState Continue(Blackboard blackboard) => Check(blackboard) ? NodeState.Success : NodeState.Failure;
    }
}