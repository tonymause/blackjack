namespace BlackJack.Model
{
    public class BlackJackModel : BindableEntity
    {
        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
    }
}
