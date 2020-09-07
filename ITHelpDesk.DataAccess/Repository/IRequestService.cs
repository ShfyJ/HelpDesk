using Dapper;
using ITHelpDesk.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ITHelpDesk.DataAccess.Repository
{
    public class IRequestService : IRequest
    {

        List<Request> _oNotifications = new List<Request>();
        public List<Request> GetNotifications(string flag, string State)
        {
            _oNotifications = new List<Request>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                var oNotis = con.Query<Request>("SELECT * FROM Request Inner JOIN Address on Request.AddressId = Address.AddressId WHERE Address.Flag='"
                    + flag + "' and rStatus='" +  State+ "'").ToList();
                
                if (oNotis != null && oNotis.Count() > 0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;
            }
        }
    }
}
