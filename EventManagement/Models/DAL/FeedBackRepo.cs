using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagement.Models.DAL
{
    
    public class FeedBackRepo : IFeedBackRepocs
    {
        private EventManagement2Entities2 _dataContext;

        public FeedBackRepo(EventManagement2Entities2 dataContext)
        {
            this._dataContext = dataContext;
        }
        public void DeleteEmployee(int EmployeeId)
        {
            feedbacktable employee_ = _dataContext.feedbacktables.Find(EmployeeId);
            _dataContext.feedbacktables.Remove(employee_);
        }
        public feedbacktable GetEmployeeById(int EmployeeId)
        {
            return _dataContext.feedbacktables.Find(EmployeeId);
        }
        public IEnumerable<feedbacktable> GetEmployees()
        {
            return _dataContext.feedbacktables.ToList();
        }

        public void InsertEmployee(feedbacktable employee_)
        {
            _dataContext.feedbacktables.Add(employee_);
        }
        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
        public void UpdateEmployee(feedbacktable employee_)
        {
            _dataContext.Entry(employee_).State = System.Data.Entity.EntityState.Modified;
        }
    }
}