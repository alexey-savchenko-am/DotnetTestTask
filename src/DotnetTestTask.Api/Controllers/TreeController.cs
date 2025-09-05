using MediatR;
using Microsoft.AspNetCore.Mvc;
using DotnetTestTask.Application.TreeNodes.CreateNode;
using DotnetTestTask.Application.RenameNode;
using DotnetTestTask.Application.DeleteNode;
using DotnetTestTask.Application.GetOrCreateTree.Models;
using DotnetTestTask.Application.GetOrCreateTree;

namespace DotnetTestTask.Api.Controllers;

[ApiController]
public class TreeController(ISender sender) : Controller
{
    [HttpPost("api.user.tree.get")]
    [EndpointDescription("Use CompanyTree for a test.")]
    public async Task<ActionResult<TreeDto>> GetTree([FromQuery] string treeName)
    {
        var tree = await sender.Send(new GetOrCreateTreeCommand(treeName));
        return Ok(tree);
    }


    [HttpPost("api.user.tree.create")]
    public async Task<IActionResult> CreateNode(
        [FromQuery] string treeName,
        [FromQuery] long parentNodeId,
        [FromQuery] string nodeName)
    {
        var result = await sender.Send(new CreateNodeCommand(treeName, parentNodeId, nodeName));

        return result.IsFailure
            ? BadRequest(result.Error) 
            : Ok(result.Value);
    }

    [HttpPost("api.user.tree.node.rename")]
    public async Task<IActionResult> RenameNode(
        [FromQuery] string treeName,
        [FromQuery] long nodeId,
        [FromQuery] string newNodeName)
    {
        var result = await sender.Send(new RenameNodeCommand(treeName, nodeId, newNodeName));

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(true);
    }

    [HttpPost("/api.user.tree.node.delete")]
    public async Task<IActionResult> DeleteNode(
        [FromQuery] string treeName,
        [FromQuery] long nodeId)
    {
        var result = await sender.Send(new DeleteNodeCommand(treeName, nodeId));

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(true);
    }
}
