using FI.WebAtividadeEntrevista.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace FI.WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF no formato válido")]
        [CPFValidator(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// IdCliente
        /// </summary>
        public long IdCliente { get; set; }
    }
}
