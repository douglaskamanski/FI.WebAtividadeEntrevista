using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Incluir(beneficiario);
        }

        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            bene.Alterar(beneficiario);
            return;
        }

        /// <summary>
        /// Lista beneficiários por cliente
        /// </summary>
        public List<DML.Beneficiario> ListarPorCliente(long idCliente)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.ListarPorCliente(idCliente);
        }

        /// <summary>
        /// Excluir o beneficiário pelo id
        /// </summary>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            cli.Excluir(id);
            return;
        }

        /// <summary>
        /// Excluir todos beneficiários por cliente
        /// </summary>
        public void ExcluirTodosPorCliente(long idCliente)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            bene.ExcluirTodosPorCliente(idCliente);
            return;
        }
    }
}
