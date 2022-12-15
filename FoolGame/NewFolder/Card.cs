using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingApp
{
    internal class Card
    {
        public bool trumpvalue = false;
        public enum Suit
        {
            Крести = 1,
            Черви = 2,
            Пики = 3,
            Бубны = 4,
        }
        public enum Value
        {
            Шесть = 6,
            Семь = 7,
            Восемь = 8,
            Девять = 9,
            Десять = 10,
            Валет = 11,
            Дама = 12,
            Король = 13,
            Туз = 14
        }
        public Card(Value cardValue, Suit cardSuit)
        {
            CardSuit = cardSuit;
            CardValue = cardValue;
        }

        public Suit CardSuit;
        public Value CardValue;
        public string ShowCard(Card card)
        {
            return card.CardValue.ToString() + " " + card.CardSuit.ToString();
        }

    }
}
