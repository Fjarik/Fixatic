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
	}
}
