using System.ComponentModel;

namespace NhatMinh_WPF_BT
{
    public class DetailOrder : INotifyPropertyChanged
    {
        public string Age { get; set; }
        public int SeatNo { get; set; }
        public int Price { get; set; }

        private int _discount;
        public int Discount
        {
            get => _discount;
            set
            {
                if (_discount != value)
                {
                    _discount = value;
                    OnPropertyChanged(nameof(Discount));
                }
            }
        }

        public DetailOrder(string age, int seatNo, int price, int discount)
        {
            Age = age;
            SeatNo = seatNo;
            Price = price;
            Discount = discount;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
