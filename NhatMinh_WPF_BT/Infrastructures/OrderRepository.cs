using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace NhatMinh_WPF_BT
{
    public class OrderRepository : IRepository<Order>
    {
        public ObservableCollection<Order> Orders { get; set; }

        public OrderRepository()
        {
            Load();
        }

        public void Load()
        {
            Orders = new ObservableCollection<Order>();

            DataProvider.pathData = FilePath.Orders;
            DataProvider.Open();

            string xPath = "//Order";
            XmlNodeList listNode = DataProvider.getDsNode(xPath);

            foreach (XmlNode node in listNode)
            {
                Order order = new Order
                (
                    node.Attributes["Id"].Value,
                    node.Attributes["IdScheduleShowTime"].Value,
                    node.Attributes["CinemaType"].Value,
                    node.Attributes["CustomerName"].Value,
                    node.Attributes["PhoneNumber"].Value,
                    DateTime.Parse(node.Attributes["Date"].Value),
                    double.Parse(node.Attributes["Total"].Value)
                );

                foreach (XmlNode detailNode in node.SelectNodes("DetailOrder"))
                {
                    DetailOrder detail = new DetailOrder
                    (
                        detailNode.Attributes["Age"].Value,
                        int.Parse(detailNode.Attributes["SeatNo"].Value),
                        int.Parse(detailNode.Attributes["Price"].Value),
                        int.Parse(detailNode.Attributes["Discount"].Value)
                    );

                    order.DetailOrders.Add(detail);
                }

                Orders.Add(order);
            }

            DataProvider.Close();
        }

        public void Add(Order order)
        {
            Orders.Add(order);

            DataProvider.pathData = FilePath.Orders;
            DataProvider.Open();

            XmlNode newNode = DataProvider.createNode("Order");
            newNode.Attributes.Append(DataProvider.createAttr("Id")).Value = order.Id;
            newNode.Attributes.Append(DataProvider.createAttr("IdScheduleShowTime")).Value = order.IdScheduleShowTime;
            newNode.Attributes.Append(DataProvider.createAttr("CinemaType")).Value = order.CinemaType;
            newNode.Attributes.Append(DataProvider.createAttr("CustomerName")).Value = order.CustomerName;
            newNode.Attributes.Append(DataProvider.createAttr("PhoneNumber")).Value = order.PhoneNumber;
            newNode.Attributes.Append(DataProvider.createAttr("Date")).Value = order.Date.ToString("dd/MM/yyyy HH:mm:ss");
            newNode.Attributes.Append(DataProvider.createAttr("Total")).Value = order.Total.ToString();

            foreach (var detail in order.DetailOrders)
            {
                XmlNode detailNode = DataProvider.createNode("DetailOrder");

                detailNode.Attributes.Append(DataProvider.createAttr("Age")).Value = detail.Age;
                detailNode.Attributes.Append(DataProvider.createAttr("SeatNo")).Value = detail.SeatNo.ToString();
                detailNode.Attributes.Append(DataProvider.createAttr("Price")).Value = detail.Price.ToString();
                detailNode.Attributes.Append(DataProvider.createAttr("Discount")).Value = detail.Discount.ToString();

                newNode.AppendChild(detailNode);
            }

            XmlNode parent = DataProvider.getNode("//Orders");
            DataProvider.AppendNode(parent, newNode);
            DataProvider.Close();
        }

        public void Delete(Order order)
        {
            var orderToRemove = Get(order.Id);
            if (orderToRemove != null)
            {
                Orders.Remove(orderToRemove);

                DataProvider.pathData = FilePath.Orders;
                DataProvider.Open();
                string xPath = $"//Order[@Id='{order.Id}']";
                XmlNode node = DataProvider.getNode(xPath);
                if (node?.ParentNode != null)
                    node.ParentNode.RemoveChild(node);
                DataProvider.Close();
            }
        }

        public void Update(Order order)
        {
            var orderToUpdate = Get(order.Id);
            if (orderToUpdate != null)
            {
                orderToUpdate.IdScheduleShowTime = order.IdScheduleShowTime;
                orderToUpdate.CinemaType = order.CinemaType;
                orderToUpdate.CustomerName = order.CustomerName;
                orderToUpdate.PhoneNumber = order.PhoneNumber;
                orderToUpdate.Date = order.Date;
                orderToUpdate.Total = order.Total;
                orderToUpdate.DetailOrders = order.DetailOrders;

                DataProvider.pathData = FilePath.Orders;
                DataProvider.Open();
                string xPath = $"//Order[@Id='{order.Id}']";
                XmlNode node = DataProvider.getNode(xPath);

                if (node != null)
                {
                    node.Attributes["IdScheduleShowTime"].Value = order.IdScheduleShowTime;
                    node.Attributes["CinemaType"].Value = order.CinemaType;
                    node.Attributes["CustomerName"].Value = order.CustomerName;
                    node.Attributes["PhoneNumber"].Value = order.PhoneNumber;
                    node.Attributes["Date"].Value = order.Date.ToString("dd/MM/yyyy HH:mm:ss");
                    node.Attributes["Total"].Value = order.Total.ToString();

                    node.RemoveAll();
                    foreach (var detail in order.DetailOrders)
                    {
                        XmlNode detailNode = DataProvider.createNode("DetailOrder");

                        detailNode.Attributes.Append(DataProvider.createAttr("Age")).Value = detail.Age;
                        detailNode.Attributes.Append(DataProvider.createAttr("SeatNo")).Value = detail.SeatNo.ToString();
                        detailNode.Attributes.Append(DataProvider.createAttr("Price")).Value = detail.Price.ToString();
                        detailNode.Attributes.Append(DataProvider.createAttr("Discount")).Value = detail.Discount.ToString();

                        node.AppendChild(detailNode);
                    }
                }

                DataProvider.Close();
            }
        }

        public Order Get(string id)
        {
            return Orders.ToList().Find(o => o.Id == id);
        }

        public List<Order> Gets()
        {
            return Orders.ToList();
        }

        public void AppendDetailsToOrder(string orderId, List<DetailOrder> newDetails)
        {
            DataProvider.pathData = FilePath.Orders;
            DataProvider.Open();

            XmlNode orderNode = DataProvider.getNode($"//Orders/Order[@Id='{orderId}']");
            if (orderNode == null)
            {
                Console.WriteLine($"Order with Id {orderId} not found.");
                DataProvider.Close();
                return;
            }

            foreach (var detail in newDetails)
            {
                XmlNode detailNode = DataProvider.createNode("DetailOrder");

                detailNode.Attributes.Append(DataProvider.createAttr("Age")).Value = detail.Age;
                detailNode.Attributes.Append(DataProvider.createAttr("SeatNo")).Value = detail.SeatNo.ToString();
                detailNode.Attributes.Append(DataProvider.createAttr("Price")).Value = detail.Price.ToString();
                detailNode.Attributes.Append(DataProvider.createAttr("Discount")).Value = detail.Discount.ToString();

                orderNode.AppendChild(detailNode);
            }

            DataProvider.Close();
        }

    }
}
