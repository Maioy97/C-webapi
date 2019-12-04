using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDAtaAccess;

namespace tutorialapi.Controllers
{
    public class EmployeeController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var  entity= entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);

                }
                else {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id "+id+"not found");
                        }
            }
        }
        public HttpResponseMessage Post([FromBody] Employee emp)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(emp);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                    message.Headers.Location = new Uri(Request.RequestUri + "/"+emp.ID.ToString());
                    return message;
                }
            }

            catch (Exception ex){
               return  Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }
        public HttpResponseMessage Delete(int id) 
        {
            try {
                using (EmployeeDBEntities entity = new EmployeeDBEntities()) {
                    var lambdareturn = entity.Employees.FirstOrDefault(e => e.ID == id);
                    if (lambdareturn != null) {
                        entity.Employees.Remove(lambdareturn);
                        entity.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with id " + id.ToString() + "not found");
                    }
                }

            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
