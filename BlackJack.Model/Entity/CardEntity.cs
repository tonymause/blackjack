using BlackJack.Model.Enum;

namespace BlackJack.Model.Entity
{
    public class CardEntity
    {
        public CardEntity(CardType type, int number)
        {
            _type = type;
            _number = number;
        }
        private int _number;

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        private CardType _type;

        public CardType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
