using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    public class TreeController : ControllerBase
    {
        [HttpGet("api/tree/{nodeId}")]
        public async Task<ActionResult<TreeModel>> GetTreeAsync(int nodeId)
        {
            var nodeFound = CurrentModel.FindNode(nodeId);
            if (nodeFound == null)
                return NotFound();
            return Ok(await Task.FromResult(nodeFound));
        }

        [HttpPost("api/tree")]
        public async Task<ActionResult<TreeModel>> CreateThematicTreeAsync([FromBody] NodeModel node)
        {
            //TEMPORARY, JUST TO CHECK THE ENDPOINT IN LIVE
            bool isRootCreation = node == null;
            if (!isRootCreation)
            {
                if (node.Name == null || node.WithChildren.HasValue || !node.ParentId.HasValue)
                    return BadRequest(@"For the creation of a node, a name and a parentId are necessary
and don't put the field withchildren in the payload, please.");
            }

            if (isRootCreation)
                CurrentModel = new TreeModel(0, "root");
            else
            {
                var nodeParentFound = CurrentModel.FindNode(node.ParentId.Value);
                int position = node.Position.HasValue ? node.Position.Value : -1;
                _ = new TreeModel(0, node.Name, nodeParentFound, position);
            }
            return Ok(await Task.FromResult(CurrentModel));
        }

        [HttpPut("api/tree/{nodeId}")]
        public async Task<ActionResult<TreeModel>> UpdateThematicTreeAsync(int nodeId, [FromBody] NodeModel node)
        {
            var nodeToMoveFound = CurrentModel.FindNode(nodeId);
            if (nodeToMoveFound == null)
                return NotFound();

            if (nodeToMoveFound.IsRoot)
                return BadRequest("The root node is not updatable.");

            var nodeParentFound = CurrentModel.FindNode(node.ParentId.Value);
            if (nodeParentFound == null)
                return BadRequest("The parent node does not exist.");

            if (node == null || !node.ParentId.HasValue)
                return BadRequest("For the updating of a node and a parentId are necessary, please.");

            int position = node.Position.HasValue ? node.Position.Value : -1;
            if (nodeToMoveFound.Parent.Id != nodeParentFound.Id || position != nodeToMoveFound.Position)
            {
                nodeParentFound.AddChild(
                    nodeToMoveFound.RemoveNode(node.WithChildren.HasValue && node.WithChildren.Value)
                    , position
                );
            }

            if (node.Name != null)
                nodeToMoveFound.Name = node.Name;

            return Ok(await Task.FromResult(CurrentModel));
        }


        [HttpDelete("api/tree/{nodeId}")]
        public async Task<ActionResult<TreeModel>> DeleteThematicTreeAsync(int nodeId)
        {
            var (newModel, childDetached) = CurrentModel.RemoveBranch(nodeId);
            if (newModel == null && childDetached == null)
                return NotFound();
            CurrentModel = newModel;
            return Ok(await Task.FromResult(CurrentModel));
        }

        //TEMPORARY, JUST TO CHECK THE ENDPOINT IN LIVE
        private static TreeModel CurrentModel = new TreeModel(1, "1"
                    , new[]
                    {
                            new TreeModel(2, "1.1"
                                , new []
                                {
                                    new TreeModel(3, "1.1.1")
                                })
                            , new TreeModel(4, "1.2")
                    });
    }
}
