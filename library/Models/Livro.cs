﻿namespace library.Models
{
    public class Livro
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public DateTime Lancamento { get; set; }

        public Livro()
        {
            Codigo = 0;
            Titulo = string.Empty;
            Autor = string.Empty;
            Lancamento = DateTime.MinValue;
        }
    }
}
