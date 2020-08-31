using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mapper;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class UpdateOrderHandler:IRequestHandler<UpdateOrderCommand, (bool IsSuccess, string ErrorMessage, OrderResponse Data)>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<(bool IsSuccess, string ErrorMessage, OrderResponse Data)> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderEntity = OrderMapper.Mapper.Map<Order>(request);
                if (orderEntity == null)
                    throw new ApplicationException($"Entity could not be mapped.");

                await _orderRepository.UpdateAsync(orderEntity);
                return (true, null, OrderMapper.Mapper.Map<OrderResponse>(orderEntity));
            }
            catch(Exception ex)
            {
                return (false, ex.Message.ToString(), null);
            }

        }
    }
}
