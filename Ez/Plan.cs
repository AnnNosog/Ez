using System;
using System.Collections.Generic;

namespace Ez
{
    internal class Plan
    {
        #region Fields
        private int _speed;
        private int _heigt;
        private List<Dispatcher> _dispatchers;
        private static Plan _instance;
        #endregion

        #region Properties
        public int Speed
        {
            get { return _speed; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Скорость не может быть отрицательная");
                }

                _speed = value;
            }
        }
        public int Heigt
        {
            get { return _heigt; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Высота не может быть отрицательная");
                }

                _heigt = value;
            }

        }
        public static Plan Instance { get { return _instance; } private set { _instance = value; } }
        public List<Dispatcher> Dispatchers { get { return _dispatchers; } set { _dispatchers = value; } }
        #endregion

        #region Constructors
        public Plan()
        {
            Dispatchers = new List<Dispatcher>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Реализация паттерна Singleton
        /// </summary>
        /// <returns></returns>
        public static Plan GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Plan();
            }

            return Instance;
        }
        #endregion
    }
}
