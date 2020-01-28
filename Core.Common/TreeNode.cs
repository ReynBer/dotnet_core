using Core.Common.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        protected TreeNode([DisallowNull] IEnumerable<TreeNode<TNode>> children, TreeNode<TNode> parent = null, int position = -1)
        {
            _parent = parent;
            if (parent != null)
            {
                if (position < 0)
                    parent._children.Add(this);
                else
                    parent._children.Insert(position.Clamp(0, parent._children.Count), this);
            }

            _children = children.ToList();
            foreach (var child in _children)
                child._parent = this;
        }

        [JsonIgnore]
        public int Position
            => IsRoot ? -1 : _parent._children.IndexOf(this);

        public TNode RemoveNode(bool isIncludingChildren = false)
        {
            //Root should not be removed -> Exception
            _parent._children.Remove(this);
            if (!isIncludingChildren)
            {
                foreach (var child in _children)
                    _parent.AddChild(child);
                _children.Clear();
            }
            _parent = null;
            return this as TNode;
        }

        public virtual TNode AddChild([DisallowNull] TreeNode<TNode> node, int position = -1)
        {
            node._parent = this;
            if (position < 0)
                _children.Add(node);
            else
                _children.Insert(position.Clamp(0, _children.Count), node);
            return this as TNode;
        }

        protected (TNode parent, TNode childDetached) RemoveBranch(Func<TNode, bool> predicate)
        {
            var childToDelete = Children.FirstOrDefault(predicate) as TreeNode<TNode>;
            if (_children.Remove(childToDelete))
                childToDelete._parent = null;
            return (this as TNode, childToDelete as TNode);
        }

        public IEnumerable<TNode> TopDown(bool isDepthTraversal = true)
        {
            var currentNode = this;
            yield return this as TNode;
            var queueOfNodes = new Queue<TreeNode<TNode>>();
            foreach (var child in currentNode._children)
            {
                yield return child as TNode;
                if (isDepthTraversal)
                {
                    if (!child.IsLeaf)
                        foreach (var n in child.TopDown(isDepthTraversal).Skip(1))
                            yield return n;
                }
                else
                    queueOfNodes.Enqueue(child);
            }
            foreach (var child in queueOfNodes.Where(n => !n.IsLeaf))
                foreach (var n in child.TopDown(isDepthTraversal).Skip(1))
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
            => (Root as TreeNode<TNode>).TopDown().Cast<TreeNode<TNode>>().Where(n => n.IsLeaf).Max(n => n.Depth);

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
