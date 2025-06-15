using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class UnitOfWork
    {
        private static UnitOfWork instance;
        private static readonly object lockObj = new object();

        private IRepository<Account> accountRepository;
        private IRepository<Cinema> cinemaRepository;
        private IRepository<Order> orderRepository;
        private IRepository<Movie> movieRepository;
        private IRepository<Schedule> scheduleRepository;
        private IRepository<ScheduleShowTime> scheduleShowTimeRepository;

        AccountService accountService = null;
        MovieService movieService = null;
        CinemaService cinemaService = null;
        ScheduleService scheduleService = null;
        ScheduleShowTimeService scheduleShowTimeService = null;
        OrderService orderService = null;

        private UnitOfWork() { }

        public static UnitOfWork Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new UnitOfWork();
                        }
                    }
                }
                return instance;
            }
        }

        public IRepository<Account> AccountRepository
        {
            get
            {
                if (accountRepository == null)
                    accountRepository = new AccountRepository();
                return accountRepository;
            }
        }

        public IRepository<Cinema> CinemaRepository
        {
            get
            {
                if (cinemaRepository == null)
                    cinemaRepository = new CinemaRepository();
                return cinemaRepository;
            }
        }

        public IRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new OrderRepository();
                return orderRepository;
            }
        }

        public IRepository<Movie> MovieRepository
        {
            get
            {
                if (movieRepository == null)
                    movieRepository = new MovieRepository();
                return movieRepository;
            }
        }

        public IRepository<Schedule> ScheduleRepository
        {
            get
            {
                if (scheduleRepository == null)
                    scheduleRepository = new ScheduleRepository();
                return scheduleRepository;
            }
        }

        public IRepository<ScheduleShowTime> ScheduleShowTimeRepository
        {
            get
            {
                if (scheduleShowTimeRepository == null)
                    scheduleShowTimeRepository = new ScheduleShowTimeRepository();
                return scheduleShowTimeRepository;
            }
        }

        // Lazy Initialization
        public AccountService GetAccountService()
        {
            if (accountService == null)
                accountService = new AccountService();
            return accountService;
        }

        public MovieService GetMovieService()
        {
            if (movieService == null)
                movieService = new MovieService();
            return movieService;
        }

        public CinemaService GetCinemaService()
        {
            if (cinemaService == null)
                cinemaService = new CinemaService();
            return cinemaService;
        }

        public ScheduleService GetScheduleService()
        {
            if (scheduleService == null)
                scheduleService = new ScheduleService();
            return scheduleService;
        }

        public ScheduleShowTimeService GetScheduleShowTimeService()
        {
            if (scheduleShowTimeService == null)
                scheduleShowTimeService = new ScheduleShowTimeService();
            return scheduleShowTimeService;
        }

        public OrderService GetOrderService()
        {
            if (orderService == null)
                orderService = new OrderService();
            return orderService;
        }

    }
}
