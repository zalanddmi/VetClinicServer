﻿using Microsoft.EntityFrameworkCore;

namespace VetClinicServer.Utils
{
    public class PaginatedList<T> : List<T>
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public PaginatedList(List<T> items, int pageNumber, int totalPages)
        {
            PageNumber = pageNumber;
            TotalPages = totalPages;

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
