using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        [HttpGet]
        public async Task<TreeModel> GetTreeAsync([FromQuery]int treeId)
            => await Task.FromResult(
                CurrentModel.FindNode(treeId)
            );

        [HttpPost]
        public async Task<TreeModel> CreateThematicTreeAsync([FromQuery]int nodeIdParent, [FromBody] Node node)
        {
            //TEMPORARY, JUST TO CHECK THE ENDPOINT IN LIVE
            if (nodeIdParent <= 0)
                CurrentModel = new TreeModel(node.Id, node.Name, new[] { CurrentModel.Root });
            else
                CurrentModel = CurrentModel.AddNode(nodeIdParent, node.Id, node.Name).Root;
            return await Task.FromResult(CurrentModel);
        }

        [HttpPut]
        public async Task<TreeModel> UpdateThematicTreeAsync([FromBody] Node node)
        {
            CurrentModel.FindNode(node.Id).Name = node.Name;
            return await Task.FromResult(CurrentModel);
        }


        [HttpDelete]
        public async Task<TreeModel> DeleteThematicTreeAsync([FromQuery]int treeId)
        {
            return await Task.FromResult(CurrentModel.DeleteBranch(treeId).Root);
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

    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
