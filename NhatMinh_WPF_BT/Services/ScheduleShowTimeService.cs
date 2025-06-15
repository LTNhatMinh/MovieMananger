using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class ScheduleShowTimeService
    {
        private static UnitOfWork _unitOfWork = UnitOfWork.Instance;
        public ObservableCollection<ScheduleShowTime> scheduleShowTimes;

        public ScheduleShowTimeService()
        {
            scheduleShowTimes = new ObservableCollection<ScheduleShowTime>(_unitOfWork.ScheduleShowTimeRepository.Gets());
        }

        public bool Add(ScheduleShowTime scheduleShowTime)
        {
            if (scheduleShowTimes == null)
                return false;

            scheduleShowTimes.Add(scheduleShowTime);
            _unitOfWork.ScheduleShowTimeRepository.Add(scheduleShowTime);
            return true;
        }

        public bool SetBoughtSeats(string idSchedule, List<BoughtSeat> boughtSeats)
        {
            if (idSchedule.Length == 0 || boughtSeats == null)
                return false;

            (_unitOfWork.ScheduleShowTimeRepository as ScheduleShowTimeRepository)
                            .SetBoughtSeats(idSchedule, boughtSeats);
            return true;
        }

        public List<ScheduleShowTime> GetListingScheduleShowTimeByScheduleId(List<string> idSchedules)
        {
            List<ScheduleShowTime> newScheduleShowTimes = new List<ScheduleShowTime>();

            foreach (ScheduleShowTime scheduleShowTime in scheduleShowTimes)
            {
                for (int i = 0; i < idSchedules.Count; i++)
                {
                    if (scheduleShowTime.IdSchedule.Contains(idSchedules[i]))
                        newScheduleShowTimes.Add(scheduleShowTime);
                }
            }

            if (newScheduleShowTimes.Count == 0)
                return null;

            return newScheduleShowTimes;
        }

        public List<ScheduleShowTimeDto> GetListingScheduleShowTimeByMovieName(string movieName, List<ScheduleShowTimeDto> scheduleShowTimeDtos)
        {
            List<ScheduleShowTimeDto> newScheduleShowTimes = new List<ScheduleShowTimeDto>();

            foreach (ScheduleShowTimeDto scheduleShowTimeDto in scheduleShowTimeDtos)
            {
                if (scheduleShowTimeDto.MovieName.Contains(movieName))
                    newScheduleShowTimes.Add(scheduleShowTimeDto);
            }

            if (newScheduleShowTimes.Count == 0)
                return null;

            return newScheduleShowTimes;
        }

        public List<ScheduleShowTimeDto> GetListingScheduleShowTimeByDate(
                                        DateTime date,
                                        List<ScheduleShowTimeDto> scheduleShowTimeDtos)
        {
            List<ScheduleShowTimeDto> newScheduleShowTimes = new List<ScheduleShowTimeDto>();

            foreach (ScheduleShowTimeDto scheduleShowTimeDto in scheduleShowTimeDtos)
            {
                if (scheduleShowTimeDto.AirDate.Date == date.Date)
                {
                    newScheduleShowTimes.Add(scheduleShowTimeDto);
                }
            }

            return newScheduleShowTimes.Count > 0 ? newScheduleShowTimes : null;
        }


        public bool UpdateStatusScheduleShowTime(ScheduleShowTime scheduleShowTime)
        {
            if (scheduleShowTime == null)
                return false;

            scheduleShowTime.Status = !scheduleShowTime.Status;
            _unitOfWork.ScheduleShowTimeRepository.Update(scheduleShowTime);
            return true;
        }

        public ScheduleShowTime GetScheduleShowTimeById(string id)
        {
            return scheduleShowTimes.FirstOrDefault(sch => sch.Id == id);
        }

        public List<ScheduleShowTimeDto> GetScheduleShowTimeDtos(List<Cinema> cinemas)
        {
            var showTimes = _unitOfWork.ScheduleShowTimeRepository.Gets();
            var schedules = _unitOfWork.ScheduleRepository.Gets();
            var movies = _unitOfWork.MovieRepository.Gets();

            List<ScheduleShowTimeDto> dtos = new List<ScheduleShowTimeDto>();

            foreach (var st in showTimes)
            {
                if (st.Status == true)
                {
                    string movieName = "Unknown";

                    var matchedSchedule = schedules.FirstOrDefault(sch => sch.Id == st.IdSchedule);
                    string cinemaName = cinemas.FirstOrDefault(cnm => cnm.IdCinema == matchedSchedule.IdCinema).Name;
                    Movie movie = movies.FirstOrDefault(m => m.Id == matchedSchedule.IdMovie);

                    if (movie != null)
                    {
                        movieName = movie.Name;
                    }

                    ScheduleShowTimeDto dto = new ScheduleShowTimeDto
                    (
                         st.Id,
                         movieName,
                         cinemaName,
                         matchedSchedule.IdCinema,
                         st.IdSchedule,
                         st.AirDate,
                         st.AirDate.ToString("HH:mm tt"),
                         movie.Duration,
                         st.Status
                    );

                    dtos.Add(dto);
                }
            }
            return dtos;
        }

        public List<ScheduleShowTimeDto> GetScheduleShowTimeDtosWithScheduleId(List<Cinema> cinemas, string scheduleId)
        {
            var showTimes = _unitOfWork.ScheduleShowTimeRepository.Gets();
            var schedules = _unitOfWork.ScheduleRepository.Gets();
            var movies = _unitOfWork.MovieRepository.Gets();

            List<ScheduleShowTimeDto> dtos = new List<ScheduleShowTimeDto>();

            foreach (var st in showTimes)
            {
                if (st.Status == true && st.IdSchedule == scheduleId)
                {
                    var matchedSchedule = schedules.FirstOrDefault(sch => sch.Id == st.IdSchedule);
                    if (matchedSchedule == null) continue;

                    var matchedMovie = movies.FirstOrDefault(m => m.Id == matchedSchedule.IdMovie);
                    var matchedCinema = cinemas.FirstOrDefault(c => c.IdCinema == matchedSchedule.IdCinema);

                    string movieName = matchedMovie != null ? matchedMovie.Name : "Unknown";
                    string cinemaName = matchedCinema != null ? matchedCinema.Name : "Unknown";
                    int duration = matchedMovie != null ? matchedMovie.Duration : 0;

                    ScheduleShowTimeDto dto = new ScheduleShowTimeDto(
                        st.Id,
                        movieName,
                        cinemaName,
                        matchedSchedule.IdCinema,
                        scheduleId,
                        st.AirDate,
                        st.AirDate.ToString("HH:mm tt"),
                        duration,
                        st.Status
                    );

                    dtos.Add(dto);
                }
            }

            return dtos;
        }

        public List<ScheduleShowTimeDto> GetScheduleShowTimeDtos(Schedule schedule, List<Cinema> cinemas)
        {
            var showTimes = _unitOfWork.ScheduleShowTimeRepository.Gets();
            var schedules = _unitOfWork.ScheduleRepository.Gets();
            var movies = _unitOfWork.MovieRepository.Gets();

            List<ScheduleShowTimeDto> dtos = new List<ScheduleShowTimeDto>();
            string cinemaName = cinemas.FirstOrDefault(cnm => cnm.IdCinema == schedule.IdCinema).Name;

            foreach (var st in showTimes)
            {
                string movieName = "Unknown";
                if (st.IdSchedule.Contains(schedule.Id))
                {
                    var movie = movies.FirstOrDefault(m => m.Id == schedule.IdMovie);
                    if (movie != null)
                    {
                        movieName = movie.Name;
                    }

                    ScheduleShowTimeDto dto = new ScheduleShowTimeDto
                    (
                         st.Id,
                         movieName,
                         cinemaName,
                         schedule.IdCinema,
                         st.IdSchedule,
                         st.AirDate,
                         st.AirDate.ToString("HH:mm tt"),
                         movie.Duration,
                         st.Status
                    );
                    dtos.Add(dto);
                }
            }

            return dtos;
        }

        public ScheduleShowTimeDto GetScheduleShowTimeDtosByScheduleIdAndMovieId(string scheduleId, string movieName, List<ScheduleShowTimeDto> scheduleShowTimeDtos)
        {
            foreach (ScheduleShowTimeDto scheduleShowTimeDto in scheduleShowTimeDtos)
            {
                if (scheduleShowTimeDto.Id.Equals(scheduleId) &&
                    scheduleShowTimeDto.MovieName.Equals(movieName))
                    return scheduleShowTimeDto;
            }

            return null;
        }

        public ScheduleShowTime GetScheduleShowTimeByScheduleId(string scheduleId, List<ScheduleShowTime> scheduleShowTimes)
        {
            foreach (ScheduleShowTime scheduleShowTime in scheduleShowTimes)
            {
                if (scheduleShowTime.IdSchedule.Contains(scheduleId))
                    return scheduleShowTime;
            }

            return null;
        }

        public bool CheckTimeDuration(DateTime newStart, ScheduleShowTimeDto scheduleShowTimeDtos)
        {
            int movieDuration = scheduleShowTimeDtos.Duration;

            DateTime start = scheduleShowTimeDtos.AirDate;
            DateTime end = start.AddMinutes(movieDuration + Parameter.PreparationTime);

            return newStart >= end;
        }

        public bool CheckOutTheMovieShow(ScheduleShowTimeDto scheduleShowTimeDtos)
        {
            int movieDuration = scheduleShowTimeDtos.Duration;

            DateTime start = scheduleShowTimeDtos.AirDate;
            DateTime end = start.AddMinutes(movieDuration + Parameter.PreparationTime);

            if (end.Date > scheduleShowTimeDtos.AirDate.Date)
                return true;
            return false;
        }

        public List<ScheduleShowTimeDto> GetScheduleShowTimeDtosWithDateAndCinemaId(
                                        DateOnly date,
                                        string cinemaId,
                                        List<Movie> movies,
                                        List<Cinema> cinemas,
                                        List<Schedule> schedules)
        {
            List<ScheduleShowTimeDto> scheduleShowTimeDtos = new List<ScheduleShowTimeDto>();

            foreach (var showTime in scheduleShowTimes)
            {
                var schedule = schedules.FirstOrDefault(s => s.Id == showTime.IdSchedule &&
                                s.AirDate.Date == date.ToDateTime(TimeOnly.MinValue).Date);

                if (schedule != null &&
                    schedule.AirDate.Date == date.ToDateTime(TimeOnly.MinValue).Date &&
                    schedule.IdCinema == cinemaId)
                {
                    var movie = movies.FirstOrDefault(m => m.Id == schedule.IdMovie);
                    var cinema = cinemas.FirstOrDefault(c => c.IdCinema == schedule.IdCinema);

                    if (movie != null && cinema != null)
                    {
                        scheduleShowTimeDtos.Add(new ScheduleShowTimeDto(
                            showTime.Id,
                            movie.Name,
                            cinema.Name,
                            schedule.IdCinema,
                            schedule.Id,
                            showTime.AirDate,
                            showTime.AirDate.ToString("HH:mm"),
                            movie.Duration,
                            showTime.Status
                        ));
                    }
                }
            }

            return scheduleShowTimeDtos;
        }

        public ScheduleShowTimeDto GetLastTimeScheduleShowTime(List<ScheduleShowTimeDto> scheduleShowTimeDtos)
        {
            return scheduleShowTimeDtos.OrderByDescending(d => d.AirDate).FirstOrDefault();
        }

        public ScheduleShowTime GetLast()
        {
            ScheduleShowTime lastScheduleShowTime = (_unitOfWork.ScheduleShowTimeRepository as ScheduleShowTimeRepository).GetLast();
            if (lastScheduleShowTime != null)
            {
                return lastScheduleShowTime;
            }
            return null;
        }
    }
}
