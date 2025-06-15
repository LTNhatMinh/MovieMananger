using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NhatMinh_WPF_BT
{

    public class MovieRepository : IRepository<Movie>
    {
        public ObservableCollection<Movie> Movies { get; set; }

        public MovieRepository()
        {
            Load();
        }

        public void Load()
        {
            Movies = new ObservableCollection<Movie>();

            DataProvider.pathData = FilePath.Movie;
            DataProvider.Open();

            string xPath = "//Movie";
            XmlNodeList listNode = DataProvider.getDsNode(xPath);

            foreach (XmlNode item in listNode)
            {
                Movie movie = new Movie
                 (
                    item.Attributes["Id"].Value,
                    item.Attributes["Name"].Value,
                    item.Attributes["Description"].Value,
                    int.Parse(item.Attributes["Duration"].Value),
                    DateTime.Parse(item.Attributes["StartAirDate"].Value),
                    DateTime.Parse(item.Attributes["EndAirDate"].Value),
                    item.Attributes["Status"].Value == "1"
                 );

                Movies.Add(movie);
            }

            DataProvider.Close();
        }

        public void Add(Movie item)
        {
            Movies.Add(item);
            DataProvider.pathData = FilePath.Movie;
            DataProvider.Open();

            XmlNode newNode = DataProvider.createNode("Movie");

            newNode.Attributes.Append(DataProvider.createAttr("Id")).Value = item.Id;
            newNode.Attributes.Append(DataProvider.createAttr("Name")).Value = item.Name;
            newNode.Attributes.Append(DataProvider.createAttr("Description")).Value = item.Description;
            newNode.Attributes.Append(DataProvider.createAttr("Duration")).Value = item.Duration.ToString();
            newNode.Attributes.Append(DataProvider.createAttr("StartAirDate")).Value = item.StartAirDate.ToString("dd/MM/yyyy HH:mm:ss");
            newNode.Attributes.Append(DataProvider.createAttr("EndAirDate")).Value = item.EndAirDate.ToString("dd/MM/yyyy HH:mm:ss");
            newNode.Attributes.Append(DataProvider.createAttr("Status")).Value = item.Status ? "1" : "0";

            string xPath = "//Movies";
            XmlNode parentNode = DataProvider.getNode(xPath);

            if (parentNode != null)
                DataProvider.AppendNode(parentNode, newNode);

            DataProvider.Close();
        }

        public void Delete(Movie entity)
        {
            var movieToRemove = Get(entity.Id);
            if (movieToRemove != null)
            {
                Movies.Remove(movieToRemove);

                DataProvider.pathData = FilePath.Movie;
                DataProvider.Open();

                string xPath = $"//Movie[@Id='{entity.Id}']";
                XmlNode node = DataProvider.getNode(xPath);
                if (node != null && node.ParentNode != null)
                    node.ParentNode.RemoveChild(node);

                DataProvider.Close();
            }
        }

        public Movie Get(string id)
        {
            return Movies.FirstOrDefault(m => m.Id == id);
        }

        public void Update(Movie entity)
        {
            var movieToUpdate = Get(entity.Id);
            if (movieToUpdate != null)
            {
                movieToUpdate.Name = entity.Name;
                movieToUpdate.Description = entity.Description;
                movieToUpdate.Duration = entity.Duration;
                movieToUpdate.StartAirDate = entity.StartAirDate;
                movieToUpdate.EndAirDate = entity.EndAirDate;
                movieToUpdate.Status = entity.Status;

                DataProvider.pathData = FilePath.Movie;
                DataProvider.Open();

                string xPath = $"//Movie[@Id='{entity.Id}']";
                XmlNode node = DataProvider.getNode(xPath);

                if (node != null)
                {
                    node.Attributes["Name"].Value = entity.Name;
                    node.Attributes["Description"].Value = entity.Description;
                    node.Attributes["Duration"].Value = entity.Duration.ToString();
                    node.Attributes["StartAirDate"].Value = entity.StartAirDate.ToString("dd/MM/yyyy HH:mm:ss");
                    node.Attributes["EndAirDate"].Value = entity.EndAirDate.ToString("dd/MM/yyyy HH:mm:ss");
                    node.Attributes["Status"].Value = entity.Status ? "1" : "0";
                }

                DataProvider.Close();
            }
        }

        public List<Movie> Gets()
        {
            return Movies.ToList();
        }
    }
}
