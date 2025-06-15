using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class ScheduleService
    {
        private static UnitOfWork _unitOfWork = UnitOfWork.Instance;
        public ObservableCollection<Schedule> schedules;

        public ScheduleService()
        {
            schedules = new ObservableCollection<Schedule>(_unitOfWork.ScheduleRepository.Gets());
        }

        public bool Add(Schedule schedule)
        {
            if (schedule == null)
                return false;

            schedules.Add(schedule);
            _unitOfWork.ScheduleRepository.Add(schedule);
            return true;
        }

        public List<Schedule> GetListingScheduleByMovieId(string idMovie)
        {
            List<Schedule> newSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                if (schedule.IdMovie.Contains(idMovie) && schedule.Status == true)
                    newSchedules.Add(schedule);
            }

            if (newSchedules.Count == 0)
                return null;

            return newSchedules;
        }

        public List<Schedule> GetListingScheduleByDateAndCinemaId(DateOnly date, string cinemaId)
        {
            List<Schedule> newSchedules = new List<Schedule>();

            foreach (Schedule schedule in schedules)
            {
                if (schedule.Status == true &&
                    schedule.IdCinema == cinemaId &&
                    schedule.AirDate.Date == date.ToDateTime(TimeOnly.MinValue).Date)
                {
                    newSchedules.Add(schedule);
                }
            }

            return newSchedules;
        }

        public List<ScheduleDto> MapSchedulesToScheduleDtos(List<Schedule> schedules, List<Movie> movies, List<Cinema> cinemas)
        {
            List<ScheduleDto> scheduleDtos = new List<ScheduleDto>();

            foreach (var schedule in schedules)
            {
                var movie = movies.FirstOrDefault(m => m.Id == schedule.IdMovie);
                string movieName = movie != null ? movie.Name : "Unknown";
                string cinemaName = cinemas.FirstOrDefault(c => c.IdCinema == schedule.IdCinema).Name;

                var dto = new ScheduleDto(
                    schedule.Id,
                    movieName,
                    schedule.IdCinema,
                    cinemaName,
                    schedule.AirDate,
                    schedule.Status
                );

                scheduleDtos.Add(dto);
            }

            return scheduleDtos;
        }

        public Schedule GetScheduleById(string idSchedule)
        {
            foreach (Schedule schedule in schedules)
            {
                if (schedule.Id.Contains(idSchedule) && schedule.Status == true)
                    return schedule;
            }
            return null;
        }

        public ScheduleDto GetScheduleByIdAndScheduleDtos(string idSchedule, List<ScheduleDto> scheduleDtos)
        {
            foreach (ScheduleDto scheduleDto in scheduleDtos)
            {
                if (scheduleDto.ScheduleId.Contains(idSchedule) && scheduleDto.Status == true)
                    return scheduleDto;
            }
            return null;
        }

        public bool UpdateStatusSchedule(Schedule schedule)
        {
            if (schedule == null)
                return false;

            schedule.Status = !schedule.Status;
            _unitOfWork.ScheduleRepository.Update(schedule);
            return true;
        }

        public List<ScheduleDto> MapToScheduleDtos(List<Schedule> schedules, List<Movie> movies, List<Cinema> cinemas)
        {
            var dtos = new List<ScheduleDto>();

            if (schedules != null)
            {
                foreach (var schedule in schedules)
                {
                    if (schedule.Status == true)
                    {
                        var movie = movies.FirstOrDefault(m => m.Id == schedule.IdMovie && schedule.Status == true);
                        var cinemaName = cinemas.FirstOrDefault(c => c.IdCinema == schedule.IdCinema).Name;
                        if (movie != null)
                            dtos.Add(new ScheduleDto(schedule.Id, movie.Name, schedule.IdCinema, cinemaName, schedule.AirDate, schedule.Status));
                    }
                }
            }

            return dtos;
        }

        public bool isExistSchedule(string idCinema, string idMovie, DateTime dateTime)
        {
            foreach (Schedule schedule in schedules)
            {
                if (schedule.IdCinema.Contains(idCinema) &&
                    schedule.IdMovie.Contains(idMovie) &&
                    schedule.AirDate.Date.Equals(dateTime))
                    return true;
            }
            return false;
        }

        public bool isExistSchedule(string idCinema, DateOnly date)
        {
            foreach (Schedule schedule in schedules)
            {
                if (schedule.IdCinema.Contains(idCinema) &&
                    schedule.AirDate.Date == date.ToDateTime(TimeOnly.MaxValue).Date)
                    return true;
            }
            return false;
        }

        public bool CheckDateSchedule(DateTime dateStart, DateTime dateSchedule)
        {
            if (dateSchedule.Date <= dateStart)
                return true;

            return false;
        }

        public List<ScheduleDto> GetScheduleDtos(
                List<Schedule> schedules,
                DateTime dateSearch,
                string cinemaId,
                List<Movie> movies,
                List<Cinema> cinemas)
        {
            List<ScheduleDto> scheduleDtos = new List<ScheduleDto>();

            foreach (var schedule in schedules)
            {
                if (schedule.AirDate.Date == dateSearch.Date &&
                    schedule.IdCinema == cinemaId)
                {
                    var movie = movies.FirstOrDefault(m => m.Id == schedule.IdMovie);
                    var cinemaName = cinemas.FirstOrDefault(c => c.IdCinema == schedule.IdCinema).Name;
                    string movieName = movie != null ? movie.Name : "Unknown";

                    scheduleDtos.Add(new ScheduleDto(
                        schedule.Id,
                        movieName,
                        schedule.IdCinema,
                        cinemaName,
                        schedule.AirDate,
                        schedule.Status
                    ));
                }
            }

            return scheduleDtos;
        }

        public Schedule GetLast()
        {
            Schedule lastSchedule = (_unitOfWork.ScheduleRepository as ScheduleRepository).GetLast();
            if (lastSchedule != null)
            {
                return lastSchedule;
            }
            return null;
        }
    }
}