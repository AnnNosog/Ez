using System;

namespace Ez
{
    internal class Dispatcher
    {
        #region Fields
        private string _name;
        private int _correctHeight;
        private int _correct;
        private int _penaltyPoint;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            private set
            {
                if (value != null)
                {
                    _name = value;
                }
                throw new ArgumentException("Недопустимое имя");
            }
        }
        public int CorrectHeight { get { return _correctHeight; } private set { _correctHeight = value; } }
        public int Correct { get { return _correct; } private set { _correct = value; } }
        public int PenaltyPoint { get { return _penaltyPoint; } private set { _penaltyPoint = value; } }
        #endregion

        #region Constructors
        public Dispatcher(string name)
        {
            Random rand = new Random();
            Name = name;
            Correct = rand.Next(-200, 200);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Расчёт рекомендуемой высоты
        /// </summary>
        /// <param name="plan"></param>
        public void GetCorrectHeight(Plan plan)
        {
            CorrectHeight = 7 * plan.Speed - Correct;
            Console.WriteLine($"Pекомендуемая высота диспетчера {Name}: {CorrectHeight}");
        }

        /// <summary>
        /// Расчёт штрафных очков
        /// </summary>
        /// <param name="plan"></param>
        public void CalcPenaltyPoint(Plan plan)
        {
            int differenceHeight = Math.Abs(plan.Heigt - CorrectHeight);

            if (plan.Speed > 1000)
            {
                PenaltyPoint += 100;
            }

            if (differenceHeight >= 300 && differenceHeight < 600)
            {
                PenaltyPoint += 25;
            }
            else if (differenceHeight >= 600 && differenceHeight < 1000)
            {
                PenaltyPoint += 50;
            }
            else if (differenceHeight == 1000)
            {
                throw new ArgumentException("---Непригоден к полётам.---");
            }
            else if (differenceHeight > 1000)
            {
                throw new ArgumentException("---Самолёт разбился.---");
            }
        }
        #endregion
    }
}
