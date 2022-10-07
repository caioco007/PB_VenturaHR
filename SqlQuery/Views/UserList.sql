USE [VenturaHR]
GO

/****** Object:  View [dbo].[UserList]    Script Date: 06/10/2022 22:22:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[UserList]
AS
	SELECT 
		  AU.Id									as 'UserId'
		, ISNULL(P.FullName, p.CompanyName)		as 'Nome'
		, AU.Email								as 'Email'
		, P.Cnpj								as 'Cnpj'
		, P.Cpf									as 'Cpf'
		, P.PhoneNumber							as 'PhoneNumber'
		, P.MobileNumber						as 'MobileNumber'
		, P.CreatedDate							as 'CreatedDate'
		, P.IsDeleted							as 'IsDeleted'
		, P.IsActive							as 'IsActive'
	FROM
		AspNetUsers (NOLOCK) AU
		INNER JOIN Person(NOLOCK) P ON AU.PersonId = P.PersonId
GO


