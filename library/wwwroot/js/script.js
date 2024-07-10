document.addEventListener('DOMContentLoaded', () => {
    buscarLivros();
});

const carregarLivros = (books) => {

    const tbody = document.getElementById('tbody');
    tbody.innerHTML = '';
    console.log(books);
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



    });
}

//Listar
const buscarLivros = async () => {
    try {
        const response = await fetch('/api/livros');

        if (!response.ok) throw new Error("Error");

        const data = await response.json();



        if (data.Success) {

            var books = data.Data;
            carregarLivros(books);


        }

    } catch (error) {
        console.error('Error:', error);
    }
}

//Adcionar
document.getElementById('addBookForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const livroData = {
        titulo: document.getElementById('titulo').value,
        autor: document.getElementById('autor').value,
        lancamento: document.getElementById('lancamento').value,
        isDigital: document.getElementById('isDigital').checked,
        formato: document.getElementById('isDigital').checked ? document.getElementById('formato').value : null,
        isImpressed: document.getElementById('isImpressed').checked,
        peso: document.getElementById('isImpressed').checked ? parseFloat(document.getElementById('peso').value) : null,
        tipoEncadernacaoID: document.getElementById('tipoEncadernacao').value
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

            buscarLivros();
        }


    } catch (error) {
        console.error('Erro:', error);
    }
});

//Editar
const editarLivro = async (codigo) => {

    try {
        const response = await fetch(`/api/livros/detail?codigo=${codigo}`, {
            method: 'GET'
        });

        if (!response.ok) throw new Error("Error");

        const data = await response.json();

        if (data.Success) {
            const book = data.Data;

            const bookTitleInput = document.getElementById('bookTitle');
            const bookAuthorInput = document.getElementById('bookAuthor');
            const bookReleaseInput = document.getElementById('bookRelease');
            var bookTagsInput = document.getElementById('bookTags');

            bookTitleInput.value = book.Titulo;
            bookAuthorInput.value = book.Autor;
            bookReleaseInput.value = book.Lancamento;
            bookTagsInput.value = '';

            book.Tags.forEach((tag, index) => {
                bookTagsInput.value += tag.Descricao + (index < book.Tags.length - 1 ? ', ' : '');  // Adiciona vírgula apenas entre as tags
            });

            document.getElementById('modal-edit-form').addEventListener('submit', async (e) => {
                e.preventDefault();

                const tagsInput = bookTagsInput.value;
                const tagsArray = tagsInput.split(',').map(tag => ({ Descricao: tag.trim() }));


                const livroData = {
                    codigo: codigo,
                    titulo: bookTitleInput.value,
                    autor: bookAuthorInput.value,
                    lancamento: bookReleaseInput.value,
                    tags: tagsArray
                };

                const response = await fetch(`/api/livros/edit`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(livroData)
                });
                if (!response.ok) throw new Error("Error");

                const result = response.json();

                if (resul.Success) { }


            });
        }
    } catch (ex) {
        throw ex;
    }
    const modal = document.getElementById('bookDetailModal');
    modal.style.display = 'block';
}


//Deletar
function deletarLivro(bookId) {
    const modal = document.getElementById('deleteModal');
    const span = document.getElementsByClassName("close")[0];
    const deleteConfirm = document.getElementById('deleteConfirm');

    // Quando o usuário clica no botão de deletar
    deleteConfirm.onclick = function () {
        modal.style.display = "none";
        deleteBook(bookId); // Chama a função que deleta o livro
    };

    // Quando o usuário clica em cancelar ou no (x)
    span.onclick = function () {
        modal.style.display = "none";
    };
    document.getElementById('deleteCancel').onclick = function () {
        modal.style.display = "none";
    };

    // Mostra a modal
    modal.style.display = "block";
}

// Função para deletar o livro
function deleteBook(bookId) {
    fetch(`/api/livros/${bookId}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Falha ao deletar o livro');
            }
            return response.json();
        })
        .then(data => {
            console.log(data.message);
            buscarLivros();
        })
        .catch(error => console.error('Erro:', error));
}


function formatarData(dataISO) {
    const data = new Date(dataISO);
    return data.toLocaleDateString('pt-BR'); // 'pt-BR' para formato brasileiro
}

document.getElementById('filterForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    const year = document.getElementById('filterYear').value;
    const month = document.getElementById('filterMonth').value;

    try {

        const queryParams = new URLSearchParams({
            year: year,
            month: month
        }).toString();


        const response = await fetch(`/api/livros/filter?${queryParams}`, {
            method: 'GET',
        });

        if (response.ok) {
            const result = await response.json();
            const books = result.Data;
            carregarLivros(books);
        } else
            throw new Error("Falha ao filtrar livros");



    } catch (error) {
        console.error('Erro:', error);
    }


});


document.querySelector('.close').onclick = function () {
    document.getElementById('bookDetailModal').style.display = 'none';
};

document.getElementById('isDigital').addEventListener('change', function () {
    document.getElementById('digitalFields').style.display = this.checked ? 'block' : 'none';
});

document.getElementById('isImpressed').addEventListener('change', function () {
    document.getElementById('impressedFields').style.display = this.checked ? 'block' : 'none';
});
