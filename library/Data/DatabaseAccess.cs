using Microsoft.Data.SqlClient;
using library.Models;
using library.Models.Dto;

namespace library.Data
{
    public class DatabaseAccess
    {
        private readonly string _connectionString;

        public DatabaseAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<LivroDto>> GetAllLivrosAsync()
        {
            try
            {
                var sql = "SELECT Codigo, Titulo, Lancamento FROM Livro";
                List<LivroDto> livros = new List<LivroDto>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var livro = new LivroDto
                                {
                                    Codigo = reader.GetInt32(reader.GetOrdinal("Codigo")),
                                    Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                    Lancamento = reader.GetDateTime(reader.GetOrdinal("Lancamento"))
                                };
                                livros.Add(livro);
                            }
                        }
                    }
                }
                return livros;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public async Task<int> AddLivroAsync(Livro livro)
        {
            try
            {
                var sql = "INSERT INTO Livro (Titulo, Autor, Lancamento) values (@Titulo, @Autor, @Lancamento); SELECT CAST(SCOPE_IDENTITY() AS int);";
                var connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Titulo", livro.Titulo);
                    command.Parameters.AddWithValue("@Autor", livro.Autor);
                    command.Parameters.AddWithValue("@Lancamento", livro.Lancamento);

                    connection.Open();
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
