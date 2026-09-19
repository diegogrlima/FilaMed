
using FilaMed.Model;

namespace FilaMed.Service
{
    internal class QueueService
    {
        private readonly Queue<HospitalTicket> _queue = new();

      

        public void AddTicket(HospitalTicket ticket)
        {
            _queue.Enqueue(ticket);
        }

        public int CountTickets()
        {
            return _queue.Count;
        }

        public List<HospitalTicket> GetAllTickets()
        {
            return [.. _queue];
        }


        public HospitalTicket CallNextTicket()
        {
            // chama o proximo ticket
            return _queue.Dequeue();

        }
    }

    
}
