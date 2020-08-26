using System;
using System.Collections.Generic;
using System.Text;

namespace ContractGenerator.Latex
{
    interface IContract
    {
        string CompanyName { get; set; }
        string EmployeeName { get; set; }
        string AddressLineOne{get; set;}
        string AddressLineTwo { get; set; }
        string EmployeeEmail { get; set; }
        string HourlyRate { get; set; }
    }
}
