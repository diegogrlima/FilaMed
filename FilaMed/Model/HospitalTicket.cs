
namespace FilaMed.Model
{
    internal class HospitalTicket
    {
        public string Name { get; }
        public DateTime IssuedAt { get; }

        public HospitalTicket(string name, DateTime issuedAt)
        {
            Name = name;
            IssuedAt = issuedAt;
        }


        public override string ToString()
        {
            return $"{Name} | {IssuedAt:HH:mm} ";
        }
    }
}

