USE [VenturaHR]
GO

/****** Object:  View [dbo].[OpportunityList]    Script Date: 06/10/2022 22:21:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[OpportunityList]
AS
	SELECT 
		  O.OpportunityId						as 'OpportunityId'
		, O.Office								as 'Office'
		, O.Description							as 'Description'
		, O.EmploymentId						as 'EmploymentId'
		, O.EmploymentId						as 'EmploymentName'
		, O.Experience							as 'Experience'
		, (O.City+' - '+O.State)				as 'Local'
		, O.CompanyId							as 'CompanyId'
		, O.ExpirationDate						as 'ExpirationDate'
		, O.CreatedDate							as 'CreatedDate'
		, O.Salary								as 'Salary'
		, O.StatusId							as 'StatusId'
		,CASE
			WHEN O.StatusId = 1 THEN 'Publicada'
			WHEN O.StatusId = 2 THEN 'Expirada'
			WHEN O.StatusId = 3 THEN 'Finalizada'		
		END										as 'StatusName'
		, O.IsDeleted							as 'IsDeleted'
		, P.CompanyName							as 'CompanyName'
	FROM
		Opportunity (NOLOCK) O
		INNER JOIN Person(NOLOCK) P ON O.CompanyId = P.PersonId
GO


