using BlackJack.Model.Enum;

namespace BlackJack.Model.Entity
{
    public class CardEntity
    {
        public CardEntity(CardType cardType, int number)
        {
            CardType = cardType;
            Number = number;
        }

        public int Number { get; set; }
        public CardType CardType { get; set; }
    }
}
