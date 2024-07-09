document.addEventListener('DOMContentLoaded', () => {
    carregarLivros();
});


//Listar
const carregarLivros = async () => {
    try {
        const response = await fetch('/api/livros');

        if (!response.ok) throw new Error("Error");

        const data = await response.json();

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
                cellLancamento.textContent = formatarData(book.Lancamento);

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

//Adcionar
document.getElementById('addBookForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const livroData = {
        titulo: document.getElementById('titulo').value,
        autor: document.getElementById('autor').value,
        lancamento: document.getElementById('lancamento').value
    };

    try {
        const response = await fetch('/api/livros', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(livroData)
        });
        if (!response.ok) throw new Error("Error");

        const data = await response.json();

        if (data.Success) {
            const book = data.Data;
            console.log(book);

            carregarLivros();
        }


    } catch (error) {
        console.error('Erro:', error);
    }
});

//Editar
const editarLivro = (codigo) => {
    console.log('Editar livro com código: ', codigo);
}


//Deletar
const deletarLivro = (codigo) => {
    console.log('Deletando livro com código: ', codigo);
}

function formatarData(dataISO) {
    const data = new Date(dataISO);
    return data.toLocaleDateString('pt-BR'); // 'pt-BR' para formato brasileiro
}

document.getElementById('filterForm').addEventListener('submit', (e) => {
    e.preventDefault();

    const filtro = {
        ano: document.getElementById('filterYear').value,
        mes: document.getElementById('filterMonth').value
    }
    console.log(filtro);
});