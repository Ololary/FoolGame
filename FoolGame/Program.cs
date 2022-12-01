using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fool2
{
    class Deck
    {

        //статический массив [deck] - собственно наша колода
        protected static double[] deck = new double[36];
        //создаю два статических массива, соответственно наша рука и рука бота. Создаю их здесь, тк.в классе Deck происходит
        //основная основная подготовка к игре, поэтому наши "руки" нужны уже здесь , для работы методов.
        protected static double[] playerhand = new double[6];
        protected static double[] brainhand = new double[6];
        //
        protected static double[] table = { };



        protected static double[] intermediate = { }; int interindex = 0;//колода промежуточного отбоя 


        //Создаю массивы для создания колоды, целые числа это достоинтсво карты, дробная часть - масть.
        //можно менять колоду, в зависимости от игры, для этого добавляем цифры в [value] и меняем размер массива [deck]
        int[] value = { 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        protected static double[] suit = { 0.1, 0.2, 0.3, 0.4 };//массив [suit] пока делаю статическим, думаю пригодится далее

        //создаётся колода, по идее можно было вбить её хардкодом, но это не тру и как будто меньше полиморфизма.
        //можно потом сделать этот метод конструктором, но чтобы он не вызывался в наследниках класса.(подумать!)
        public void CreateDeck()
        {

            int decki = 0;
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < suit.Length; j++)
                {
                    deck[decki] = value[i] + suit[j];
                    decki++;
                }
            }

        }

        //перегружаемый метод-дешифратор, разделяет целую и дробные части и даёт картам имена.
        // upd:В перегрузку с массивом добавил нумерацию по индексу. потом понадобится для интерфейса управления
        //upd:Метод для одиночной карты сделал string , чтобы удобней было выводить во время игры
        protected void ShowCard(double[] cards)
        {
            double value;

            for (int i = 0; i < cards.Length; i++)
            {
                value = Math.Truncate(cards[i]);
                switch (value)
                {
                    case 11:
                        Console.Write("Валет ");
                        break;
                    case 12:
                        Console.Write("Дама ");
                        break;
                    case 13:
                        Console.Write("Король ");
                        break;
                    case 14:
                        Console.Write("Туз ");
                        break;

                    default:
                        Console.Write(value.ToString() + " ");
                        break;
                }
                switch (Math.Round(cards[i] % 1, 1)) //тут выделяем остаток и округляем до 1го дробного разряда, чтобы избежать ошибки округления.
                {
                    case 0.1:
                        Console.Write($"Черви [{Array.IndexOf(cards, cards[i])}]");
                        Console.WriteLine();
                        break;
                    case 0.2:
                        Console.Write($"Бубны [{Array.IndexOf(cards, cards[i])}]");
                        Console.WriteLine();
                        break;
                    case 0.3:
                        Console.Write($"Пики [{Array.IndexOf(cards, cards[i])}]");
                        Console.WriteLine();
                        break;
                    case 0.4:
                        Console.Write($"Крести [{Array.IndexOf(cards, cards[i])}]");
                        Console.WriteLine();
                        break;

                }
            }
        }
        protected string ShowCard(double card)
        {
            string result;
            double value;
            value = Math.Truncate(card);
            switch (value)
            {
                case 11:
                    result = "Валет ";
                    break;
                case 12:
                    result = "Дама ";
                    break;
                case 13:
                    result = "Король ";
                    break;
                case 14:
                    result = "Туз ";
                    break;

                default:
                    result = value.ToString() + " ";
                    break;
            }
            switch (Math.Round(card % 1, 1)) //тут выделяем остаток и округляем до 1го дробного разряда, чтобы избежать ошибки округления.
            {
                case 0.1:
                    result += "Черви";
                    Console.WriteLine();
                    break;
                case 0.2:
                    result += "Бубны";
                    Console.WriteLine();
                    break;
                case 0.3:
                    result += "Пики";
                    Console.WriteLine();
                    break;
                case 0.4:
                    result += "Крести";
                    Console.WriteLine();
                    break;

            }
            return result;
        }

        //переменные и метод для создания и  хранения козыря

        static double trump;
        protected static double trumpvalue;
        Random randomtrump = new Random();//вынес обьект рандома за метод,т.к. он рекурсивный и при создании обьекта рандома
        //чаще чес раз 15мс возникает ошибка повторения
        public void MakeATrump()
        {

            trump = deck[randomtrump.Next(0, deck.Length)];

            if (Math.Truncate(trump) == 14)
            {
                Console.WriteLine("Козырь: " + ShowCard(trump));
                Console.WriteLine("Туз колоду не держит.Сдаю новый козырь.");
                MakeATrump();
            }
            else
            {
                trumpvalue = Math.Round(trump % 1, 1);
                Console.Write("Козырь: " + ShowCard(trump));

            }
        } //deck = DeckLow(Array.IndexOf(deck, trump), deck);

        //Универсальный метод для уменьшения колоды или руки, позже создам такой же для увеличения
        protected double[] DeckLow(int index, double[] hand)
        {
            double[] newhand = new double[hand.Length - 1];

            for (int i = 0; i < index; i++)
            {
                newhand[i] = hand[i];
            }
            for (int i = index + 1; i < hand.Length; i++)
            {
                newhand[i - 1] = hand[i];
            }
            hand = newhand;
            return hand;
        }
        //метод для изначальной сдачи карт. При сдаче карты в руку, она убавляется из массива колоды, поэтому повторения исключены
        //upd:Теперь не только для изначальной, а для до набора карт, при отбое.
        public void CardDrow()
        {
            Random randomc = new Random();

            //Здесь проверяем количество карт, если их меньше 6ти то увеличиваем массив до 6ти. 
            if (playerhand.Length <= 6)
            {
                Array.Resize(ref playerhand, 6);
            }
            if (brainhand.Length <= 6)
            {
                Array.Resize(ref brainhand, 6);
            }
            //Метод сдаёт карты поочерёдно, начиная с того , кто ходил в предыдущем коне. Сдаю с конца, чтобы сначала заполнились карты
            //ходящего, а потом отбивающегося
            if (playerfirst)
            {
                for (int i = 5; i >= 0; i--)
                {
                    if (playerhand[i] == 0)
                    {
                        if (deck.Length > 0)
                        {
                            playerhand[i] = deck[randomc.Next(0, deck.Length)];
                            deck = DeckLow(Array.IndexOf(deck, playerhand[i]), deck);
                        }
                        else
                        {
                            Console.WriteLine("КОНЕЦ КОЛОДЫ!!");
                            for (int j = 0; j < playerhand.Length; j++)//Если колода закончилась, убираем лишние нули из рук
                            {
                                if (playerhand[j] == 0)
                                {
                                    playerhand = DeckLow(Array.IndexOf(playerhand, playerhand[j]), playerhand);
                                    j--;
                                }

                            }
                            for (int j = 0; j < brainhand.Length; j++)
                            {
                                if (brainhand[j] == 0)
                                {
                                    brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[j]), brainhand);
                                    j--;
                                }

                            }
                            break;
                        }

                    }
                    if (brainhand[i] == 0)
                    {
                        if (deck.Length > 0)
                        {
                            brainhand[i] = deck[randomc.Next(0, deck.Length)];
                            deck = DeckLow(Array.IndexOf(deck, brainhand[i]), deck);
                        }
                        else
                        {
                            Console.WriteLine("КОНЕЦ КОЛОДЫ!!");
                            for (int j = 0; j < playerhand.Length; j++)//Если колода закончилась, убираем лишние нули из рук
                            {
                                if (playerhand[j] == 0)
                                {
                                    playerhand = DeckLow(Array.IndexOf(playerhand, playerhand[j]), playerhand);
                                    j--;
                                }

                            }
                            for (int j = 0; j < brainhand.Length; j++)
                            {
                                if (brainhand[j] == 0)
                                {
                                    brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[j]), brainhand);
                                    j--;
                                }

                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 5; i >= 0; i--)
                {
                    if (brainhand[i] == 0)
                    {
                        if (deck.Length > 0)
                        {
                            brainhand[i] = deck[randomc.Next(0, deck.Length)];
                            deck = DeckLow(Array.IndexOf(deck, brainhand[i]), deck);
                        }
                        else
                        {
                            Console.WriteLine("КОНЕЦ КОЛОДЫ!!");
                            for (int j = 0; j < playerhand.Length; j++)//Если колода закончилась, убираем лишние нули из рук
                            {
                                if (playerhand[j] == 0)
                                {
                                    playerhand = DeckLow(Array.IndexOf(playerhand, playerhand[j]), playerhand);
                                    j--;
                                }

                            }
                            for (int j = 0; j < brainhand.Length; j++)
                            {
                                if (brainhand[j] == 0)
                                {
                                    brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[j]), brainhand);
                                    j--;
                                }

                            }
                            break;
                        }
                    }
                    if (playerhand[i] == 0)
                    {
                        if (deck.Length > 0)
                        {
                            playerhand[i] = deck[randomc.Next(0, deck.Length)];
                            deck = DeckLow(Array.IndexOf(deck, playerhand[i]), deck);
                        }
                        else
                        {
                            Console.WriteLine("КОНЕЦ КОЛОДЫ!!");
                            for (int j = 0; j < playerhand.Length; j++)//Если колода закончилась, убираем лишние нули из рук
                            {
                                if (playerhand[j] == 0)
                                {
                                    playerhand = DeckLow(Array.IndexOf(playerhand, playerhand[j]), playerhand);
                                    j--;
                                }

                            }
                            for (int j = 0; j < brainhand.Length; j++)
                            {
                                if (brainhand[j] == 0)
                                {
                                    brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[j]), brainhand);
                                    j--;
                                }

                            }
                            break;
                        }

                    }

                }
            }





        }//много повторяющегося кода, возможно нужно создать метод для убирания пустых ячеек из рук(!)
        //метод, обьявляющий кто ходит первый. У кого меньше козырь, тот и первый.
        public static bool playerfirst { get; set; }
        static bool takecards { get; set; }
        public void WhoFirst()
        {
            double[] PlayerTrumps = { 15, 15, 15, 15, 15, 15 };
            int plTrIndex = 0;
            double[] BrainTrumps = { 15, 15, 15, 15, 15, 15 };
            int brTrIndex = 0;


            for (int i = 0; i < playerhand.Length; i++)
            {
                if (Math.Round(playerhand[i] % 1, 1) == trumpvalue)
                {
                    PlayerTrumps[plTrIndex] = playerhand[i];
                    plTrIndex++;
                }
            }
            for (int i = 0; i < brainhand.Length; i++)
            {
                if (Math.Round(brainhand[i] % 1, 1) == trumpvalue)
                {
                    BrainTrumps[brTrIndex] = brainhand[i];
                    brTrIndex++;
                }
            }
            double minPlayer = PlayerTrumps.Min();
            double minBrain = BrainTrumps.Min();
            if (minPlayer == minBrain)
            {
                Console.WriteLine();
                Console.WriteLine("У игроков нет козырей на руках. Сдаю козырь заново");
                Console.WriteLine();
                MakeATrump();
                WhoFirst();
            }
            if (minPlayer < minBrain)
            {
                Console.WriteLine("Игрок,твой ход первый");
                playerfirst = true;
            }

            if (minPlayer > minBrain)
            {
                Console.WriteLine("Первый ход за Мозгом");
                playerfirst = false;
            }


        }
        void WantMore()
        {
            if (Win(playerhand))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("ПОБЕДА ИГРОКА!!!");
                return;
            }
            if (takecards)
            {
                Console.WriteLine("Докинешь карты? [1]- ДА / [0] - НЕТ");
                int anykey = int.Parse(Console.ReadLine());
                if (anykey == 0)
                {
                    Console.WriteLine("Компьютер забирает карты себе");
                    brainhand = brainhand.Concat(table).ToArray();
                    brainhand = brainhand.Concat(intermediate).ToArray();
                    Console.WriteLine("Козырь " + ShowCard(trump));
                    Console.WriteLine("Раздаю карты: ");
                    CardDrow();
                    playerhand = SortCard(playerhand);
                    Console.WriteLine("Карты компьютера: ");
                    Console.WriteLine();
                    ShowCard(brainhand);
                    Console.WriteLine();
                    Console.WriteLine("Карты игрока: ");
                    Console.WriteLine();
                    ShowCard(playerhand);
                    Console.WriteLine();
                    Array.Resize(ref table, 0);
                    tableindex = 0;
                    Array.Resize(ref intermediate, 0);
                    interindex = 0;
                    takecards = false;
                    cardcount = 0;
                }
                else
                {
                    ShowCard(playerhand);
                    ChoseCard();
                    brainhand = brainhand.Concat(table).ToArray();
                    brainhand = brainhand.Concat(intermediate).ToArray();
                    Console.WriteLine("Козырь " + ShowCard(trump));
                    Console.WriteLine("Раздаю карты: ");
                    CardDrow();
                    playerhand = SortCard(playerhand);
                    Console.WriteLine("Карты компьютера: ");
                    Console.WriteLine();
                    ShowCard(brainhand);
                    Console.WriteLine();
                    Console.WriteLine("Карты игрока: ");
                    Console.WriteLine();
                    ShowCard(playerhand);
                    Console.WriteLine();
                    Array.Resize(ref table, 0);
                    tableindex = 0;
                    Array.Resize(ref intermediate, 0);
                    interindex = 0;
                    takecards = false;
                    cardcount = 0;

                }

            }
            else
            {
                Console.WriteLine("Докинешь карты? [1]- ДА / [0] - НЕТ");
                int anykey = int.Parse(Console.ReadLine());
                if (anykey == 1)
                {
                    ShowCard(playerhand);
                    Array.Resize(ref table, 0);
                    tableindex = 0;
                    ChoseCard();
                    BeatThis();
                    WantMore();
                }
                else
                {
                    Console.WriteLine("Компьютер отбился! ХОД КОМПЬЮТЕРА!");
                    Console.WriteLine("Козырь " + ShowCard(trump));
                    Console.WriteLine("Раздаю карты: ");
                    CardDrow();
                    playerfirst = false;
                    playerhand = SortCard(playerhand);
                    Console.WriteLine("Карты компьютера: ");
                    Console.WriteLine();
                    ShowCard(brainhand);
                    Console.WriteLine();
                    Console.WriteLine("Карты игрока: ");
                    Console.WriteLine();
                    ShowCard(playerhand);
                    Console.WriteLine();
                    Array.Resize(ref table, 0);
                    tableindex = 0;
                    Array.Resize(ref intermediate, 0);
                    interindex = 0;
                    takecards = false;
                    cardcount = 0;

                }
            }
        }

        void BrainWantMore()
        {
            double[] wantmore = intermediate.Concat(table).ToArray();
            bool canmore = false;
            if (Win(playerhand))
            {
                Console.WriteLine();
                Console.WriteLine("ИГРОК ПОБЕДИЛ!!!");
                Console.WriteLine("ИГРОК ПОБЕДИЛ!!!");
                Console.WriteLine("ИГРОК ПОБЕДИЛ!!!");
                return;
            }
            if (takecards)
            {
                for (int i = 0; i < wantmore.Length; i++)
                {
                    for (int j = 0; j < brainhand.Length; j++)
                    {
                        if (table.Length + 1 < playerhand.Length)
                        {
                            if ((Math.Truncate(brainhand[j]) == Math.Truncate(wantmore[i])))
                            {
                                Console.WriteLine("Компьютер подкидывает на стол карту " + ShowCard(brainhand[j]));
                                Array.Resize(ref table, table.Length + 1);
                                table[tableindex] = brainhand[j];
                                brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[j]), brainhand);
                            }
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine("ИГРОКУ НЕЧЕМ ОТБИТЬСЯ,ОН ЗАБИРАЕТ КАРТЫ");
                playerhand = playerhand.Concat(table).ToArray();
                playerhand = playerhand.Concat(intermediate).ToArray();
                Console.WriteLine("ХОД КОМПЬЮТЕРА:");
                Console.WriteLine("Козырь " + ShowCard(trump));
                Console.WriteLine("Раздаю карты: ");
                CardDrow();
                playerhand = SortCard(playerhand);
                Console.WriteLine("Карты компьютера: ");
                Console.WriteLine();
                ShowCard(brainhand);
                Console.WriteLine();
                Console.WriteLine("Карты игрока: ");
                Console.WriteLine();
                ShowCard(playerhand);
                Console.WriteLine();
                Array.Resize(ref table, 0);
                tableindex = 0;
                Array.Resize(ref intermediate, 0);
                interindex = 0;
                takecards = false;
                cardcount = 0;
                return;
            }
            else
            {
                for (int i = 0; i < wantmore.Length; i++)
                {
                    for (int j = 0; j < brainhand.Length; j++)
                    {
                        if (table.Length + 1 < playerhand.Length)
                        {
                            if ((Math.Truncate(brainhand[j]) == Math.Truncate(wantmore[i])))
                            {
                                Console.WriteLine("Компьютер подкидывает на стол карту " + ShowCard(brainhand[j]));
                                Array.Resize(ref table, table.Length + 1);
                                table[tableindex] = brainhand[j];
                                brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[j]), brainhand);
                                canmore = true;
                            }
                        }
                    }
                }
                if (canmore)
                {
                    ShowCard(playerhand);
                    ChoseCard();
                    BrainWantMore();
                }
                else
                {

                    Console.WriteLine();
                    Console.WriteLine("БИТА! ХОД ИГРОКА");
                    Console.WriteLine("Козырь " + ShowCard(trump));
                    Console.WriteLine("Раздаю карты: ");
                    CardDrow();
                    playerfirst = true;
                    playerhand = SortCard(playerhand);
                    Console.WriteLine("Карты компьютера: ");
                    Console.WriteLine();
                    ShowCard(brainhand);
                    Console.WriteLine();
                    Console.WriteLine("Карты игрока: ");
                    Console.WriteLine();
                    ShowCard(playerhand);
                    Console.WriteLine();
                    Array.Resize(ref table, 0);
                    tableindex = 0;
                    Array.Resize(ref intermediate, 0);
                    interindex = 0;
                    takecards = false;
                    cardcount = 0;
                }

            }
        }
        bool Win(double[] hand)
        {
            bool over = false;
            if (deck.Length == 0 && hand.Length == 0)
            {
                over = true;
                return over;
            }
            else return over;
        }
        public void TheGame()
        {
            if (Win(playerhand))
            {
                Console.WriteLine();
                Console.WriteLine("ВЫ ПОБЕДИЛИ!!!");
                Console.WriteLine("ВЫ ПОБЕДИЛИ!!!");
                Console.WriteLine("ВЫ ПОБЕДИЛИ!!!");
                return;
            }
            if (Win(brainhand))
            {
                Console.WriteLine();
                Console.WriteLine("МОЗГ ПОБЕДИЛ!!!");
                Console.WriteLine("МОЗГ ПОБЕДИЛ!!!");
                Console.WriteLine("МОЗГ ПОБЕДИЛ!!!");
                return;
            }

            if (playerfirst)
            {
                ChoseCard();
                BeatThis();
                WantMore();
                TheGame();


            }
            else
            {
                BrainAttack();
                ChoseCard();
                BrainWantMore();
                TheGame();
            }
        }

        static int tableindex = 0;//индексатоs для рекурсивного метода ChoseCard.
        static int cardcount = 0;
        public void ChoseCard()
        {

            if (playerfirst)
            {
                Console.WriteLine("Выберите карту: ");
                int card = int.Parse(Console.ReadLine());
                for (int i = 0; i < playerhand.Length; i++)
                {
                    if (card == Array.IndexOf(playerhand, playerhand[i]))
                    {
                        Console.WriteLine("Вы положили на стол карту:  " + ShowCard(playerhand[i]));
                        Array.Resize(ref table, table.Length + 1);
                        table[tableindex] = playerhand[i];
                        playerhand = DeckLow(Array.IndexOf(playerhand, playerhand[i]), playerhand);
                        tableindex++;
                        break;
                    }
                }
                if (Win(playerhand))
                {
                    Console.WriteLine();
                    Console.WriteLine("ВЫ ПОБЕДИЛИ!!!");
                    Console.WriteLine("ВЫ ПОБЕДИЛИ!!!");
                    Console.WriteLine("ВЫ ПОБЕДИЛИ!!!");
                    return;
                }

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Выберите карту. Если вам нечем крыть, введите [20]");
                int card = int.Parse(Console.ReadLine());
                for (int i = 0; i < playerhand.Length; i++)
                {
                    if (card == Array.IndexOf(playerhand, playerhand[i]))
                    {
                        Array.Resize(ref intermediate, intermediate.Length + 1);
                        //Добавляем отбиваемую карту в промежуточный отбой
                        intermediate[interindex] = table[tableindex];
                        interindex++;
                        //Меняем карту на столе на карту,которой отбиваемся и удаляем её из руки
                        //Array.Resize(ref table, table.Length + 0);
                        Console.WriteLine($"Игрок бьёт карту {ShowCard(table[tableindex])} картой {ShowCard(playerhand[i])}");
                        table[tableindex] = playerhand[i];
                        playerhand = DeckLow(Array.IndexOf(playerhand, playerhand[i]), playerhand);
                        tableindex++;
                        cardcount++;
                        break;
                    }
                    if (Win(playerhand))
                    {
                        Console.WriteLine("ВЫ ПОБЕДИЛИ!!");
                        return;
                    }
                    else if (card == 20)
                    {
                        takecards = true;
                        return;
                    }
                }
            }
            if (playerfirst)
            {
                Console.WriteLine("Будете класть ещё карту? [0] ДА [1] НЕТ");
                int more = int.Parse(Console.ReadLine());
                if (more == 0)
                {
                    ShowCard(playerhand);
                    ChoseCard();
                }
                else
                {
                    Console.WriteLine("Хорошо, вы походили под компьютер картами");
                    ShowCard(table);
                }

            }
            else
            {
                if (cardcount < table.Length)
                {
                    Console.WriteLine();
                    ShowCard(playerhand);
                    ChoseCard();
                }
            }


        }

        public double[] SortCard(double[] hand)
        {
            double[] trumphand = { };

            int j = 0;
            int index = 1;
            //тут циклами создаю два массива для козырных и обычных карт и обрезаю лишние "пустые карты" методом DeckLow
            for (int i = 0; i < hand.Length; i++)
            {
                if (Math.Round(hand[i] % 1, 1) == trumpvalue)
                {
                    Array.Resize(ref trumphand, index);
                    trumphand[j] = hand[i];
                    hand = DeckLow(Array.IndexOf(hand, hand[i]), hand);
                    i--;
                    //Очень важный момент!! Если мы убираем элемент из массива, по котрому бежит цикл, то и индекс нужно задискрементить!!!
                    j++;
                    index++;
                }
            }

            //сортирую карты по возрастанию в обоих массивах
            Array.Sort(hand);
            Array.Sort(trumphand);
            //в "руке" сортирую карты по масти пузырьковым методом
            for (int i = 0; i < hand.Length; i++)
                for (int k = 0; k < hand.Length - 1; k++)
                    if (Math.Round(hand[k] % 1, 1) > Math.Round(hand[k + 1] % 1, 1))
                    {
                        double t = hand[k + 1];
                        hand[k + 1] = hand[k];
                        hand[k] = t;
                    }
            //обьединяю массивы и возвращаю отсортированную руку.
            double[] sorthand = hand.Concat(trumphand).ToArray();
            hand = sorthand;
            return hand;
        }

        void BeatThis() //как будто переусложнённый вариант, но потом попробую сократить
        {
            //индексатор для отбива
            int beatindexer = 0;
            //копии для операций
            double[] processhand = brainhand;
            double[] processtable = table;
            //проверяем есть ли козыри в руке и копируем их в отдельный массив, при этом убирая их из руки
            double[] trumps = TrumpOut(ref processhand);
            //проверяем, можем ли мы отбиться без козырей
            //здесь создам руку для отбива,чтобы в случае успешного отбива об этом обьявить и завершить работу метода
            double[] beathand = { };
            int beatind = 0;
            Array.Sort(processhand);
            for (int i = 0; i < processtable.Length; i++)
            {
                for (int j = 0; j < processhand.Length; j++)
                {
                    //ЕСЛИ МАСТЬ В РУКЕ РАВНА МАСТИ НА СТОЛЕ И(&) ДОСТОИНСТВО КАРТЫ В РУКЕ БОЛЬШЕ ДОСТОИНСТВА КАРТЫ НА СТОЛЕ
                    if (Math.Round(processhand[j] % 1, 1) == Math.Round(processtable[i] % 1, 1) &&
                        Math.Truncate(processhand[j]) > Math.Truncate(processtable[i]))
                    {
                        //Заносим побитую карту в руку для битья
                        Array.Resize(ref beathand, beathand.Length + 1);
                        beathand[beatind] = processhand[j];

                        processhand = DeckLow(Array.IndexOf(processhand, processhand[j]), processhand);
                        beatindexer++;
                        beatind++;
                        break;
                    }
                }
                if (beatindexer == table.Length)
                {
                    Console.WriteLine("Компьютер отбивается картами: ");
                    ShowCard(beathand);
                    intermediate = intermediate.Concat(beathand).ToArray();
                    table = beathand;
                    brainhand = processhand.Concat(trumps).ToArray();
                    if (Win(brainhand))
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("ПОБЕДА КОМПЬЮТЕРА!!!");
                    }
                    return;
                }
            }
            //проверяем побили или нет без козырей
            //если да то обьявляем,что отбито
            //если нет,то обьединяем козырную и не козырную часть руки и пробуем отбиться с козырями
            if (beatindexer < table.Length)
            {
                beatindexer = 0;
                beatind = 0;
                Array.Resize(ref beathand, 0);
                processhand = brainhand;
                processtable = table;
                Array.Sort(processhand);
                for (int i = 0; i < processtable.Length; i++)
                {
                    for (int j = 0; j < processhand.Length; j++)
                    {
                        //ЕСЛИ МАСТЬ В РУКЕ КОЗЫРЬ И КАРТА НА СТОЛЕ КОЗЫРНАЯ, ТО БЬЁМ, ПРИ УСЛОВИИ, ЧТО ДОСТОИНСТВО КОЗЫРЯ В РУКЕ БОЛЬШЕ ЧЕМ НА СТОЛЕ
                        if (Math.Round(processhand[j] % 1, 1) == trumpvalue && Math.Round(processtable[i] % 1, 1) == trumpvalue)
                        {
                            if (Math.Truncate(processhand[j]) > Math.Truncate(processtable[i]))
                            {
                                //Заносим побитую карту в руку для битья
                                Array.Resize(ref beathand, beathand.Length + 1);
                                beathand[beatind] = processhand[j];
                                processhand = DeckLow(Array.IndexOf(processhand, processhand[j]), processhand);
                                beatindexer++;
                                beatind++;
                                break;
                            }

                        }
                        else if (Math.Round(processhand[j] % 1, 1) == trumpvalue && Math.Round(processtable[i] % 1, 1) != trumpvalue)
                        {
                            //Заносим побитую карту в руку для битья
                            Array.Resize(ref beathand, beathand.Length + 1);
                            beathand[beatind] = processhand[j];
                            processhand = DeckLow(Array.IndexOf(processhand, processhand[j]), processhand);
                            beatindexer++;
                            beatind++;
                            break;

                        }
                    }

                    if (beatindexer == table.Length)
                    {
                        Console.WriteLine("Компьютер отбивается картами: ");
                        ShowCard(beathand);
                        intermediate = intermediate.Concat(beathand).ToArray();
                        table = beathand;
                        brainhand = processhand;
                        if (Win(brainhand))
                        {
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("ПОБЕДА КОМПЬЮТЕРА!!!");
                        }
                        return;
                    }
                }
            }

            if (beatindexer < table.Length)
            {
                Console.WriteLine("Компьютер забирает карты себе:");
                takecards = true;
            }
        }

        //Метод принимает в себя руку, вынимает из неё козыри, и возвращает массив козырей
        double[] TrumpOut(ref double[] array)
        {
            double[] trumphead = { };
            int trindex = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (Math.Round(array[i] % 1, 1) == trumpvalue)
                {
                    Array.Resize(ref trumphead, trumphead.Length + 1);
                    trumphead[trindex] = array[i];
                    array = DeckLow(Array.IndexOf(array, array[i]), array);
                    trindex++;
                    i--;
                }

            }
            return trumphead;
        }
        void BrainAttack()
        {
            //Отделим козырные карты  в отдельный массив, если они есть.
            Console.WriteLine();//потом перенести
            double[] trumps = TrumpOut(ref brainhand);
            bool hastwo = true;
            //Проверим, есть ли пары по достоинству некозырной части руки.Если есть, то меньшие из них кладём на стол
            //Для этого отсортируем их, карты выстроятся по возрастанию, и пары по достоинству встанут рядом.
            Array.Sort(brainhand);
            if ((table.Length + 2) <= playerhand.Length)
            {
                for (int i = 0; i < brainhand.Length - 1; i++)
                {
                    if (Math.Truncate(brainhand[i]) == Math.Truncate(brainhand[i + 1]))
                    {
                        Array.Resize(ref table, table.Length + 2);
                        table[tableindex] = brainhand[i];
                        Console.WriteLine($"Мозг положил на стол {ShowCard(brainhand[i])}");
                        brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[i]), brainhand);
                        tableindex++;
                        table[tableindex] = brainhand[i];
                        Console.WriteLine($"Мозг положил на стол {ShowCard(brainhand[i])}");
                        brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[i]), brainhand);
                        brainhand = brainhand.Concat(trumps).ToArray();
                        hastwo = false;
                        break;
                    }
                    if (Win(brainhand))
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("ПОБЕДА КОМПЬЮТЕРА!!!");
                        return;
                    }
                }
            }

            //Если некозырных пар нет, то обьединяем карты с козырями, и повторяем то же самое
            if ((table.Length + 2) <= playerhand.Length)
            {
                if (hastwo)
                {
                    brainhand = brainhand.Concat(trumps).ToArray();
                    Array.Sort(brainhand);
                    for (int i = 0; i < brainhand.Length - 1; i++)
                    {
                        if (Math.Truncate(brainhand[i]) == Math.Truncate(brainhand[i + 1]))
                        {
                            Array.Resize(ref table, table.Length + 2);
                            table[tableindex] = brainhand[i];
                            Console.WriteLine($"Мозг положил на стол {ShowCard(brainhand[i])}");
                            brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[i]), brainhand);
                            tableindex++;
                            table[tableindex] = brainhand[i];
                            Console.WriteLine($"Мозг положил на стол {ShowCard(brainhand[i])}");
                            brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[i]), brainhand);
                            hastwo = false;
                            break;
                        }

                    }
                }
                if (Win(brainhand))
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("ПОБЕДА КОМПЬЮТЕРА!!!");
                    return;
                }
            }
            //Если нет пар вообще,то ходим с маленькой некозырной и возвращаем козыри в руку
            if (table.Length + 1 <= playerhand.Length)
            {
                if (hastwo)
                {
                    trumps = TrumpOut(ref brainhand);
                    if (brainhand.Length == 0)
                    {
                        brainhand = brainhand.Concat(trumps).ToArray();
                        Array.Sort(brainhand);
                        Array.Resize(ref table, table.Length + 1);
                        table[tableindex] = brainhand[0];
                        Console.WriteLine($"Мозг положил на стол {ShowCard(brainhand[0])}");
                        brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[0]), brainhand);
                        brainhand = brainhand.Concat(trumps).ToArray();
                    }
                    else
                    {
                        Array.Sort(brainhand);
                        Array.Resize(ref table, table.Length + 1);
                        table[tableindex] = brainhand[0];
                        Console.WriteLine($"Мозг положил на стол {ShowCard(brainhand[0])}");
                        brainhand = DeckLow(Array.IndexOf(brainhand, brainhand[0]), brainhand);
                        brainhand = brainhand.Concat(trumps).ToArray();
                    }
                }

                if (Win(brainhand))
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("ПОБЕДА КОМПЬЮТЕРА!!!");
                    return;
                }
            }
            tableindex = 0;
        }
    }
    class Player : Deck
    {
        //класс Игрок, наследуется от Колоды. Он наследует уже созданную статическую колоду [deck] и основные методы для начала игры.
        public string PlayerName { get; set; }
        public void ShowMycard()
        {
            ShowCard(playerhand);
        }
        public void ShowDeckValue()
        {
            Console.WriteLine("Осталось в колоде " + deck.Length + " карт.");
        }

        //метод для сортировки карт в руке. Сортирует козыри по порядку с одной стороны, и карты по мастям по порядку - с другой.
        public void SortMyCard()
        {
            playerhand = SortCard(playerhand);
        }
    }
    //Класс Мозг, пока пустой, но все его размышления буду писать здесь
    class Brain : Deck
    {
        public string BrainName { get; set; }
        public void ShowBraincard()
        {
            ShowCard(brainhand);

        }
        public Brain()
        {
            Random random = new Random();
            BrainName = "Мозг" + Convert.ToString(random.Next(1, 100));
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            deck.CreateDeck();
            Console.WriteLine("Приветствую,игрок! Введи своё имя: ");
            Player player = new Player();
            player.PlayerName = Console.ReadLine();
            Console.WriteLine($"Ну что же,давай сыграем в Дурака, {player.PlayerName}");
            Brain brain = new Brain();
            Console.WriteLine("Твой противник: " + brain.BrainName);
            Console.WriteLine("Раздаю карты...");

            Console.WriteLine();
            deck.CardDrow();
            Console.WriteLine("Ваши карты: ");
            player.ShowMycard();
            Console.WriteLine();

            Console.WriteLine("Карты компьютера: ");
            brain.ShowBraincard();
            Console.WriteLine();

            deck.MakeATrump();
            Console.WriteLine();
            player.ShowDeckValue();
            Console.WriteLine();
            deck.WhoFirst();
            Console.WriteLine("Ваши карты отсортированы: ");
            Console.WriteLine();
            player.SortMyCard();
            player.ShowMycard();
            Console.WriteLine();
            Console.WriteLine("Да начнётся игра ");
            deck.TheGame();



        }
    }

}
