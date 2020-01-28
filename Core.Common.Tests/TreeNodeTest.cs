using Core.Common.Extension;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Core.Common.Tests
{
    public class TreeNodeTest
    {
        private class NestedTree : TreeNode<NestedTree>
        {
            public int Id { get; set; }

            public NestedTree(int id) : base(Enumerable.Empty<NestedTree>(), null)
            {
                Id = id;
            }
            public NestedTree(int id, [DisallowNull] NestedTree parent) : base(Enumerable.Empty<NestedTree>(), parent)
            {
                Id = id;
            }

            public (NestedTree parent, NestedTree childDetached) RemoveBranch(int id)
                => Root.TopDown().FirstOrDefault(n => n.Id == id).Parent.RemoveBranch(c => c.Id == id);

            public NestedTree Clone()
            {
                var treeById = new[] { this }.ToDictionary(k => k, v => new NestedTree(v.Id));
                foreach (var node in TopDown().Skip(1))
                    treeById[node] = new NestedTree(node.Id, treeById[node.Parent]);
                return treeById.First().Value.Root;
            }
        }

        private readonly NestedTree _root;
        private readonly NestedTree _node0_2_5;
        private readonly IEnumerable<NestedTree> _leaves;
        private readonly int _size;

        public TreeNodeTest()
        {
            var leaves = new List<NestedTree>();
            int i = 0;
            var n0 = new NestedTree(i++);//0
            var n0_1 = new NestedTree(i++, n0);//1
            var n0_2 = new NestedTree(i++, n0);//2-
            leaves.Add(new NestedTree(i++, n0_1));//3
            leaves.Add(new NestedTree(i++, n0_1));//4
            var n0_2_5 = new NestedTree(i++, n0_2);//5-
            leaves.Add(new NestedTree(i++, n0_2));//6-
            leaves.Add(new NestedTree(i++, n0_2_5));//7-
            _root = n0;
            _leaves = leaves;
            _node0_2_5 = n0_2_5;
            _size = i;
        }

        [Theory]
        [InlineData(new int[] { 0, 1, 2, 3, 4, 6 }, new int[] { 5, 7 }, true)]
        [InlineData(new int[] { 0, 1, 2, 3, 4, 6, 7 }, new int[] { 5 }, false)]
        public void CheckRemoveNode(int[] expected, int[] expectedRemoved, bool withChildren)
        {
            var nodeRemoved = _node0_2_5.RemoveNode(withChildren);
            Assert.Equal(expected, _root.TopDown(false).Select(n => n.Id).ToArray());
            Assert.Equal(expectedRemoved, nodeRemoved.TopDown(false).Select(n => n.Id).ToArray());
        }

        [Theory]
        [InlineData(15, null)]
        [InlineData(15, -1)]
        [InlineData(15, 0)]
        [InlineData(15, 2)]
        [InlineData(15, 10000)]
        public void CheckAddChild(int id, int? position)
        {
            var nodeAdded = new NestedTree(id);
            var positionExpected = position.HasValue
                    ? position.Value
                    : -1;
            int maxValue = _root.Children.Count();
            if (positionExpected < 0)
                positionExpected = maxValue;
            positionExpected = positionExpected.Clamp(0, maxValue);
            if (position.HasValue)
                _root.AddChild(nodeAdded, position.Value);
            else
                _root.AddChild(nodeAdded);
            Assert.Contains(_root.Children, n => n.Id == id);
            Assert.Equal(_root, nodeAdded.Parent);
            Assert.Equal(_root.Children.ElementAt(positionExpected), nodeAdded);
        }

        [Theory]
        [InlineData(new int[] { 0, 1, 3, 4, 2, 5, 7, 6 }, true)]
        [InlineData(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, false)]
        public void CheckTopDownDepthTraversale(int[] expected, bool isDepthTransversal)

            => Assert.Equal(expected, _root.TopDown(isDepthTransversal).Select(n => n.Id).ToArray());

        [Fact]
        public void CheckMaxDepth()
            => Assert.Equal(3, _root.MaxDepth);

        [Fact]
        public void CheckRootProperty()

            => Assert.Equal(0, _node0_2_5.Root.Id);

        [Fact]
        public void CheckBottomUpProperty()

            => Assert.Equal(new[] { 5, 2, 0 }, _node0_2_5.BottomUp().Select(n => n.Id).ToArray());

        [Fact]
        public void CheckDepthProperty()

            => Assert.Equal(new[] { 5, 2, 0 }.Length - 1, _node0_2_5.Depth);

        [Fact]
        public void CheckIsLeafProperty()
        {
            Assert.Equal(_leaves.Select(n => n.Id).OrderBy(n => n), _root.TopDown().Where(n => n.IsLeaf).Select(n => n.Id).OrderBy(n => n));
            Assert.Equal(_size - _leaves.Count(), _root.TopDown().Count(n => !n.IsLeaf));
            Assert.Equal(_leaves.Count(), _root.TopDown().Count(n => n.IsLeaf));
        }

        [Fact]
        public void CheckIsRootProperty()
        {
            Assert.True(_root.IsRoot);
            Assert.Equal(1, _root.TopDown().Count(n => n.IsRoot));
            Assert.Equal(_size - 1, _root.TopDown().Count(n => !n.IsRoot));
        }

        [Fact]
        public void CheckClone()
        {
            var newTree = _root.Clone();
            Assert.Equal(_root.TopDown().Select(n => n.Id), newTree.TopDown().Select(n => n.Id));
            var oldNodes = _root.TopDown().GetEnumerator();
            var newNodes = newTree.TopDown().GetEnumerator();
            while (oldNodes.MoveNext())
            {
                Assert.True(newNodes.MoveNext());
                Assert.Equal(oldNodes.Current.Id, newNodes.Current.Id);
                Assert.False(ReferenceEquals(oldNodes.Current, newNodes.Current));
            }
            Assert.False(newNodes.MoveNext());
        }

        [Fact]
        public void CheckRemoveBranch()
        {
            var newTree = _root.Clone().RemoveBranch(2).parent.Root;
            Assert.Equal(new int[] { 0, 1, 3, 4 }, newTree.TopDown(false).Select(n => n.Id).ToArray());
        }
    }
}
