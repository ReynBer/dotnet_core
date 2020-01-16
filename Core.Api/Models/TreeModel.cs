using Core.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class TreeModel : TreeNode<TreeModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TreeModel(TreeModel parent, int id, string name) : base(parent, null)
        {
            Id = id;
            Name = name;
        }

        [JsonConstructor]
        public TreeModel(int id, string name, IEnumerable<TreeModel> children = null) : base(null, children)
        {
            Id = id;
            Name = name;
        }

        public TreeModel Clone()
        {
            var treeById = new[] { Root }.ToDictionary(k => k, v => new TreeModel(null, v.Id, v.Name));
            foreach (var node in TopDown().Skip(1))
                treeById[node] = new TreeModel(treeById[node.Parent], node.Id, node.Name);
            return treeById.First().Value.Root;
        }

        public TreeModel FindNode(int nodeId)
            => TopDown().FirstOrDefault(n => n.Id == nodeId);

        public TreeModel AddNode(int? nodeIdParent, int id, string name)
            => new TreeModel(
                (nodeIdParent.HasValue
                ? TopDown().First(n => n.Id == nodeIdParent.Value)
                : null
                ), id, name);

        public TreeModel DeleteBranch(int treeId)
            => FindNode(treeId).Parent.DeleteBranch(c => c.Id == treeId);
    }
}
