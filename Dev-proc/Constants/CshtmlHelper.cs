namespace Dev_proc.Constants
{
    public static class CshtmlHelper
    {
		public static string AvailablePlacesToName(int? availablePlaces)
		{
			if (availablePlaces != null)
			{
				return $"| available places: {availablePlaces}";
			}
			return "";
		}
	}
}
