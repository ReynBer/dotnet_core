using Core.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class TreeModel : TreeNode<TreeModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TreeModel(int id, string name, TreeModel parent, int position = -1) : base(Enumerable.Empty<TreeModel>(), parent, position)
        {
            Id = id;
            Name = name;
        }

        [JsonConstructor]
        public TreeModel(int id, string name, [DisallowNull] IEnumerable<TreeModel> children) : base(children, null)
        {
            Id = id;
            Name = name;
        }

        public TreeModel(int id, string name) : this(id, name, Enumerable.Empty<TreeModel>())
        {
        }

        public TreeModel Clone()
        {
            var treeById = new[] { this }.ToDictionary(k => k, v => new TreeModel(v.Id, v.Name, (TreeModel)null));
            foreach (var node in TopDown(true).Skip(1))
                treeById[node] = new TreeModel(node.Id, node.Name, treeById[node.Parent]);
            return treeById.First().Value.Root;
        }

        public TreeModel FindNode(int nodeId, bool fromRoot = true)
            => nodeId <= 0 ? Root : (fromRoot ? Root : this).TopDown().FirstOrDefault(n => n.Id == nodeId);

        public (TreeModel parent, TreeModel childDetached) RemoveBranch(int treeId)
        {
            var nodeFound = FindNode(treeId);
            return nodeFound == null
                ? (null, null)
                : (nodeFound.IsRoot
                    ? (null, nodeFound)
                    : nodeFound.Parent.RemoveBranch(c => c.Id == treeId)
                );
        }
    }
}

