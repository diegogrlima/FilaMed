
namespace FilaMed.Model
{
    internal class HospitalTicket
    {
        private string Name {  get; set; } = string.Empty;
        private DateTime IssuedAt { get; set; }

        public HospitalTicket(string name, DateTime issuedAt)
        {
            Name = name;
            IssuedAt = issuedAt;
        }


        public override string ToString()
        {
            return $"{Name} | {IssuedAt:HH:MM} ";
        }
    }
}
