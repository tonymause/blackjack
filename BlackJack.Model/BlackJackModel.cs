using BlackJack.Model.Entity;

namespace BlackJack.Model
{
    public class BlackJackModel : BindableEntity
    {
        #region Private Field

        private string _title;

        #endregion

        #region Property
        
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
