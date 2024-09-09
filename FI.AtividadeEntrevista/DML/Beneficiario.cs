﻿namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de Beneficiario que representa o registo na tabela Beneficiarios do Banco de Dados
    /// </summary>
    public class Beneficiario
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long IdCliente { get; set; }
    }
}
