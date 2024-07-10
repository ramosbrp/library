using Microsoft.Data.SqlClient;
using library.Models;
using library.Models.Dto;
using System.Text;
using System.Data;

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

                var connection = new SqlConnection(_connectionString);
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
                return livros;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public async Task<int> AddLivroAsync(LivroDto livroDto)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand("AddLivro", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Titulo", livroDto.Titulo);
                    command.Parameters.AddWithValue("@Autor", livroDto.Autor);
                    command.Parameters.AddWithValue("@Lancamento", livroDto.Lancamento);
                    command.Parameters.AddWithValue("@IsDigital", livroDto.IsDigital);
                    command.Parameters.AddWithValue("@Formato", livroDto.IsDigital ? livroDto.Formato : DBNull.Value);
                    command.Parameters.AddWithValue("@IsImpressed", livroDto.IsImpressed);
                    command.Parameters.AddWithValue("@Peso", livroDto.IsImpressed ? livroDto.Peso : DBNull.Value);
                    command.Parameters.AddWithValue("@TipoEncadernacaoID", livroDto.IsImpressed && livroDto.TipoEncadernacaoID.HasValue ? livroDto.TipoEncadernacaoID.Value : DBNull.Value);

                    var tagsAsString = string.Join(",", livroDto.Tags.Select(t => t.Descricao));
                    command.Parameters.AddWithValue("@Tags", tagsAsString);

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

        public async Task<List<LivroDto>> FilterAsync(int? year, int? month)
        {
            try
            {
                var sql = new StringBuilder("SELECT Codigo, Titulo, Lancamento FROM Livro");

                var parameters = new List<SqlParameter>();

                if (year.HasValue || month.HasValue)
                {
                    sql.Append(" WHERE ");
                    bool addAnd = false;
                    if (year.HasValue)
                    {
                        sql.Append("YEAR(Lancamento) = @Year");
                        parameters.Add(new SqlParameter("@Year", year));
                        addAnd = true;
                    }

                    if (month.HasValue)
                    {
                        if (addAnd) sql.Append(" AND ");
                        sql.Append("MONTH(Lancamento) = @Month");
                        parameters.Add(new SqlParameter("@Month", month));
                    }
                }

                var connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand(sql.ToString(), connection))
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    List<LivroDto> livros = new List<LivroDto>();
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

                    return livros;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<LivroDto> Detail(int codigo)
        {
            try
            {
                var sql = "SELECT * FROM vw_LivroDetalhes WHERE Codigo = @LivroId";
                var livro = new LivroDto();

                var connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LivroId", codigo);
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            livro.Codigo = reader.GetInt32(reader.GetOrdinal("Codigo"));
                            livro.Titulo = reader.GetString(reader.GetOrdinal("Titulo"));
                            livro.Autor = reader.GetString(reader.GetOrdinal("Autor"));
                            livro.Lancamento = reader.GetDateTime(reader.GetOrdinal("Lancamento"));
                            livro.Tags = !reader.IsDBNull(reader.GetOrdinal("Tags"))
                               ? reader.GetString(reader.GetOrdinal("Tags")).Split(',').Select(t => new Tag { Descricao = t.Trim() }).ToList()
                               : new List<Tag>();
                        }
                    }
                }

                return livro;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> UpdateAsync(LivroDto livroDto)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                using (var command = new SqlCommand("UpdateLivro", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LivroId", livroDto.Codigo);
                    command.Parameters.AddWithValue("@Titulo", livroDto.Titulo);
                    command.Parameters.AddWithValue("@Autor", livroDto.Autor);
                    // Formatação da data como string no formato ISO 8601
                    command.Parameters.AddWithValue("@Lancamento", livroDto.Lancamento.ToString("yyyy-MM-ddTHH:mm:ss"));

                    var tagsAsString = string.Join(",", livroDto.Tags.Select(t => t.Descricao));
                    command.Parameters.AddWithValue("@Tags", tagsAsString);

                    connection.Open();
                    var result = await command.ExecuteNonQueryAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Idealmente, você deve tratar essa exceção de forma mais apropriada ou logar o erro.
            }
        }

        public async Task<bool> DeleteLivroAsync(int livroId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("DeleteLivro", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LivroId", livroId);

                    await connection.OpenAsync();

                    try
                    {
                        var result = await command.ExecuteScalarAsync();
                        if (result != null && result.ToString().Contains("sucesso"))
                        {
                            return true;
                        }
                        else
                        {
                            return false; // Livro não encontrado ou erro na deleção
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception here using your preferred logging framework
                        throw new Exception("Error deleting the book: " + ex.Message);
                    }
                }
            }
        }


    }
}
