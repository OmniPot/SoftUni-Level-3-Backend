namespace BattleShips.Client.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;

    public class ApiException : Exception
    {
        public ApiException(HttpResponseMessage response)
        {
            this.Response = response;
        }

        public HttpResponseMessage Response { get; set; }

        public HttpStatusCode StatusCode
        {
            get { return this.Response.StatusCode; }
        }


        public IEnumerable<string> Errors
        {
            get { return this.Data.Values.Cast<string>().ToList(); }
        }


        public static ApiException Create(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            var anonymousErrorObject = new
            {
                message = "",
                ModelState = new Dictionary<string, string[]>()
            };

            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            var apiException = new ApiException(response);

            if (deserializedErrorObject.ModelState != null)
            {
                var errors = deserializedErrorObject.ModelState.Select(kvp => string.Join("\n", kvp.Value));
                for (var index = 0; index < errors.Count(); index++)
                {
                    apiException.Data.Add(index, errors.ElementAt(index));
                }
            }
            else
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                foreach (var keyValuePair in error)
                {
                    apiException.Data.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return apiException;
        }
    }
}