using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutify.Models;

namespace Mutify.Controllers
{
    [ApiController]
    public abstract class BaseController<T, dtoT> : ControllerBase where T : PrimaryKeyModel where dtoT : class

    {
        protected readonly IMapper _mapper;
        protected readonly MutifyContext _context;
        protected abstract DbSet<T> _dbSet { get; }
        protected abstract Expression<Func<T, dtoT>> _asDto { get; }

        public BaseController(MutifyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        protected string _getDomain()
        {
            return $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        }

        protected async Task<List<dtoT>> _GetAll()
        {
            return await _dbSet.Select(_asDto).ToListAsync();
        }
        protected async Task<dtoT> _GetOneById(int id)
        {
            return await _dbSet.Where(entity => entity.Id == id).Select(_asDto).FirstOrDefaultAsync();
        }
    }
}