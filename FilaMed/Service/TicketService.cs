
using FilaMed.Model;

namespace FilaMed.Service
{
    internal class TicketService
    {
        private static int Count = 0;


        public static HospitalTicket GenerateTicket()
        {
            Count++;

            int remainder = Count % 3;

            string number = Count.ToString("D2");
            string letter = string.Empty;
            letter = remainder switch
            {
                1 => "A",
                2 => "B",
                0 => "C",
                _ => "Opção inválida"
            };

            string format = letter + number;

            var now = DateTime.Now;

            return new HospitalTicket(format, now);
        }
    }
}
