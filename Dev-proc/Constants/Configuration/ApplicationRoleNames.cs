using Dev_proc.Models.Identity;
using System;
using System.Collections.Generic;

namespace Dev_proc.Constants.Configuration
{
    public class ApplicationRoleNames
    {
        public const string Administrator = "Administrator";
        public const string Dean = "Dean";
        public const string Company = "Company";
        public const string Student = "Student";

        public static readonly Dictionary<RoleType, String> SystemRoleNamesDictionary = new()
        {
            [RoleType.Administrator] = ApplicationRoleNames.Administrator,
            [RoleType.Dean] = ApplicationRoleNames.Dean,
            [RoleType.Company] = ApplicationRoleNames.Company,
            [RoleType.Student] = ApplicationRoleNames.Student
        };

        public const string AdminAndDean = Administrator + "," + Dean;
        public const string AdminAndCompany = Administrator + "," + Company;

    }
}
