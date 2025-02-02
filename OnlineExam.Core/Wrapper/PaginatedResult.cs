using Microsoft.EntityFrameworkCore;

namespace OnlineExam.Core.Wrapper
{
    public class PaginatedResult<T>
    {
        public List<T>? Items { get; private set; }
        public int PageSize   { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages => (int)Math.Ceiling((double)(TotalCount / PageSize));


        public async static Task<PaginatedResult<T>?> CreateAsync(IQueryable<T> Source, int PageNumber, int PageSize = 10)
        {
            var TotalCount = Source.Count();
            List<T>? paginatedList = await Source.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

            if (paginatedList.Any())
            {
                return new PaginatedResult<T>
                {
                    Items      = paginatedList,
                    PageNumber = PageNumber,
                    PageSize   = PageSize,
                    TotalCount = TotalCount
                };
            }
            else
                return null;
        }
    }
}
