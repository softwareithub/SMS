using SERP.Utilities.ResponseUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERP.Utilities.ResponseMessage
{
    public sealed class ResponseData
    {
        private static ResponseData instance = null;
        private ResponseData()
        {
        }
        public static ResponseData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResponseData();
                }
                return instance;
            }
        }

        public  string GenericResponse(ResponseStatus responseStatus)
        {
            switch(responseStatus)
            {
                case ResponseStatus.AddedSuccessfully:
                    return "Record added successfully";
                case ResponseStatus.AlreadyExists:
                    return "Record already exists";
                case ResponseStatus.DeletedSuccessfully:
                    return "Record deleted successfully";
                case ResponseStatus.ServerError:
                    return "Server error please contact admin team";
                case ResponseStatus.UpdatedSuccessFully:
                    return "Record updated successfully";
                default:
                    return string.Empty;
            }
        }
    }
}
