using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoDevilAPI.Extensions
{
    public static class CoinbaseProConverters
    {
        public static OrderResponse ConvertToOrderResponse(this CoinbasePro.Services.Orders.Models.Responses.OrderResponse order)
        {
            if (order == null)
            {
                return null;
            }

            OrderResponse result = new OrderResponse()
            {
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                DoneAt = order.DoneAt == DateTime.MinValue ? (DateTime?)null:order.DoneAt,
                FilledSize = order.FilledSize,
                Price = order.Price,
                ProductId = order.ProductId,
                Side = order.Side.ToString(),
                Size = order.Size,
            };

            return result;

        }
        public static List<OrderResponse> ConvertToOrderResponseList(this IList<IList<CoinbasePro.Services.Orders.Models.Responses.OrderResponse>> orderResponseList)
        {
            if (orderResponseList == null)
            {
                return null;
            }

            List<OrderResponse> result = new List<OrderResponse>();

            orderResponseList.ToList().ForEach(x =>
            {
                x.ToList().ForEach(y => result.Add(y.ConvertToOrderResponse())); 
            });

            return result;

        }
    }
}
