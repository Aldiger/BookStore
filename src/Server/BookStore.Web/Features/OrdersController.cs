﻿namespace BookStore.Web.Features;

using System.Threading.Tasks;
using Application.Sales.Orders.Commands.Cancel;
using Application.Sales.Orders.Commands.Complete;
using Application.Sales.Orders.Commands.Create;
using Application.Sales.Orders.Queries.Details;
using Application.Sales.Orders.Queries.Mine;
using Application.Sales.Orders.Queries.Search;
using Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class OrdersController : ApiController
{
    [HttpGet]
    [AuthorizeAdministrator]
    public async Task<ActionResult<OrdersSearchResponseModel>> Search(
        [FromQuery] OrdersSearchQuery query)
        => await this.Send(query);

    [HttpGet]
    [Route(nameof(Mine))]
    public async Task<ActionResult<MineOrdersResponseModel>> Mine(
        [FromQuery] MineOrdersQuery query)
        => await this.Send(query);

    [HttpGet]
    [Route(Id)]
    public async Task<ActionResult<OrderDetailsResponseModel?>> Details(
        [FromRoute] OrderDetailsQuery query)
        => await this.Send(query);

    [HttpPost]
    public async Task<ActionResult<int>> Create(
        [FromRoute] OrderCreateCommand command)
        => await this.Send(command);

    [HttpPut]
    [AuthorizeAdministrator]
    [Route(Id + PathSeparator + nameof(Cancel))]
    public async Task<ActionResult<int>> Cancel(
        [FromRoute] OrderCancelCommand command)
        => await this.Send(command);

    [HttpPut]
    [AuthorizeAdministrator]
    [Route(Id + PathSeparator + nameof(Complete))]
    public async Task<ActionResult<int>> Complete(
        [FromRoute] OrderCompleteCommand command)
        => await this.Send(command);
}