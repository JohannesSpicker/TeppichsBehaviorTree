namespace ModularBehaviourTree.Construction.Factories.Composites
{
    internal abstract class CompositeFactory : NodeFactory
    {
        public NodeFactory[] nodeFactories;

        protected Node[] BuildNodes()
        {
            Node[] nodes = new Node[nodeFactories.Length];

            for (int i = 0; i < nodeFactories.Length; i++)
                nodes[i] = nodeFactories[i].CreateNode();

            return nodes;
        }
    }
}