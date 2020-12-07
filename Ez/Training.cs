using System;
using System.Collections.Generic;

namespace Ez
{
    internal delegate void heightCorrect(Plan plan);
    class Training
    {
        #region Fields
        private static Plan _plan;
        private heightCorrect _changeFly;
        private bool _endFly;
        private List<Dispatcher> _report;
        #endregion

        #region Properties
        public bool EndFly { get { return _endFly; } private set { _endFly = value; } }
        public List<Dispatcher> Report { get { return _report; } set { _report = value; } }
        public static Plan Plan { get { return _plan; } set { _plan = value; } }
        #endregion

        #region Constructors
        public Training()
        {
            if (Plan != null)
            {
                throw new ArgumentException("Самолёт занят");
            }
            Plan = Plan.GetInstance();
            Report = new List<Dispatcher>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Добавление диспетчера
        /// </summary>
        /// <param name="name">Имя диспетчера</param>
        private void AddDispatcher(string name)
        {
            Dispatcher dispatcher = new Dispatcher(name);
            Plan.Dispatchers.Add(dispatcher);
            Report.Add(dispatcher);
            _changeFly += dispatcher.GetCorrectHeight;
            _changeFly += dispatcher.CalcPenaltyPoint;
        }

        /// <summary>
        /// Удаление диспетчера
        /// </summary>
        /// <param name="index">Порядковый номер диспетчера, который будет удалён</param>
        private void DeleteDispatcher(int index)
        {
            int correctIndex = index - 1;

            if (correctIndex < 0)
            {
                throw new ArgumentException("Некорректный выбор диспетчера для удаления.");
            }

            if (Plan.Dispatchers.Count == 0)
            {
                throw new ArgumentException("Список диспетчеров пуст.");
            }

            if (correctIndex < Plan.Dispatchers.Count)
            {
                _changeFly -= Plan.Dispatchers[correctIndex].GetCorrectHeight;
                _changeFly -= Plan.Dispatchers[correctIndex].CalcPenaltyPoint;
                Plan.Dispatchers.RemoveAt(correctIndex);
            }
            else
            {
                throw new ArgumentException("Некорректный выбор диспетчера для удаления.");
            }
        }

        /// <summary>
        /// Основной метод тренировки 
        /// </summary>
        public void GoTraining()
        {
            Console.WriteLine("«Тренажер пилота самолета»");
            Console.WriteLine("Задача пилота – взлететь на самолете, набрать максимальную(1000 км/ч.) скорость, а затем посадить самолет.");
            Console.WriteLine("Самолет может лететь, если его контролируют минимум 2 диспетчера.");
            Console.WriteLine("Для начала полёта нажмите 'y', для выхода из тренировки 'q'");
            char input = Convert.ToChar(Console.ReadLine());
            while (input != 'y' && input != 'q')
            {
                Console.WriteLine("Неверный ввод.");
                input = Convert.ToChar(Console.ReadLine());
            }

            if (input == 'q')
            {
                EndTraining();
                Console.WriteLine("Тренировка прекращена");
                return;
            }

            Console.Clear();

            Console.Write("Введите имя первого пилота:");
            AddDispatcher(Console.ReadLine());

            Console.Write("Введите имя второго пилота:");
            AddDispatcher(Console.ReadLine());
            Console.Clear();

            while (true)
            {
                while (Plan.Dispatchers.Count < 2)
                {
                    Console.WriteLine("Необходимо минимум два пилота! Добавьте пилотов.");
                    Console.Write("Введите имя пилота:");
                    string inp = Console.ReadLine();
                    AddDispatcher(inp);
                    continue;
                }

                Console.WriteLine("----Управление----");
                Console.WriteLine("Скорость - изменяется клавишами-стрелками Left(A) и Right(D):\n(Right: +50км/ч, Left: -50км/ч, Shift - Right : +150 км/ч, Shift - Left: -150 км/ч).");
                Console.WriteLine("Высота -  изменяется клавишами-стрелками Up(W) и Down(S):\n(Up: +250м, Down: -250м, Shift - Up: +500м, Shift - Down: -500м).\n");
                Console.WriteLine("Для добавления диспетчера нажмите 'I', для удаления - 'U'");

                for (int i = 0; i < 50; i++)
                {
                    Console.Write('-');
                }
                Console.WriteLine();

                if (Plan.Speed == 1000)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("----Вы набрали максимальную скорость. Посадите самолёт.----");
                    Console.ResetColor();
                    EndFly = true;
                }
                else if (Plan.Speed > 1000)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("----Снизьте скорость.----");
                    Console.ResetColor();
                }
                else if (Plan.Speed == 0 && Plan.Heigt == 0 && EndFly)
                {
                    Console.Clear();
                    Console.WriteLine("Вы посадили самолёт. Поздравляем.");

                    int totalPenaltyPoint = 0;

                    foreach (var des in Report)
                    {
                        Console.WriteLine($"Штрафные очки {des.Name} - {des.PenaltyPoint}");
                        totalPenaltyPoint += des.PenaltyPoint;
                    }

                    Console.WriteLine($"Всего штрафных очков: {totalPenaltyPoint}");

                    EndTraining();
                    break;
                }

                if (Plan.Speed > 50)
                {
                    _changeFly(Plan);
                }

                Console.WriteLine($"Текущай высота: {Plan.Heigt}");
                Console.WriteLine($"Текущай скорость: {Plan.Speed}");

                ConsoleKeyInfo button;

                /*Для того, чтобы Shift срабатывал со стрелками, необходима нижняя строка.
                Но, он занимает поток, из за которого приходится два раза Enter нажимать и считвается раньше, чем надо
                Вынесла управление так же на WASD*/
                //Console.TreatControlCAsInput = true;

                button = Console.ReadKey(true);

                if ((button.Modifiers & ConsoleModifiers.Shift) != 0)
                {
                    if (button.Key == ConsoleKey.RightArrow || button.Key == ConsoleKey.D)
                    {
                        Plan.Speed += 150;
                    }
                    else if (button.Key == ConsoleKey.LeftArrow || button.Key == ConsoleKey.A)
                    {
                        Plan.Speed -= 150;
                    }
                    else if (button.Key == ConsoleKey.UpArrow || button.Key == ConsoleKey.W)
                    {
                        Plan.Heigt += 500;
                    }
                    else if (button.Key == ConsoleKey.DownArrow || button.Key == ConsoleKey.S)
                    {
                        Plan.Heigt -= 500;
                    }
                }
                else
                {
                    if (button.Key == ConsoleKey.RightArrow || button.Key == ConsoleKey.D)
                    {
                        Plan.Speed += 50;
                    }
                    else if (button.Key == ConsoleKey.LeftArrow || button.Key == ConsoleKey.A)
                    {
                        Plan.Speed -= 50;
                    }
                    else if (button.Key == ConsoleKey.UpArrow || button.Key == ConsoleKey.W)
                    {
                        Plan.Heigt += 250;
                    }
                    else if (button.Key == ConsoleKey.DownArrow || button.Key == ConsoleKey.S)
                    {
                        Plan.Heigt -= 250;
                    }
                    else if (button.Key == ConsoleKey.I)
                    {
                        Console.Write("Введите имя пилота:");
                        AddDispatcher(Console.ReadLine());
                        Console.Clear();
                        continue;
                    }
                    else if (button.Key == ConsoleKey.U)
                    {
                        Console.WriteLine("Введите номер диспетчера, которог нужно удалить: ");
                        DeleteDispatcher(Convert.ToInt32(Console.ReadLine()));
                        Console.Clear();
                        continue;
                    }
                }

                if (Plan.Speed == 0 && Plan.Heigt > 0)
                {
                    throw new Exception("Невозможно находится на высоте с нулевой скоростью.");
                }

                Console.Clear();
            }
        }

        /// <summary>
        /// Метод окончания тренировки 
        /// </summary>
        private void EndTraining()
        {
            Plan = null;
        }

        #endregion
    }
}
