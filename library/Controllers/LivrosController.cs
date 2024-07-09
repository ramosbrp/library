using library.Data;
using library.Models;
using library.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly DatabaseAccess _databaseAccess;

    public LivrosController(DatabaseAccess databaseAccess)
    {
        _databaseAccess = databaseAccess;
    }

    [HttpGet]
    public async Task<IActionResult> GetLivros()
    {
        var sql = "SELECT * FROM Livro";
        var livros = await _databaseAccess.QueryAsync<LivroDto>(sql, reader =>
            new LivroDto
            {
                Codigo = reader.GetInt32(reader.GetOrdinal("Codigo")),
                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                Autor = reader.GetString(reader.GetOrdinal("Autor")),
                Lancamento = reader.GetDateTime(reader.GetOrdinal("Lancamento"))
            });

        return Ok(livros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLivro(int id)
    {
        var sql = $"SELECT * FROM Livro WHERE Codigo = @id";
        var livro = await _databaseAccess.QueryAsync<LivroDto>(sql, reader =>
            new LivroDto
            {
                Codigo = reader.GetInt32(reader.GetOrdinal("Codigo")),
                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                Autor = reader.GetString(reader.GetOrdinal("Autor")),
                Lancamento = reader.GetDateTime(reader.GetOrdinal("Lancamento"))
            });

        if (livro.Any())
            return Ok(livro.First());
        else
            return NotFound();
    }

}
