using library.Data;
using library.Models;
using library.Models.Dto;
using library.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace library.Services
{


    public class LivroService : ILivroService
    {
        private readonly DatabaseAccess _databaseAccess;

        public LivroService(DatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }



        public async Task<List<LivroDto>> GetAllAsync()
        {
            return await _databaseAccess.GetAllLivrosAsync();
        }

        public async Task<LivroDto> AddAsync(LivroDto livroDto)
        {
            var livro = new Livro
            {
                Titulo = livroDto.Titulo,
                Autor = livroDto.Autor,
                Lancamento = livroDto.Lancamento
            };


            var id = await _databaseAccess.AddLivroAsync(livro);
            livro.Codigo = id;

            return new LivroDto
            {
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                Lancamento = livro.Lancamento
            };
        }
    }
}
