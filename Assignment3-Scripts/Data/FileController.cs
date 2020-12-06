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
            // Test if the CSV data was successfully loaded
            //Debug.Log("customer type: " + one_customer.Type);
            //Debug.Log("customer time: " + one_customer.Time);
            //Debug.Log("customer service name: " + one_customer.ServiceName);
        }
        // Test if the customer_list was constructed as planned
        for (int w=0; w < customer_list.Count; w++){
          for (int t=0; t < customer_list[w].Count; t++){
            print(customer_list[w][t]);
        }
      }
    }
}
