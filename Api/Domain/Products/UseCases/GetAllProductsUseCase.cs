﻿using Api.Common;
using Api.Data;
using Api.Domain.Products.Data;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Products.UseCases;

public class GetAllProductsUseCase(AppDbContext db) : IPagedUseCase<PagedRequest, List<ProductDto>>
{
    public async Task<PagedResult<List<ProductDto>>> Execute(PagedRequest req, CancellationToken ct)
    {
        var skip = (req.Page - 1) * req.PageSize;

        var products = await db.Products
            .Skip(skip)
            .Take(req.PageSize)
            .Select(e => ProductMapper.ProductToDto(e))
            .ToListAsync(ct);

        var totalRecords = await db.Products.CountAsync(ct);
        var totalPages = (int)Math.Ceiling((double)totalRecords / req.PageSize);

        return Result<List<ProductDto>>.Success(products).ToPagedResult(new PagedInfo(
            req.Page,
            req.PageSize,
            totalPages, totalRecords));
    }
}