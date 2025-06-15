using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NhatMinh_WPF_BT
{
    public class CinemaRepository : IRepository<Cinema>
    {
        public ObservableCollection<Cinema> Cinemas { get; set; }

        public CinemaRepository()
        {
            Load(); // lần đầu tiên
        }

        public void Load()
        {
            Cinemas = new ObservableCollection<Cinema>();

            DataProvider.pathData = FilePath.Cinema;
            DataProvider.Open();

            string xPath = string.Format("//Cinema");
            XmlNodeList listNode = DataProvider.getDsNode(xPath);
            Seat seat = null;
            Cinema cinema = null;
            foreach (XmlNode itemCinema in listNode)
            {
                string idCinema = itemCinema.Attributes["IdCinema"].Value;
                string nameCinema = itemCinema.Attributes["Name"].Value;
                string cinemaType = itemCinema.Attributes["Type"].Value;
                int priceCenter = Convert.ToInt32(itemCinema.Attributes["PriceCenter"].Value);

                if (cinemaType == "Vip")
                {
                    cinema = new CinemaVip(idCinema, nameCinema);
                    (cinema as CinemaVip).PriceCenter = priceCenter;
                }
                else
                {
                    cinema = new CinemaStandard(idCinema, nameCinema);
                    (cinema as CinemaStandard).PriceCenter = priceCenter;
                }
                xPath = string.Format("//Cinema[@IdCinema='{0}']/Seat", idCinema);

                XmlNodeList listNodeCinema = DataProvider.getDsNode(xPath);

                foreach (XmlNode itemSeat in listNodeCinema)
                {
                    seat = new Seat();
                    seat.Id = itemSeat.Attributes["Id"].Value;
                    seat.Name = itemSeat.Attributes["Name"].Value;
                    seat.Status = (itemSeat.Attributes["Status"].Value == "0") ? true : false;

                    cinema.Seats.Add(seat);
                }
                Cinemas.Add(cinema);
            }
            DataProvider.Close();
        }

        public void Add(Cinema item)
        {
            Cinemas.Add(item);

            DataProvider.pathData = FilePath.Cinema;
            DataProvider.Open();

            XmlNode newNode = DataProvider.createNode("Cinema");

            XmlAttribute attr1 = DataProvider.createAttr("Id");
            attr1.Value = item.IdCinema;

            XmlAttribute attr2 = DataProvider.createAttr("Name");
            attr2.Value = item.Name;

            XmlAttribute attr3 = DataProvider.createAttr("Type");
            attr3.Value = item.Type.ToString();

            XmlAttribute attr4 = DataProvider.createAttr("PriceCenter");
            attr4.Value = item.PriceCenter.ToString();


            newNode.Attributes.Append(attr1);
            newNode.Attributes.Append(attr2);
            newNode.Attributes.Append(attr3);
            newNode.Attributes.Append(attr4);

            string xPath = string.Format("//CinemaStore");
            XmlNode node = DataProvider.getNode(xPath);
            DataProvider.AppendNode(node, newNode);
            DataProvider.Close();
        }

        public void Delete(Cinema entity)
        {
            var CinemaToRemove = Get(entity.IdCinema);
            if (CinemaToRemove != null)
            {
                Cinemas.Remove(CinemaToRemove);

                DataProvider.pathData = FilePath.Cinema;
                DataProvider.Open();

                string xPath = $"//Cinema[@IdCinema='{entity.IdCinema}']";
                XmlNode node = DataProvider.getNode(xPath);

                if (node != null && node.ParentNode != null)
                    node.ParentNode.RemoveChild(node);

                DataProvider.Close();
            }
        }

        public Cinema Get(string id)
        {
            foreach (var Cinema in Cinemas)
            {
                if (Cinema.IdCinema == id.ToString())
                {
                    return Cinema;
                }
            }
            return null;
        }

        public void Update(Cinema entity)
        {
            var CinemaToUpdate = Get(entity.IdCinema);
            if (CinemaToUpdate != null)
            {
                CinemaToUpdate.IdCinema = entity.IdCinema;
                CinemaToUpdate.Name = entity.Name;
                CinemaToUpdate.Type = entity.Type;
                CinemaToUpdate.PriceCenter = entity.PriceCenter;

                DataProvider.pathData = FilePath.Cinema;
                DataProvider.Open();

                string xPath = $"//Cinema[@IdCinema='{entity.IdCinema}']";
                XmlNode node = DataProvider.getNode(xPath);

                if (node != null)
                {
                    node.Attributes["Name"].Value = entity.Name;
                    node.Attributes["Type"].Value = entity.Type;
                    node.Attributes["PriceCenter"].Value = entity.PriceCenter.ToString();
                }

                DataProvider.Close();
            }
        }

        public List<Cinema> Gets()
        {
            return Cinemas.ToList();
        }
    }
}
