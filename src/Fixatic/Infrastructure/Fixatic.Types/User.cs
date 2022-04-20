namespace Fixatic.Types
{
    public class User
    {
        public int UserId { get; set; }

        public DateTime Created { get; set; }

        public bool IsEnabled { get; set; }

        public string? Email { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string GetFullName()
        {
	        if (Firstname != null && Lastname != null)
	        {
		        return Firstname + " " + Lastname;
	        }
	        else
	        {
		        return " ";
	        }
        }
        
        // Note: this is important so the MudSelect can compare pizzas
        public override bool Equals(object o) {
	        var other = o as User;
	        return other?.UserId == UserId;
        }

        // Note: this is important too!
        public override int GetHashCode()
        {
	        return GetFullName().GetHashCode();
        }

        // Implement this for the Pizza to display correctly in MudSelect
        public override string ToString()
        {
	        return GetFullName();
        }
    }
}
