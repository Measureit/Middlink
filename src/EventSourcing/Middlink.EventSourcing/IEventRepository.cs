﻿using System;
using System.Threading.Tasks;

namespace Middlink.EventSourcing
{

    public interface IEventRepository
    {

        Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : AggregateRoot, new();

        Task AddAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot;
    }
}
