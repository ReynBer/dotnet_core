using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class NodeModel
    {
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int? Position { get; set; }
        public bool? WithChildren { get; set; }
    }
}
