namespace IntexBrickwell.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IntexBrickwell.Models;
using Microsoft.AspNetCore.Mvc;
public class PaginatedList<T> : List<T>, IActionResult
{
    private IActionResult _actionResultImplementation;
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; } // Add this line

    
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        
        this.AddRange(items);
    }
    
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
    
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    public Task ExecuteResultAsync(ActionContext context)
    {
        return _actionResultImplementation.ExecuteResultAsync(context);
    }

    public static async Task<object> CreateAsync(IQueryable<Product> asNoTracking, int pageNumber)
    {
        throw new NotImplementedException();
    }
}