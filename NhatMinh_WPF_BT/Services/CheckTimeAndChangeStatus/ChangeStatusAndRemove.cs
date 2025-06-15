using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class ChangeStatusAndRemove
    {
        public bool Change<T>(List<T> list, IRepository<T> repository) where T : IHasDateAndStatus
        {
            if (list == null)
                return false;
            foreach (T item in list)
            {
                if (item.GetDateToCheck().Date < DateTime.Now.Date)
                {
                    item.Status = false;
                    repository.Update(item);
                }
            }
            return true;
        }

        public bool Remove<T>(List<T> list) where T : IHasDateAndStatus
        {
            if (list == null || list.Count == 0)
                return false;

            int removedCount = list.RemoveAll(item => item.Status == false);

            return removedCount > 0;
        }
    }
}
