using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.Models.ViewModels
{
     class WorkerDetailModel
    {
        public int R_id { get; set; }
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get
            {
                return FirstName + " " + LastName;
            } }
        public int score { get; set; }
        public string State { get; set; }
        public IEnumerable<Request> RequestsPerformed { get; set; }
    }
}
