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
        var sql = "SELECT Codigo, Titulo, Lancamento FROM Livro"; // Ajuste conforme a estrutura de LivroDto
        List<LivroDto> livros = await _databaseAccess.QueryAsync<LivroDto>(sql, reader =>
            new LivroDto
            {
                Codigo = reader.GetInt32(reader.GetOrdinal("Codigo")),
                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                Lancamento = reader.GetDateTime(reader.GetOrdinal("Lancamento"))
            });

        return livros;
    }
}
}
