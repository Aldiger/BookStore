﻿namespace BookStore.Application.Common.Contracts;

using System.Threading.Tasks;
using Domain.Common.Events;

public interface IEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    Task Handle(TEvent domainEvent);
}