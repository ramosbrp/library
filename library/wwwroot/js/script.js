document.addEventListener('DOMContentLoaded', () => {
    carregarLivros();
});


const carregarLivros = async () => {
    try {
        const response = await fetch('/api/livros');

        if (!response.ok) throw new Error("Error");

        const data = await response.json();
        console.log(data);

        const tbody = document.getElementById('tbody');
        tbody.innerHTML = '';

        if (data.Success) {

            var books = data.Data;

            books.forEach(book => {

                const row = tbody.insertRow();
                const cellNome = row.insertCell();
                const cellLancamento = row.insertCell();
                const cellAcoes = row.insertCell();

                cellNome.textContent = book.Titulo;
                cellLancamento.textContent = book.Lancamento;

                //Botão Editar
                const btnEditar = document.createElement('button');
                btnEditar.textContent = 'Editar';
                btnEditar.className = 'btnEditar';
                btnEditar.addEventListener('click', () => {
                    editarLivro(book.Codigo)
                });
                cellAcoes.appendChild(btnEditar);

                //Botão Deletar
                const btnDeletar = document.createElement('button');
                btnDeletar.textContent = 'Deletar';
                btnDeletar.className = 'btnDeletar';
                btnDeletar.addEventListener('click', () => {
                    deletarLivro(book.Codigo);
                });
                cellAcoes.appendChild(btnDeletar);



            })

        }

    } catch (error) {
        console.error('Error:', error);
    }
}

const editarLivro = (codigo) => {
    console.log('Editar livro com código: ', codigo);
}

const deletarLivro = (codigo) => {
    console.log('Deletando livro com código: ', codigo);
}