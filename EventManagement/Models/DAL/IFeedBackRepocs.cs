using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagement.Models.DAL
{
    internal interface IFeedBackRepocs
    {
        IEnumerable<feedbacktable> GetEmployees();
        feedbacktable GetEmployeeById(int EmployeeId);
        void InsertEmployee(feedbacktable employee_);
        void UpdateEmployee(feedbacktable employee_);
        void DeleteEmployee(int EmployeeId);
        void SaveChanges();
    }
}