using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _PageSize = 10;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public string OrderBy { get; set; }

        public string Fields { get; set; }
    }
}
