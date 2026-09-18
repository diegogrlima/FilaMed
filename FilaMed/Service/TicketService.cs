
using FilaMed.Model;

namespace FilaMed.Service
{
    internal class TicketService
    {
        private static int Count = 0;


        public static HospitalTicket GeneratePassword()
        {
            Count++;

            int remainder = Count % 3;

            string number = Count.ToString("D2");
            string letter = string.Empty;


            if (remainder == 1)
            {
                letter = "A";
            }
            else if (remainder == 2)
            {

                letter = "B";
            }
            else if (remainder == 3)
            {
                letter = "C";
            }

            string format = letter + number;

            var now = DateTime.Now;

            return new HospitalTicket(format, now);
        }
    }
}
