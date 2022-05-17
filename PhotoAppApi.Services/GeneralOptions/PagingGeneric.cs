using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.GeneralOptions
{
    public static class PagingGeneric
    {
        /// <summary>
        /// Pages query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageNum">The serial number of a block of entities starting from 1.</param>
        /// <param name="pageSize">The number of entities in divided blocks.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws exeption if pageNum is less than 1 or pageSize is equal or less than zero.</exception>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNum, int pageSize)
        {
            if (pageNum < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNum));
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            if (pageNum != 1)
                query = query.Skip((pageNum - 1) * pageSize);

            return query.Take(pageSize);
        }
    }
}
