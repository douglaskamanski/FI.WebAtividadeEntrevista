CREATE PROC FI_SP_ExcluirTodosBenePorCliente
	@IDCLIENTE BIGINT
AS
BEGIN
	DELETE BENEFICIARIOS WHERE IDCLIENTE = @IDCLIENTE
END