using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NhatMinh_WPF_BT
{
    public class ScheduleRepository : IRepository<Schedule>, IGetLast<Schedule>
    {
        public ObservableCollection<Schedule> Schedules { get; set; }

        public ScheduleRepository()
        {
            Load();
        }

        public void Load()
        {
            Schedules = new ObservableCollection<Schedule>();

            DataProvider.pathData = FilePath.Schedule;
            DataProvider.Open();

            XmlNodeList nodes = DataProvider.getDsNode("//Schedule");

            foreach (XmlNode node in nodes)
            {
                Schedule schedule = new Schedule
                (
                    node.Attributes["Id"].Value,
                    node.Attributes["IdMovie"].Value,
                    node.Attributes["IdCinema"].Value,
                    DateTime.ParseExact(node.Attributes["AirDate"].Value, "dd/MM/yyyy", null),
                    node.Attributes["Status"].Value == "1" ? true : false
                );

                Schedules.Add(schedule);
            }

            DataProvider.Close();
        }

        public void Add(Schedule schedule)
        {
            Schedules.Add(schedule);

            DataProvider.pathData = FilePath.Schedule;
            DataProvider.Open();
            XmlNode newNode = DataProvider.createNode("Schedule");

            newNode.Attributes.Append(DataProvider.createAttr("Id")).Value = schedule.Id;
            newNode.Attributes.Append(DataProvider.createAttr("IdMovie")).Value = schedule.IdMovie;
            newNode.Attributes.Append(DataProvider.createAttr("IdCinema")).Value = schedule.IdCinema;
            newNode.Attributes.Append(DataProvider.createAttr("AirDate")).Value = schedule.AirDate.ToString("dd/MM/yyyy");
            newNode.Attributes.Append(DataProvider.createAttr("Status")).Value = schedule.Status ? "1" : "0";

            XmlNode parent = DataProvider.getNode("//Schedules");
            DataProvider.AppendNode(parent, newNode);
            DataProvider.Close();
        }

        public void Delete(Schedule schedule)
        {
            var target = Get(schedule.Id);
            if (target != null)
            {
                Schedules.Remove(target);

                DataProvider.pathData = FilePath.Schedule;
                DataProvider.Open();
                string xPath = $"//Schedule[@Id='{schedule.Id}']";
                XmlNode node = DataProvider.getNode(xPath);
                if (node?.ParentNode != null)
                    node.ParentNode.RemoveChild(node);
                DataProvider.Close();
            }
        }

        public void Update(Schedule schedule)
        {
            var target = Get(schedule.Id);
            if (target != null)
            {
                target.IdMovie = schedule.IdMovie;
                target.IdCinema = schedule.IdCinema;
                target.AirDate = schedule.AirDate;
                target.Status = schedule.Status;

                DataProvider.pathData = FilePath.Schedule;
                DataProvider.Open();
                string xPath = $"//Schedule[@Id='{schedule.Id}']";
                XmlNode node = DataProvider.getNode(xPath);

                if (node != null)
                {
                    node.Attributes["IdMovie"].Value = schedule.IdMovie;
                    node.Attributes["IdCinema"].Value = schedule.IdCinema;
                    node.Attributes["AirDate"].Value = schedule.AirDate.ToString("dd/MM/yyyy");
                    node.Attributes["Status"].Value = schedule.Status ? "1" : "0";
                }

                DataProvider.Close();
            }
        }

        public Schedule Get(string id)
        {
            return Schedules.ToList().Find(s => s.Id == id);
        }

        public List<Schedule> Gets()
        {
            return Schedules.ToList();
        }

        public Schedule GetLast()
        {
            DataProvider.pathData = FilePath.Schedule;
            DataProvider.Open();

            XmlNode lastSchedule = DataProvider.GetLastValue("//Schedules/Schedule");

            if (lastSchedule != null)
            {
                string id = lastSchedule.Attributes["Id"]?.Value;
                string idMovie = lastSchedule.Attributes["IdMovie"]?.Value;
                string idCinema = lastSchedule.Attributes["IdCinema"]?.Value;
                DateTime airDate = DateTime.Parse(lastSchedule.Attributes["AirDate"]?.Value);
                bool status = lastSchedule.Attributes["Status"]?.Value == "1";

                return new Schedule(id, idMovie, idCinema, airDate, status);
            }

            return null;
        }
    }
}
