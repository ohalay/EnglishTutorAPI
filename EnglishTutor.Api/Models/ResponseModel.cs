using System.Collections.Generic;

namespace EnglishTutor.Api.Models
{
    public class ResponseModel<T>
    {
        public IEnumerable<T> Result { get; set; }

        public int? Total { get; set; }
    }
}
