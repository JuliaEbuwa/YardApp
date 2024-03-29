﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yard.infrastructure.Utility
{
    public class GenericPagination<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public GenericPagination(List<T> currentPageItems, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(currentPageItems);
        }
        public static GenericPagination<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var currentPageItems = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new GenericPagination<T>(currentPageItems, count, pageNumber, pageSize);
        }
        public static async  Task<GenericPagination<T>> ToPagedListAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var currentPageItems =await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new GenericPagination<T>(currentPageItems, count, pageNumber, pageSize);
        }

    }

}
