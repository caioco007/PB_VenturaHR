USE [VenturaHR]
GO

/****** Object:  StoredProcedure [dbo].[pr_GetResponse]    Script Date: 06/10/2022 22:23:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[pr_GetResponse]
	@OpportunityId INT
AS

	DECLARE @Insurances TABLE(CandidateId INT, FullName NVARCHAR(MAX), MobileNumber NVARCHAR(MAX), Email NVARCHAR(250), NotesByOpportunity DECIMAL)
	
	
	INSERT INTO @Insurances
		SELECT
			R.CandidateId,
			P.FullName,
			P.MobileNumber,
			P.Email,
			R.NotesByOpportunity
		FROM
			Opportunity(NOLOCK) O
			INNER JOIN OpportunityCriterion(NOLOCK) OC ON O.OpportunityId = OC.OpportunityId
			INNER JOIN Response(NOLOCK) R ON O.OpportunityId = R.OpportunityId
			INNER JOIN ResponseCriterion(NOLOCK) RC ON R.CandidateId = RC.CandidateId
			INNER JOIN Person(NOLOCK) P ON R.CandidateId = P.PersonId
		WHERE 
			O.OpportunityId = @OpportunityId


	SELECT * FROM @Insurances ORDER BY NotesByOpportunity DESC
GO


