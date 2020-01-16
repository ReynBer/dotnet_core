using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common
{
    public abstract class TreeNode<TNode>
        where TNode : class
    {
        protected TreeNode<TNode> _parent;
        protected readonly IList<TreeNode<TNode>> _children;

        public IEnumerable<TNode> Children { get => _children.Cast<TNode>(); }

        [JsonIgnore]
        public TNode Parent { get => _parent as TNode; }

        protected TreeNode(TreeNode<TNode> parent = null, IEnumerable<TreeNode<TNode>> children = null)
        {
            _parent = parent;
            parent?._children?.Add(this);
            _children = children?.ToList() ?? new List<TreeNode<TNode>>();
            foreach (var child in _children)
                child._parent = this;
        }

        protected TNode DeleteBranch(Func<TNode, bool> predicate)
        {
            var childToDelete = Children.FirstOrDefault(predicate) as TreeNode<TNode>;
            _children.Remove(childToDelete);
            if (childToDelete != null)
                childToDelete._parent = null;//To be GC
            return this as TNode;
        }

        public IEnumerable<TNode> TopDown(bool isDepthTraversal = true, TNode node = null)
        {
            var currentNode = (node == null ? Root : node) as TreeNode<TNode>;
            if (node == null)
                yield return currentNode as TNode;
            var queueOfNodes = new Queue<TreeNode<TNode>>();
            foreach (var child in currentNode._children)
            {
                var childNode = child as TNode;
                yield return childNode;
                if (isDepthTraversal)
                {
                    if (!child.IsLeaf)
                        foreach (var n in TopDown(isDepthTraversal, childNode))
                            yield return n;
                }
                else
                    queueOfNodes.Enqueue(child);
            }
            foreach (var child in queueOfNodes.Where(n => !n.IsLeaf))
                foreach (var n in TopDown(isDepthTraversal, child as TNode))
                    yield return n;
        }

        public IEnumerable<TNode> BottomUp()
        {
            var node = this;
            while (!node.IsRoot)
            {
                yield return node as TNode;
                node = node._parent;
            }
            yield return node as TNode;
        }

        [JsonIgnore]
        public bool IsRoot
            => _parent == null;

        [JsonIgnore]
        public int MaxDepth
            => TopDown().Cast<TreeNode<TNode>>().Where(n => n.IsLeaf).Max(n => n.Depth);

        [JsonIgnore]
        public bool IsLeaf
            => !_children.Any();

        [JsonIgnore]
        public int Depth
            => BottomUp().Select((n, i) => i).Last();

        [JsonIgnore]
        public TNode Root
            => BottomUp().Last();
    }
}
