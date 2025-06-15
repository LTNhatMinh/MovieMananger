using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace NhatMinh_WPF_BT
{
    public class ScheduleShowTimeRepository : IRepository<ScheduleShowTime>, ISetBoughtSeatsRepository, IGetLast<ScheduleShowTime>
    {
        public ObservableCollection<ScheduleShowTime> ScheduleShowTimes { get; set; }

        public ScheduleShowTimeRepository()
        {
            Load();
        }

        public void Load()
        {
            ScheduleShowTimes = new ObservableCollection<ScheduleShowTime>();

            DataProvider.pathData = FilePath.ScheduleShowTimes;
            DataProvider.Open();

            XmlNodeList nodes = DataProvider.getDsNode("//ScheduleShowTime");

            foreach (XmlNode node in nodes)
            {

                ScheduleShowTime item = new ScheduleShowTime
                (
                    node.Attributes["Id"].Value,
                    node.Attributes["IdSchedule"].Value,
                    DateTime.ParseExact(node.Attributes["AirDate"].Value, "dd/MM/yyyy HH:mm:ss", null),
                    node.Attributes["Status"].Value == "1"
                );

                foreach (XmlNode seatNode in node.SelectNodes("BoughtSeat"))
                {
                    item.BoughtSeats.Add(new BoughtSeat
                    (
                        seatNode.Attributes["SeatNo"].Value
                    ));
                }

                ScheduleShowTimes.Add(item);
            }

            DataProvider.Close();
        }

        public void Add(ScheduleShowTime scheduleShowTime)
        {
            ScheduleShowTimes.Add(scheduleShowTime);

            DataProvider.pathData = FilePath.ScheduleShowTimes;
            DataProvider.Open();
            XmlNode newNode = DataProvider.createNode("ScheduleShowTime");
            newNode.Attributes.Append(DataProvider.createAttr("Id")).Value = scheduleShowTime.Id;
            newNode.Attributes.Append(DataProvider.createAttr("IdSchedule")).Value = scheduleShowTime.IdSchedule;
            newNode.Attributes.Append(DataProvider.createAttr("AirDate")).Value = scheduleShowTime.AirDate.ToString("dd/MM/yyyy HH:mm:ss");
            newNode.Attributes.Append(DataProvider.createAttr("Status")).Value = scheduleShowTime.Status ? "1" : "0";

            foreach (var seat in scheduleShowTime.BoughtSeats)
            {
                XmlNode seatNode = DataProvider.createNode("BoughtSeat");
                seatNode.Attributes.Append(DataProvider.createAttr("SeatNo")).Value = seat.SeatNo;
                newNode.AppendChild(seatNode);
            }

            XmlNode parent = DataProvider.getNode("//ScheduleShowTimes");
            DataProvider.AppendNode(parent, newNode);
            DataProvider.Close();
        }

        public void Delete(ScheduleShowTime scheduleShowTime)
        {
            var target = Get(scheduleShowTime.Id);
            if (target != null)
            {
                ScheduleShowTimes.Remove(target);

                DataProvider.pathData = FilePath.ScheduleShowTimes;
                DataProvider.Open();
                string xPath = $"//ScheduleShowTime[@Id='{scheduleShowTime.Id}']";
                XmlNode node = DataProvider.getNode(xPath);
                if (node?.ParentNode != null)
                    node.ParentNode.RemoveChild(node);
                DataProvider.Close();
            }
        }

        public void Update(ScheduleShowTime scheduleShowTime)
        {
            var target = Get(scheduleShowTime.Id);
            if (target != null)
            {
                target.IdSchedule = scheduleShowTime.IdSchedule;
                target.AirDate = scheduleShowTime.AirDate;
                target.Status = scheduleShowTime.Status;
                target.BoughtSeats = scheduleShowTime.BoughtSeats;

                DataProvider.pathData = FilePath.ScheduleShowTimes;
                DataProvider.Open();
                string xPath = $"//ScheduleShowTime[@Id='{scheduleShowTime.Id}']";
                XmlNode node = DataProvider.getNode(xPath);

                if (node != null)
                {
                    node.Attributes["IdSchedule"].Value = scheduleShowTime.IdSchedule;
                    node.Attributes["AirDate"].Value = scheduleShowTime.AirDate.ToString("dd/MM/yyyy HH:mm:ss");
                    node.Attributes["Status"].Value = scheduleShowTime.Status ? "1" : "0";

                    XmlNodeList seatNodes = node.SelectNodes("BoughtSeat");
                    foreach (XmlNode seat in seatNodes)
                    {
                        node.RemoveChild(seat);
                    }

                    foreach (var seat in scheduleShowTime.BoughtSeats)
                    {
                        XmlNode seatNode = DataProvider.createNode("BoughtSeat");
                        seatNode.Attributes.Append(DataProvider.createAttr("SeatNo")).Value = seat.SeatNo;
                        node.AppendChild(seatNode);
                    }
                }

                DataProvider.Close();
            }
        }

        public ScheduleShowTime Get(string id)
        {
            return ScheduleShowTimes.FirstOrDefault(x => x.Id == id);
        }

        public List<ScheduleShowTime> Gets()
        {
            return ScheduleShowTimes.ToList();
        }

        public void SetBoughtSeats(string idSchedule, List<BoughtSeat> boughtSeats)
        {
            DataProvider.pathData = FilePath.ScheduleShowTimes;
            DataProvider.Open();

            XmlNode scheduleNode = DataProvider.getNode($"//ScheduleShowTimes/ScheduleShowTime[@Id='{idSchedule}']");

            var oldSeats = scheduleNode.SelectNodes("BoughtSeat");
            foreach (XmlNode old in oldSeats)
            {
                scheduleNode.RemoveChild(old);
            }

            foreach (var seat in boughtSeats)
            {
                XmlNode seatNode = DataProvider.createNode("BoughtSeat");
                seatNode.Attributes.Append(DataProvider.createAttr("SeatNo")).Value = seat.SeatNo.ToString();

                scheduleNode.AppendChild(seatNode);
            }

            DataProvider.Close();
        }

        public ScheduleShowTime GetLast()
        {
            DataProvider.pathData = FilePath.ScheduleShowTimes;
            DataProvider.Open();

            // Đảm bảo xpath đúng là tới các node con
            XmlNode lastSchedule = DataProvider.GetLastValue("//ScheduleShowTimes/ScheduleShowTime");

            if (lastSchedule != null)
            {
                string id = lastSchedule.Attributes["Id"]?.Value;
                string idSchedule = lastSchedule.Attributes["IdSchedule"]?.Value;
                DateTime airDate = DateTime.Parse(lastSchedule.Attributes["AirDate"]?.Value);
                bool status = lastSchedule.Attributes["Status"]?.Value == "1";

                return new ScheduleShowTime(id, idSchedule, airDate, status);
            }

            return null;
        }
    }
}
