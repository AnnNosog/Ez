using System;

namespace Ez
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Training training = new Training();
                training.GoTraining();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}
