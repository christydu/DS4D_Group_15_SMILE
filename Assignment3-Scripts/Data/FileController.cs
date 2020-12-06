using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerList: List<Customer> {}

    public class Customer: List<string> {
      public string Type {get; set;}
      public string Time {get; set;}
      public string ServiceName {get; set;}
    }

    static public CustomerList customer_list;


public class FileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CustomerList customer_list = new CustomerList();
        CSV.GetInstance().loadFile(Application.dataPath + "/Fruit-Juice-Maker/Res", "testcsv.csv");
        for (int i = 1; i < CSV.GetInstance().m_ArrayData.Count; i++)
        {
            Customer one_customer = new Customer();
            one_customer.Type = CSV.GetInstance().getString(i,0);
            one_customer.Time = CSV.GetInstance().getString(i,1);
            one_customer.ServiceName = CSV.GetInstance().getString(i,2);
        }
    }
}
