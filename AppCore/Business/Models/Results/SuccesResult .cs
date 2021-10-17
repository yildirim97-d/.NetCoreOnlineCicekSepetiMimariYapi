using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Business.Models.Results
{
   public class SuccesResult : Result
    {
        public SuccesResult(string message):base (ResultStatus.Succes,message)

        {

        }
        public SuccesResult():base(ResultStatus.Succes,"")
        {

        }
        
    }
    public class SuccesResult<TResultType> : Result<TResultType>
    {
        public SuccesResult(string message,TResultType data) : base(ResultStatus.Succes,message,data)
        {

        }
        public SuccesResult(string message) : base(ResultStatus.Succes, message, default)
        {

        }

        public SuccesResult(TResultType data) : base(ResultStatus.Succes, "", data)
        {

        }
        public SuccesResult():base(ResultStatus.Succes, "", default)
        {

        }
    }
}
