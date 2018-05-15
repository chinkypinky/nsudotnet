
using System;

namespace Tatarintsev.Nsudotnet.NumberGuesser
{
    class Program
    {

        static int Main(string[] args)
        {
            string[] history = new string[1000];
            string[] looserPhrases = { "Едрить ты бестолочь." ,
                "Даже моя бабушка быстрее тебя угадала.",
                "Мдаа... С таким IQ как ты вообще компьютер включил.",
                "Лучше сдайся. Не позорь себя..."};
            Console.WriteLine("Введи свое имя.");
            string name= Console.In.ReadLine(),tmpString;
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
                        Console.WriteLine("Ну наконец-то! Боже мой, с {0} попытки... И не прошло и {1} минут. А нет, прошло... ",++forth,(end-begin).Minutes);
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
                    Console.WriteLine("{0}! {1}",name,looserPhrases[new Random().Next(4)]);
            }
            return 0;
        }
    }
}
