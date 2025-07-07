-- =========================================================
-- Author:		Dave Brown
-- Create date: 07 JUL 2025
-- Description:	Get Suppliers
-- =========================================================
CREATE PROCEDURE [dbo].[Supplier_LoadAll]
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Err int

	SELECT
		[Id],
		[SupplierName],
		[Deleted],
		[AmendUserID],
		[AmendDate]
	FROM [Supplier]
	WHERE
		[Deleted] <> 1
	ORDER BY
		[SupplierName] ASC

	SET @Err = @@Error

	RETURN @Err
END
