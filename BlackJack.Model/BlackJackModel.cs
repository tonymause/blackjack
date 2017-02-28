using BlackJack.Model.Entity;
using BlackJack.Model.Enum;

namespace BlackJack.Model
{
    public class BlackJackModel : BindableEntity
    {
        #region Private Field

        private int _index;
        private CardEntity _card;
        private UserType _userType;

        #endregion

        #region Ctor

        public BlackJackModel(int index, CardEntity card, UserType userType)
        {
            Index = index;
            Card = card;
            UserType = userType;
        }

        #endregion

        #region Property

        public int Index { get; set; }

        public CardEntity Card
        {
            get { return _card; }
            set
            {
                _card = value;
                OnPropertyChanged();
            }
        }

        public UserType UserType { get; set; }

        #endregion
    }
}
