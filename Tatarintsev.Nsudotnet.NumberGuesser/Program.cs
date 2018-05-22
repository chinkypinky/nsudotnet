
using System;

namespace Tatarintsev.Nsudotnet.NumberGuesser
{
    class Program
    {

        static int Main(string[] args)
        {
            Console.WriteLine("Введи свое имя.");
            string name = Console.In.ReadLine(), tmpString;
            string[] history = new string[1000];
            string[] looserPhrases = { String.Format("{0}! Едрить ты бестолочь.",name) ,
               String.Format( "{0}! Даже моя бабушка быстрее тебя угадала.",name),
                String.Format("{0}! Мдаа... С таким IQ как ты вообще компьютер включил.",name),
                String.Format("{0}! Лучше сдайся. Не позорь себя...",name)};
            int randomNumber = new Random().Next(101), curNumber,forth=0;
            Console.WriteLine("Погнали! Введи число.");
            DateTime begin =  DateTime.Now;
            while (true)
            {
                if (int.TryParse(tmpString = Console.In.ReadLine(), out curNumber))
                {
                    if (curNumber < randomNumber)
                    {
                        Console.WriteLine("Нужно взять побольше.");
                        history[forth] = "меньше";
                    }
                    else if (curNumber > randomNumber)
                    {
                        Console.WriteLine("Нужно взять поменьше.");
                        history[forth] = "больше";
                    }
                    else
                    {
                        DateTime end = DateTime.Now;
                        Console.WriteLine("Ну наконец-то! Боже мой, с {0} попытки... И не прошло и {1} минут. А нет, прошло... ",++forth,(end-begin).Minutes+(end-begin).Hours*60);
                        foreach(string i in history)
                        {
                            if (i == null)
                                break;
                            Console.WriteLine(i);
                        }
                        break;
                    }
                }
                else if (tmpString == "q")
                {
                    Console.WriteLine("Извини, но я спешу. Пока!");
                    return 0;
                }
                else
                    Console.WriteLine("Сказали же тебе, что надо число ввести,болван!");
                forth++;
                if (forth % 4 == 0)
                    Console.WriteLine(looserPhrases[new Random().Next(4)]);
            }
            return 0;
        }
    }
}
