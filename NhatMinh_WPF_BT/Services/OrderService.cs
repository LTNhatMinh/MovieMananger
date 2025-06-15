using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NhatMinh_WPF_BT
{
    public class OrderService
    {
        private static UnitOfWork _unitOfWork = UnitOfWork.Instance;
        public ObservableCollection<Order> orders;

        public OrderService()
        {
            orders = new ObservableCollection<Order>(_unitOfWork.OrderRepository.Gets());
        }

        public bool Add(Order order)
        {
            if (order == null)
                return false;

            orders.Add(order);
            _unitOfWork.OrderRepository.Add(order);
            return true;
        }

        public List<OrderDto> GetOrderDtos(
                            List<Order> orders,
                            List<ScheduleShowTime> showTimes,
                            List<Schedule> schedules,
                            List<Movie> movies)
        {
            var result = new List<OrderDto>();

            foreach (var order in orders)
            {
                string movieName = "";

                foreach (var showTime in showTimes)
                {
                    if (showTime.Id == order.IdScheduleShowTime)
                    {
                        foreach (var schedule in schedules)
                        {
                            if (schedule.Id == showTime.IdSchedule)
                            {
                                foreach (var movie in movies)
                                {
                                    if (movie.Id == schedule.IdMovie)
                                    {
                                        movieName = movie.Name;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }

                result.Add(new OrderDto
                (
                    order.Id,
                    movieName,
                    order.CinemaType,
                    order.CustomerName,
                    order.PhoneNumber,
                    order.Date,
                    order.Total
                ));
            }

            return result;
        }

        public Order GetOrderById(string id)
        {
            foreach (Order order in orders)
            {
                if (order.Id.Contains(id))
                    return order;
            }
            return null;
        }

        public List<Order> GetListingOrderByDate(DateTime date)
        {
            List<Order> newOrders = new List<Order>();
            foreach (Order order in orders)
            {
                if (order.Date.ToString("dd/MM/yyyy").Contains(date.ToString("dd/MM/yyyy")))
                    newOrders.Add(order);
            }
            if (newOrders.Count > 0)
                return newOrders;

            return null;
        }

        public List<OrderDto> GetListingOrderByMovieName(string movieName, List<OrderDto> orderDto)
        {
            List<OrderDto> newOrders = new List<OrderDto>();
            foreach (OrderDto order in orderDto)
            {
                if (order.MovieName.Equals(movieName, StringComparison.OrdinalIgnoreCase))
                    newOrders.Add(order);
            }
            if (newOrders.Count > 0)
                return newOrders;

            return null;
        }

        public List<OrderDto> GetListingOrderByMovieNameAndDate(string movieName, DateTime date, List<OrderDto> orderDto)
        {
            List<OrderDto> newOrders = new List<OrderDto>();
            foreach (OrderDto order in orderDto)
            {
                if (order.MovieName.Equals(movieName, StringComparison.OrdinalIgnoreCase) &&
                    order.Date.Date == date.Date)
                    newOrders.Add(order);
            }
            if (newOrders.Count > 0)
                return newOrders;

            return null;
        }

        public double TotalOrderDetail(Order order)
        {
            double total = 0;
            for (int i = 0; i < order.DetailOrders.Count; i++)
            {
                double discountedPrice = order.DetailOrders[i].Price * (1 - (order.DetailOrders[i].Discount / 100.0));
                total += order.DetailOrders[i].Price - order.DetailOrders[i].Discount;
            }
            return total;
        }

        public bool isExistBoughtSeat(List<BoughtSeat> boughtSeats, int i)
        {
            foreach (var boughtSeat in boughtSeats)
            {
                if ((int.Parse(boughtSeat.SeatNo)) == i)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
