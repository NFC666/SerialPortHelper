using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SertailTest.Models
{
    class Ticket
    {

        public Ticket(string cashier, List<Common> commons,float income)
        {
            id = Guid.NewGuid();
            createAt = DateTime.Now;
            this.commons = commons;
            totalCommonAmout = commons.Count;
            foreach (var item in commons)
            {
                totalCommonValue += item.commonValue;
            }
            foreach (var item in commons)
            {
                totalCommonValueAfterDiscount += item.paid;
            }
            this.income = income;
            net = income - totalCommonValueAfterDiscount;
            this.cashier = cashier;
        }

        Guid id;
        DateTime createAt;
        List<Common> commons;
        int totalCommonAmout;
        float totalCommonValue;
        float totalCommonValueAfterDiscount;
        float income;
        float net;
        string cashier;

    }

    class Common
    {
        public string commonName {  get; set; }
        public int commonAmount {  get; set; }
        public float commonValue {  get; set; }
        public float paid {  get; set; }
    }
    
}
