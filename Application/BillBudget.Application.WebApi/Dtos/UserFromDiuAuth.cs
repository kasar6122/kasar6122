namespace BillBudget.Application.WebApi.Dtos
{
    public class UserFromDiuAuth
    {
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public string Department { get; set; }
        public string Designation { get; set; }
        public string SupervisorId { get; set; }

        public string SupervisorName { get; set; }
        public string SupervisorDepartment { get; set; }
        public string SupervisorDesignation { get; set; }

    }

}
