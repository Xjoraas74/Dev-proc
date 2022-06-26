using Dev_proc.Models.Enums;

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
		public static string CandidatureStatusColor(CandidateStatus status)
		{
			switch (status)
			{
				case CandidateStatus.New:
					return "btn-primary";
				case CandidateStatus.Interview:
					return "btn-warning";
				case CandidateStatus.Accepted:
					return "btn-success";
				case CandidateStatus.Denied:
					return "btn-danger";
				case CandidateStatus.Undefiend:
					return "btn-dark";
				case CandidateStatus.Offer:
					return "btn-info";
				default:
					return "btn-light";
			}

		}
	}
}
