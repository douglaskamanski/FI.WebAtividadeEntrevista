using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join("<br>", erros));
            }

            if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                Response.StatusCode = 400;
                return Json("Esse CPF já em uso por outro cliente");
            }

            if (model.Beneficiarios != null)
            {
                foreach (var beneficiario in model.Beneficiarios)
                {
                    if (model.Beneficiarios.Any(b => b.Id != beneficiario.Id && b.CPF == beneficiario.CPF))
                    {
                        Response.StatusCode = 400;
                        return Json("Dois CPF em uso para um mesmo beneficiário");
                    }
                }
            }

            model.Id = bo.Incluir(new Cliente()
            {
                CPF = model.CPF,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone
            });

            if (model.Beneficiarios != null)
            {
                foreach (var beneficiario in model.Beneficiarios)
                {
                    var IdBeneficiario = boBeneficiario.Incluir(new Beneficiario()
                    {
                        CPF = beneficiario.CPF,
                        Nome = beneficiario.Nome,
                        IdCliente = model.Id
                    });
                }
            }

            return Json("Cadastro efetuado com sucesso");
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join("<br>", erros));
            }

            if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                Response.StatusCode = 400;
                return Json("Esse CPF já em uso por outro cliente");
            }

            if (model.Beneficiarios != null)
            {
                foreach (var beneficiario in model.Beneficiarios)
                {
                    if (model.Beneficiarios.Any(b => b.Id != beneficiario.Id && b.CPF == beneficiario.CPF))
                    {
                        Response.StatusCode = 400;
                        return Json("Dois CPF em uso para um mesmo beneficiário");
                    }
                }
            }

            bo.Alterar(new Cliente()
            {
                Id = model.Id,
                CPF = model.CPF,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone
            });

            IEnumerable<Beneficiario> listaBeneficiarios = boBeneficiario.ListarPorCliente(model.Id);

            //se a lista do front veio vazia e tiver lista no back -> apaga todos
            //se tiver na lista do back e nao tiver do front -> apaga um
            if (listaBeneficiarios.Count() > 0)
            {
                if (model.Beneficiarios == null)
                {
                    boBeneficiario.ExcluirTodosPorCliente(model.Id);
                }
                else
                {
                    foreach (var beneficiario in listaBeneficiarios)
                    {
                        if (!model.Beneficiarios.Any(b => b.Id == beneficiario.Id))
                        {
                            boBeneficiario.Excluir(beneficiario.Id);
                        }
                    }
                }
            }

            if (model.Beneficiarios != null)
            {
                foreach (var beneficiario in model.Beneficiarios)
                {
                    if (beneficiario.Id == 0)
                    {
                        var IdBeneficiario = boBeneficiario.Incluir(new Beneficiario()
                        {
                            CPF = beneficiario.CPF,
                            Nome = beneficiario.Nome,
                            IdCliente = model.Id
                        });
                    }
                    else
                    {
                        boBeneficiario.Alterar(new Beneficiario()
                        {
                            Id = beneficiario.Id,
                            CPF = beneficiario.CPF,
                            Nome = beneficiario.Nome,
                            IdCliente = beneficiario.IdCliente
                        });
                    }
                }
            }

            return Json("Cadastro alterado com sucesso");
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                BoBeneficiario boBeneficiario = new BoBeneficiario();
                IEnumerable<Beneficiario> listaBeneficiarios = boBeneficiario.ListarPorCliente(id);

                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CPF = cliente.CPF,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Beneficiarios = listaBeneficiarios
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}