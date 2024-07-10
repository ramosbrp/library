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

        public async Task<List<LivroDto>> FilterAsync(int? year, int? month)
        {
            return await _databaseAccess.FilterAsync(year, month);
        }

        public async Task<LivroDto> Detail(int codigo)
        {
            return await _databaseAccess.Detail(codigo);
        }

        public async Task<LivroDto> UpdateAsync(LivroDto livroDto)
        {
            var livro = new Livro
            {
                Titulo = livroDto.Titulo,
                Autor = livroDto.Autor,
                Lancamento = livroDto.Lancamento
            };

            List<Tag> tags = new List<Tag>();
            tags = livroDto.Tags;

            //foreach (var item in livroDto.Tags)
            //{
            //    tags.Add(item);
            //}


            var id = await _databaseAccess.UpdateAsync(livroDto);

            livro.Codigo = id;

            return new LivroDto {
                Codigo = id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                Lancamento= livro.Lancamento,
                Tags = tags
            };
        }
    }
}
