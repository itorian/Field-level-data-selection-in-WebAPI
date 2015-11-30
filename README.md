# Field-level-data-selection-in-WebAPI
This project is all about field level selection support by WebAPI. Assume your client is interested in only selected field (by default we send all the fields), this project will help you to achieve this functionality.
Look into below code, your client will send fields in query string like http://localhost:36140/api/student?fields=id,name and below method will return only id and name fields in response.


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
