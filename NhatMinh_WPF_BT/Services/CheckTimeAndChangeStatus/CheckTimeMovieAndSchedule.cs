using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NhatMinh_WPF_BT
{
    public class CheckTimeMovieAndSchedule
    {
        private ChangeStatusAndRemove changeStatusAndRemove = new ChangeStatusAndRemove();
        public bool CheckMovieTimeAndChangeStatus(List<Movie> movies)
        {
            if (movies == null)
                return false;

            changeStatusAndRemove.Change(movies, UnitOfWork.Instance.MovieRepository);
            return true;
        }

        public bool CheckSchedulesTimeAndChangeStatus(List<Schedule> schedules)
        {
            if (schedules == null)
                return false;

            changeStatusAndRemove.Change(schedules, UnitOfWork.Instance.ScheduleRepository);
            //changeStatusAndRemove.Remove(schedules);
            return true;
        }

        public bool CheckScheduleShowtimesTimeAndChangeStatus(List<ScheduleShowTime> scheduleShowTimes)
        {
            if (scheduleShowTimes == null)
                return false;

            changeStatusAndRemove.Change(scheduleShowTimes, UnitOfWork.Instance.ScheduleShowTimeRepository);
            //changeStatusAndRemove.Remove(scheduleShowTimes);
            return true;
        }

        public bool CheckTheDeadline(List<Movie> movies, List<Schedule> schedules,
                                    List<ScheduleShowTime> scheduleShowTimes)
        {
            if (movies.Count == 0 || schedules.Count == 0 || scheduleShowTimes.Count == 0)
                return false;

            CheckMovieTimeAndChangeStatus(movies);
            CheckSchedulesTimeAndChangeStatus(schedules);
            CheckScheduleShowtimesTimeAndChangeStatus(scheduleShowTimes);

            return true;
        }
    }
}
