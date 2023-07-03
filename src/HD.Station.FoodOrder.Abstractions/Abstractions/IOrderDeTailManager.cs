﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Station.FoodOrder.Abstractions.Data;

namespace HD.Station.FoodOrder.Abstractions.Abstractions
{
    public interface IOrderDeTailManager: IManager<OrderDetail, Guid>
    {
        Task<IQueryable<OrderDetail>> GetAllAsync();
        IEnumerable<OrderDetail> GetListAllAsync();
        Task<IQueryable<OrderDetail>> QueryIncludeFilterAsync(string filterText = null);
        Task<(OperationResult State, OrderDetail Value)> AddEntityAsync(OrderDetail entity);
        Task<OperationResult> DeleteInAnotherRecordAsync(Guid id);
        Task<OrderDetail> GetByCustomerIdAsync(Guid id);
    }
}
