using System;

namespace DevOpsDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            int retention;
            
            Console.WriteLine("Starting DevOps-Deploy!");
            Console.WriteLine("Enter Rentention Number: ");
            string retentionInput = Console.ReadLine();

            if (Int32.TryParse(retentionInput, out retention))
            {
                if (retention > 0)
                {
                    ReleaseRetention.GetRetained(retention);
                }
                else
                {
                    Console.WriteLine("Retention number needs be greater than 0");
                }
            } 
            else
            {
                Console.WriteLine("Retention number needs to be a integer");
            } 

        }
    }
}
