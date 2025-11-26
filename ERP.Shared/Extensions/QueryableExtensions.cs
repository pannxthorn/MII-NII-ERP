using ERP.Shared._base;
using Microsoft.EntityFrameworkCore;

namespace ERP.Shared.Extensions
{
    /// <summary>
    /// Extension methods for IQueryable to support pagination
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Apply pagination to IQueryable and return PaginatedResponse
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TDto">DTO type</typeparam>
        /// <param name="query">Source query</param>
        /// <param name="request">Pagination request</param>
        /// <param name="mapper">Function to map entity to DTO</param>
        /// <returns>Paginated response with mapped DTOs</returns>
        public static async Task<PaginatedResponse<TDto>> ToPaginatedResponseAsync<T, TDto>(
            this IQueryable<T> query,
            PaginationRequest request,
            Func<T, TDto> mapper)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(request.Skip)
                .Take(request.PageSize)
                .ToListAsync();

            var mappedItems = items.Select(mapper).ToList();

            return new PaginatedResponse<TDto>(
                mappedItems,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }

        /// <summary>
        /// Apply pagination to IQueryable and return PaginatedResponse (async mapper version)
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TDto">DTO type</typeparam>
        /// <param name="query">Source query</param>
        /// <param name="request">Pagination request</param>
        /// <param name="mapperAsync">Async function to map entity to DTO</param>
        /// <returns>Paginated response with mapped DTOs</returns>
        public static async Task<PaginatedResponse<TDto>> ToPaginatedResponseAsync<T, TDto>(
            this IQueryable<T> query,
            PaginationRequest request,
            Func<T, Task<TDto>> mapperAsync)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(request.Skip)
                .Take(request.PageSize)
                .ToListAsync();

            var mappedItems = new List<TDto>();
            foreach (var item in items)
            {
                var mapped = await mapperAsync(item);
                mappedItems.Add(mapped);
            }

            return new PaginatedResponse<TDto>(
                mappedItems,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }

        /// <summary>
        /// Apply pagination to already-loaded list
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="source">Source list</param>
        /// <param name="request">Pagination request</param>
        /// <returns>Paginated response</returns>
        public static PaginatedResponse<T> ToPaginatedResponse<T>(
            this IEnumerable<T> source,
            PaginationRequest request)
        {
            var sourceList = source.ToList();
            var totalCount = sourceList.Count;

            var items = sourceList
                .Skip(request.Skip)
                .Take(request.PageSize)
                .ToList();

            return new PaginatedResponse<T>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}
