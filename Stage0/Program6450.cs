namespace Stage0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome6450();
            Welcome2735();
            //Enter your name:
            //yokheved
            //yokheved, welcome to my first console application
        }

        private static void Welcome6450()
        {
            Console.WriteLine("Enter your name:  ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}