using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class MovieService
    {
        private static UnitOfWork _unitOfWork = UnitOfWork.Instance;
        public ObservableCollection<Movie> movies;

        public MovieService()
        {
            movies = new ObservableCollection<Movie>(_unitOfWork.MovieRepository.Gets());
        }

        public bool Add(Movie movie)
        {
            if (movie == null)
                return false;

            movies.Add(movie);
            _unitOfWork.MovieRepository.Add(movie);
            return true;
        }

        public bool isExistMovieByName(string movieName)
        {
            foreach (Movie movie in movies)
            {
                if (movie.Name.Contains(movieName))
                    return true;
            }
            return false;
        }

        public Movie GetMovieById(string id)
        {
            foreach (Movie movie in movies)
            {
                if (movie.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return movie;
            }
            return null;
        }

        public Movie GetMovieByName(string name)
        {
            foreach (Movie movie in movies)
            {
                if (movie.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return movie;
            }
            return null;
        }

        public Movie GetMovieByIdAndStatus(string id)
        {
            foreach (Movie movie in movies)
            {
                if (movie.Id.Equals(id, StringComparison.OrdinalIgnoreCase) && movie.Status == true)
                    return movie;
            }
            return null;
        }

        public List<Movie> GetMovieByStatus()
        {
            List<Movie> list = new List<Movie>();
            foreach (Movie movie in movies)
            {
                if (movie.Status == true)
                    list.Add(movie);
            }
            if (list.Count > 0)
                return list;
            return null;
        }

        public bool UpdateStatusMovie(Movie movie)
        {
            if (movie == null)
                return false;
            movie.Status = !movie.Status;
            _unitOfWork.MovieRepository.Update(movie);
            return true;
        }

        public bool CheckStartDateAndEndDate(DateTime start, DateTime end)
        {
            if (start >= end)
                return false;
            return true;
        }

        public bool CheckOutTheTimeForFilming(Movie movie)
        {
            if (DateTime.Now < movie.EndAirDate)
                return true;

            return false;
        }

        public bool ChangeStatusSchedule(ScheduleService scheduleService, ScheduleShowTimeService scheduleShowTimeService, Movie movie,
                                         List<ScheduleShowTime> scheduleShowTimes, List<string> scheduleIds)
        {
            List<Schedule> schedules = new List<Schedule>();

            //if (schedules.Count == 0 || scheduleShowTimes.Count == 0 || scheduleIds.Count == 0 || movie == null)
            //    return false;
            schedules = scheduleService.GetListingScheduleByMovieId(movie.Id);
            if (schedules != null)
            {
                foreach (Schedule schedule in schedules)
                {
                    scheduleService.UpdateStatusSchedule(schedule);
                    scheduleIds.Add(schedule.Id);
                }
            }

            scheduleShowTimes = scheduleShowTimeService.GetListingScheduleShowTimeByScheduleId(scheduleIds);
            if (scheduleShowTimes != null)
            {
                foreach (ScheduleShowTime scheduleShowTime in scheduleShowTimes)
                    scheduleShowTimeService.UpdateStatusScheduleShowTime(scheduleShowTime);
            }
            return true;
        }
    }
}
