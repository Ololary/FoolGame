using System;
using System.Collections.Generic;
using System.Text;
using TrainingApp;

namespace FoolGame.NewFolder
{
    internal class GameStart
    {
        public List<Card> deck = new List<Card>()
{
    new Card(Card.Value.Шесть,Card.Suit.Крести),
    new Card(Card.Value.Шесть,Card.Suit.Черви),
    new Card(Card.Value.Шесть,Card.Suit.Пики),
    new Card(Card.Value.Шесть,Card.Suit.Бубны),

    new Card(Card.Value.Семь,Card.Suit.Крести),
    new Card(Card.Value.Семь,Card.Suit.Черви),
    new Card(Card.Value.Семь,Card.Suit.Пики),
    new Card(Card.Value.Семь,Card.Suit.Бубны),

    new Card(Card.Value.Восемь,Card.Suit.Крести),
    new Card(Card.Value.Восемь,Card.Suit.Черви),
    new Card(Card.Value.Восемь,Card.Suit.Пики),
    new Card(Card.Value.Восемь,Card.Suit.Бубны),

    new Card(Card.Value.Девять,Card.Suit.Крести),
    new Card(Card.Value.Девять,Card.Suit.Черви),
    new Card(Card.Value.Девять,Card.Suit.Пики),
    new Card(Card.Value.Девять,Card.Suit.Бубны),

    new Card(Card.Value.Десять,Card.Suit.Крести),
    new Card(Card.Value.Десять,Card.Suit.Черви),
    new Card(Card.Value.Десять,Card.Suit.Пики),
    new Card(Card.Value.Десять,Card.Suit.Бубны),

    new Card(Card.Value.Валет,Card.Suit.Крести),
    new Card(Card.Value.Валет,Card.Suit.Черви),
    new Card(Card.Value.Валет,Card.Suit.Пики),
    new Card(Card.Value.Валет,Card.Suit.Бубны),

    new Card(Card.Value.Дама,Card.Suit.Крести),
    new Card(Card.Value.Дама,Card.Suit.Черви),
    new Card(Card.Value.Дама,Card.Suit.Пики),
    new Card(Card.Value.Дама,Card.Suit.Бубны),

    new Card(Card.Value.Король,Card.Suit.Крести),
    new Card(Card.Value.Король,Card.Suit.Черви),
    new Card(Card.Value.Король,Card.Suit.Пики),
    new Card(Card.Value.Король,Card.Suit.Бубны),

    new Card(Card.Value.Туз,Card.Suit.Крести),
    new Card(Card.Value.Туз,Card.Suit.Черви),
    new Card(Card.Value.Туз,Card.Suit.Пики),
    new Card(Card.Value.Туз,Card.Suit.Бубны),
};
        public List<Card> playerhand = new List<Card>();
        public List<Card> brainhand = new List<Card>();
        Random random = new Random();
        public void CardToHands()
        {
            for (int i = 0; i < 6; i++)
            {
                var rand = deck[random.Next(deck.Count)];
                deck.Remove(rand);
                playerhand.Add(rand);
            }
            for (int i = 0; i < 6; i++)
            {
                var rand = deck[random.Next(deck.Count)];
                deck.Remove(rand);
                brainhand.Add(rand);
            }
        }
        public void ShowHand(List<Card> hand)
        {
            foreach (var item in hand)
            {
                Console.WriteLine(item.ShowCard(item));
            }

        }

        public void MakeATrump()
        {
            Console.WriteLine("Достаю козырную карту...");
            var trump = deck[random.Next(deck.Count)];
            Console.WriteLine(trump.ShowCard(trump)+"!!");
            Console.WriteLine($"Козырь {trump.CardSuit}!!");
        }
    }
}
