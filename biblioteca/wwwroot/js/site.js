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

function validateUser() {
    var mensagem = "";
    var countErrors = 0;

    var form = document.getElementById("formCad");
    var name = form.name.value;
    var email = form.email.value;
    var cpf = form.cpf.value;

    if (name == "") {
        mensagem = mensagem + "- Informar login. \n";
        countErrors++;
    }

    if (!(name.indexOf(" ") == -1)) {
        mensagem = mensagem + "- Informar login válido (sem espaços). \n";
        countErrors++;
    }

    if (email == "") {
        mensagem = mensagem + "- Informar um e-mail. \n";
        countErrors++;
    }

    if (!validateEmail(email)) {
        mensagem = mensagem + "- Informar um e-mail válido. \n";
        countErrors++;
    }

    if (cpf == "") {
        mensagem = mensagem + "- Informar cpf. \n";
        countErrors++;
    }

    if (countErrors > 0) {
        alert(mensagem);
        form.name.focus();
        return false;
    } else {
        form.cpf.value = outFormat(cpf);
    }
}

function validateBooking() {
    var mensagem = "";
    var countErrors = 0;

    var form = document.getElementById("formCad");
    var book = form.book.value;
    var user = form.user.value;

    if (book == "" || user == "") {
        mensagem = mensagem + "- Uma reserva precisa ter um usuário e um livro selecionado! \n";
        countErrors++;
    }

    if (countErrors > 0) {
        alert(mensagem);
        form.name.focus();
        return false;
    }
}

function validateEmail(email) {

    user = email.substring(0, email.indexOf("@"));
    domain = email.substring(email.indexOf("@") + 1, email.length);

    if ((user.length >= 1) && (domain.length >= 3) &&
        (user.search("@") == -1) && (domain.search("@") == -1) &&
        (user.search(" ") == -1) && (domain.search(" ") == -1) &&
        (domain.search(".") != -1) && (domain.indexOf(".") >= 1) &&
        (domain.lastIndexOf(".") < domain.length - 1)) {

        return true;
    }
    else {
        return false;
    }

}

function cpfFormat(field) {
    field.value = field.value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3\-\$4");
}

function outFormat(field) {
    field.value = field.value.replace(/(\.|\/|\-)/g, "");
}