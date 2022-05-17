using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.GeneralOptions
{
    public class PagingOptions
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be equal or greater than {1}.")]
        public int PageNum { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must equal or be greater than {1}.")]
        public int PageSize { get; set; }
        public int NumPages { get; private set; }

        /// <summary>
        /// Checks if there enough entities in query for current paging optrins.
        /// 
        /// If the current page is higher than number of available pages, sets the page to the last available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        public void CheckPaging<T>(IQueryable<T> query)
        {
            NumPages = (int)Math.Ceiling((double)query.Count() / PageSize);
            if (NumPages == 0)
                NumPages++;
            if (PageNum > NumPages)
                PageNum = NumPages;
        }
    }
}
