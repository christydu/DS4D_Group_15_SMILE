using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CSV
{
      static CSV csv;
      public List<string> m_ArrayData;

      public static CSV GetInstance()
      {
          if (csv == null)
          {
              csv = new CSV();
          }
          return csv;
      }

      private CSV() { m_ArrayData = new List<string> (); }

      public void loadFile(string path, string filename)
      {
          m_ArrayData.Clear();
          StreamReader sr = null; // the original data read from the file
          try
          {
              sr = File.OpenText(path + "//" + filename);
              Debug.Log("file finded!");
          }
          catch
          {
              Debug.Log("file not finded!");
              return;
          }
          string line;
          while((line=sr.ReadLine()) != null)
          {
              m_ArrayData.Add(line);
          }
          sr.Close();
          sr.Dispose();
      }

}
