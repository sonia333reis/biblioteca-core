// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function validateBook() {
    var mensagem = "";
    var countErrors = 0;

    var form = document.getElementById("formCad");
    var name = form.name.value;
    var writter = form.writter.value;
    var release = form.release.value;

    if (name.indexOf(" ") == -1) {
        mensagem = mensagem + "- Informar nome válido. \n";
        countErrors++;
    }

    if (name == "") {
        mensagem = mensagem + "- Informar um nome. \n";
        countErrors++;
    }

    if (writter.indexOf(" ") == -1) {
        mensagem = mensagem + "- Informar um autor válido. \n";
        countErrors++;
    }

    if (writter == "") {
        mensagem = mensagem + "-Informar um autor. \n";
        countErrors++;
    }

    if (writter.match(/\d+/g) != null) {
        mensagem = mensagem + "-Não é permitido inserir números no nome do autor. \n";
        countErrors++;
    }

    if (release == "") {
        mensagem = mensagem + "- Informar a data de lançamento completa. \n";
        countErrors++;
    }

    if (countErrors > 0) {
        alert(mensagem);
        form.name.focus();
        return false;
    }
}