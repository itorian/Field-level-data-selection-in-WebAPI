using EmptyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace EmptyWebAPI.Controllers
{
    public class StudentController : ApiController
    {
        // Data source
        List<Student> db = new List<Student>();
        public StudentController()
        {
            db = new List<Student>
            {
                new Student { Id = 1, Name = "Abhimanyu K Vatsa", City = "Bokaro" },
                new Student { Id = 2, Name = "Deepak Kumar Gupta", City = "Bokaro" },
                new Student { Id = 3, Name = "Manish Kumar", City = "Muzaffarpur" },
                new Student { Id = 4, Name = "Rohit Ranjan", City = "Patna" },
                new Student { Id = 5, Name = "Shiv", City = "Motihari" },
                new Student { Id = 6, Name = "Rajesh Singh", City = "Dhanbad" },
                new Student { Id = 7, Name = "Staya", City = "Bokaro" },
                new Student { Id = 8, Name = "Samiran", City = "Chas" },
                new Student { Id = 9, Name = "Rajnish", City = "Bokaro" },
                new Student { Id = 10, Name = "Mahtab", City = "Dhanbad" },
            };
        }

        //public IHttpActionResult Get()
        //{
        //    return Ok(db);
        //}

        public IHttpActionResult Get(string fields = null)
        {
            try
            {
                List<string> lstFields = new List<string>();
                if (fields != null)
                {
                    lstFields = fields.ToLower().Split(',').ToList();
                }
                return Ok(db.Select(i => CreateShappedObject(i, lstFields)));

            }
            catch(Exception)
            {
                return InternalServerError();
            }
        }





        public object CreateShappedObject(object obj, List<string> lstFields)
        {
            if (!lstFields.Any())
            {
                return obj;
            }
            else
            {
                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in lstFields)
                {
                    var fieldValue = obj.GetType()
                        .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .GetValue(obj, null);

                    ((IDictionary<string, object>)objectToReturn).Add(field, fieldValue);
                }

                return objectToReturn;
            }
        }
    }
}
