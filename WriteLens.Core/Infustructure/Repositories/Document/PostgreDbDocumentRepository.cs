using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Infrastructure.Data;
using WriteLens.Core.Infrastructure.Data.PostgresDb;
using WriteLens.Core.Infrastructure.Data.PostgresDb.Entities;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Utilities;
using ZstdSharp.Unsafe;

namespace WriteLens.Core.Infrastructure.Repositories;

public class PostgresDbDocumentRepository : IDocumentRepository
{
    public readonly ApplicationDbContext _context;
    public readonly IMapper _mapper;

    public PostgresDbDocumentRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Document>> GetManyByUserIdAsync(Guid userId, DocumentQueryParams? queryParams)
    {
        IQueryable<DocumentEntity> documents = _context.Documents
            .Where(d => d.UserId == userId)
            .Include(d => d.User);
        
        if (queryParams != null)
        {
            if (!string.IsNullOrEmpty(queryParams.Search))
            {
                documents = documents.Where(d => EF.Functions.ILike(d.Title, $"%{queryParams.Search}%"));
            }

            if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
            {
                string sortBy = TextUtitlity.Capitalize(queryParams.SortBy);
                await ValidateDocumentHasField(sortBy);

                bool isDesc = queryParams.SortDirection == "desc";
                documents = isDesc
                    ? documents.OrderByDescending(d => EF.Property<object>(d, sortBy))
                    : documents.OrderBy(d => EF.Property<object>(d, sortBy));
            }
        }

        return (await documents.ToListAsync()).Select(d => _mapper.Map<Document>(d)).ToList();
    }

    private async Task ValidateDocumentHasField(string filedName)
    {
        string?[] documentFields = typeof(DocumentEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToArray();
        
        if (documentFields.Contains(filedName))
            return;

        throw new InvalidSortingFiledException(
            $@"Invalid Sorting Filed Exception:\
            Document object doesn't have field '{filedName}'"
        );
    }

    public async Task<Document?> GetSingleByIdAsync(Guid documentId)
    {
        DocumentEntity? document = await getSingleEntityByIdAsync(documentId);
        return _mapper.Map<Document>(document);
    }

    public async Task AddSingleAsync(Guid userId, Document document)
    {
        DocumentEntity dbDocument = _mapper.Map<DocumentEntity>(document);
        dbDocument.UserId = userId;
        await _context.AddAsync(dbDocument);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSingleByIdAsync(Guid documentId, UpdateDocumentCommand updateDocumentCommand)
    {
        DocumentEntity? document = await getSingleEntityByIdAsync(documentId);
        if (document is null) return;

        if (updateDocumentCommand.Title != null) document.Title = updateDocumentCommand.Title;
        document.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSingleByIdAsync(Guid documentId)
    {
        DocumentEntity? document = await getSingleEntityByIdAsync(documentId);

        if (document is null) return;

        _context.Remove(document);
        await _context.SaveChangesAsync();
    }

    private async Task<DocumentEntity?> getSingleEntityByIdAsync(Guid documentId)
    {
        DocumentEntity? document = await _context.Documents
            .Where(d => d.Id == documentId)
            .Include(d => d.User)
            .FirstOrDefaultAsync();
        
        return document;
    }
}