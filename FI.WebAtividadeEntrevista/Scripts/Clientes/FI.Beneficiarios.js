$(document).ready(function () {
    $('#btnBeneficiario').click(function (e) {
        e.preventDefault();

        $("#modalBeneficiarios").modal();
    });

    $('#btnIncluirBene').click(function (e) {
        adicionarBeneficiario();
    });
})

function adicionarBeneficiario() {
    let cpf = document.getElementById("CPFBene").value;
    let nome = document.getElementById("NomeBene").value;

    if (!validarCPF(cpf)) {
        alert('CPF invalido!');
        return;
    }

    if (verificaCPFExiste(cpf)) {
        alert('CPF em uso por outro beneficiario!');
        return;
    }

    if (nome.trim() === '') {
        alert('Nome vazio!');
        return;
    }

    adicionarBeneficiarioTabela(0, cpf, nome, 0);

    document.getElementById("CPFBene").value = '';
    document.getElementById("NomeBene").value = '';
}

function adicionarBeneficiarioTabela(id, cpf, nome, idCliente) {
    let tabela = document.getElementById("tabBene").getElementsByTagName('tbody')[0];
    let novaLinha = tabela.insertRow();

    let celulaId = novaLinha.insertCell(0);
    let celulaCpf = novaLinha.insertCell(1);
    let celulaNome = novaLinha.insertCell(2);
    let celulaIdCliente = novaLinha.insertCell(3);
    let celulaBtnExcluir = novaLinha.insertCell(4);

    celulaId.textContent = id;
    celulaCpf.textContent = cpf;
    celulaNome.textContent = nome;
    celulaIdCliente.textContent = idCliente;
    celulaBtnExcluir.innerHTML = `<button id="btnExcluirBene" class="btn btn-xs btn-primary" onClick="this.parentNode.parentNode.remove();">Excluir</button>`;
}

function limpaModalBeneficiarios() {
    document.getElementById("CPFBene").value = '';
    document.getElementById("NomeBene").value = '';
    let tabela = document.getElementById("tabBene");
    let linhas = tabela.getElementsByTagName("tbody")[0];
    linhas.innerHTML = "";
}

function listaDeBeneficiarios() {
    const tabela = document.getElementById('tabBene');
    let arrayValores = [];

    for (let i = 1; i < tabela.rows.length; i++) {
        for (let j = 0; j < tabela.rows[i].cells.length; j++) {
            let beneficiario = {
                Id: parseInt(tabela.rows[i].cells[0].innerText),
                CPF: tabela.rows[i].cells[1].innerText,
                Nome: tabela.rows[i].cells[2].innerText,
                IdCliente: parseInt(tabela.rows[i].cells[3].innerText)
            };
            arrayValores.push(beneficiario);
            break;
        }
    }

    return arrayValores;
}

function carregaTabelaBeneficiario(listaBeneficiario) {
    listaBeneficiario.forEach(beneficiario => {
        adicionarBeneficiarioTabela(beneficiario.Id, beneficiario.CPF, beneficiario.Nome, beneficiario.IdCliente);
    });
}

function verificaCPFExiste(valor) {
    const tabela = document.getElementById('tabBene');

    for (let i = 0; i < tabela.rows.length; i++) {
        for (let j = 0; j < tabela.rows[i].cells.length; j++) {
            if (tabela.rows[i].cells[1].innerText === valor) {
                return true;
            }
            break;
        }
    }

    return false;
}

function validarCPF(cpf) {
    const regex = /^\d{3}\.\d{3}\.\d{3}-\d{2}$/;

    if (!regex.test(cpf)) {
        return false;
    }

    let cpfNumero = cpf.replace(/[.-]/g, '')

    if (cpfNumero.length !== 11 || /^(\d)\1{10}$/.test(cpfNumero)) {
        return false;
    }

    let soma = 0;
    let resto;

    for (let i = 1; i <= 9; i++) {
        soma += parseInt(cpfNumero.substring(i - 1, i)) * (11 - i);
    }

    resto = (soma * 10) % 11;

    if (resto === 10 || resto === 11) {
        resto = 0;
    }

    if (resto !== parseInt(cpfNumero.substring(9, 10))) {
        return false;
    }

    soma = 0;

    for (let i = 1; i <= 10; i++) {
        soma += parseInt(cpfNumero.substring(i - 1, i)) * (12 - i);
    }

    resto = (soma * 10) % 11;

    if (resto === 10 || resto === 11) {
        resto = 0;
    }

    if (resto !== parseInt(cpfNumero.substring(10, 11))) {
        return false;
    }

    return true;
}
