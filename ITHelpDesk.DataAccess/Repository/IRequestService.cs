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
                    + flag + "' and rStatus='" + State + "'").ToList();

                if (oNotis != null && oNotis.Count() > 0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;
            }
        }
        public List<Request> GetEvents()
        {
            _oNotifications = new List<Request>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                var oNotis = con.Query<Request>("SELECT Request.RequestId, Request.RName, Request.RDescription, Request.RStatus, Request.RDateTime, Request.Completed_at, AspNetUsers.FName " +
                    "FROM Request LEFT JOIN Workers on  Workers.WorkerId = Request.WorkerId LEFT JOIN AspNetUsers on Workers.UserId =AspNetUsers.Id ").ToList();

                if (oNotis != null && oNotis.Count() > 0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;
            }
        }

        public List<Request> GetRequester(int id, string rStatus)
        {
            _oNotifications = new List<Request>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                var oNotis = con.Query<Request>("SELECT * FROM Request WHERE RequestmakerId=" + id + " and rStatus='" + rStatus + "'").ToList();

                if (oNotis != null && oNotis.Count() > 0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;
            }
        }

        public List<Request> GetEmployee(int id, string rStatus)
        {
            _oNotifications = new List<Request>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                var oNotis = con.Query<Request>("SELECT * FROM Request WHERE WorkerId=" + id + " and rStatus='" + rStatus + "'").ToList();

                if (oNotis != null && oNotis.Count() > 0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;
            }
        }

        public List<Request> GetAll()
        {
            _oNotifications = new List<Request>();
            using (IDbConnection con = new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                var oNotis = con.Query<Request>("SELECT * FROM Request WHERE rStatus='blue' OR rStatus='Taken'").ToList();

                if (oNotis != null && oNotis.Count() > 0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;
            }
        }
    }
}
