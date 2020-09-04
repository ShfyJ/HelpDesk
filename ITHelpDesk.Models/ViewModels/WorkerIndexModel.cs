using System;
using System.Collections.Generic;
using System.Text;

namespace ITHelpDesk.Models.ViewModels
{
    class WorkerIndexModel
    {
        public IEnumerable<WorkerDetailModel> Workers { get; set; }
    }
}
