using OnlineQuizConn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ConnectionManager
{
   public class DataContextHelper
    {
        public static OnlineQuizConnDB GetPPContextHelper(bool enableAutoSelect = true)
        {
            return (GetNewDataContext("OnlineQuizConn", enableAutoSelect));
        }

        private static OnlineQuizConnDB GetNewDataContext(string connectionStringName, bool enableAutoSelect)
        {
            OnlineQuizConnDB repository = new OnlineQuizConnDB(connectionStringName);
            repository.EnableAutoSelect = enableAutoSelect;
            //repository.ELHelperInstance = elHelperInstance;



            return (repository);
        }
    }
}
