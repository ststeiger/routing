

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__Steuern_2017_ZO_Road]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__Steuern_2017_ZO_Road](
	[RD_UID] [uniqueidentifier] NOT NULL,
	[RD_GemeindeNummer] [int] NULL,
	[RD_Ort] [nvarchar](255) NULL,
	[RD_Entfernung] [float] NULL,
	[RD_Dauer] [float] NULL
) ON [PRIMARY]
END
GO





SELECT 
	 kanton 
	,gemeindenummer 
	,gemeinde 
	,latitude 
	,longitude 
	,from_50000/100.0e*6500*13 AS k50 
	,from_80000/100.0e*6500*13 AS k80 
	,from_100000/100.0e*6500*13 AS k100 
	,from_125000/100.0e*6500*13 AS k125 
	,from_150000/100.0e*6500*13 AS k150
	,from_175000/100.0e*6500*13 AS k175 

	,__Steuern_2017_ZO_Road.RD_Ort 
	,__Steuern_2017_ZO_Road.RD_Entfernung 
	,__Steuern_2017_ZO_Road.RD_Dauer 
FROM __Steuern_2017 

LEFT JOIN __Steuern_2017_ZO_Road -- 3033 vs. 3022
	ON __Steuern_2017_ZO_Road.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND RD_Ort = 'Erlen' 
	-- AND RD_Ort = 'Zürich HB'
	-- AND RD_Ort = 'St. Gallen HB'
	-- AND RD_Ort = 'Basel SBB'
	-- AND RD_Ort = 'Winterthur'

WHERE __Steuern_2017_ZO_Road.RD_UID IS NULL 

-- WHERE longitude IS NULL 
-- WHERE gemeinde LIKE '%Altstätten%'
-- WHERE CAST(latitude AS varchar(30)) LIKE '47.1166%'  -- [47.11667, 7.116667] -- Plateau de Diesse
-- WHERE CAST(latitude AS varchar(30)) LIKE '46.831%'  -- [46.83184, 7.505493].  -- Seftigen
-- WHERE CAST(latitude AS varchar(30)) LIKE '47.2416%' -- [47.24167, 8.958333] -- Berg (SG)
-- WHERE CAST(longitude AS varchar(30)) LIKE '8.6166%' -- 46.16667, 8.616667 -- Centovalli  
-- WHERE gemeindenummer = 5050 -- 46.41667, 8.983333 5050  -- Serravalle

--  Exception of type 'Itinero.Exceptions.RouteNotFoundException' was thrown.
-- WHERE gemeindenummer = 6205 -- Bettmeralp -- 46.390499	8.061880





-- SELECT * FROM __Steuern_2017_ZO_Road WHERE RD_Ort = 'Erlen' 
-- TRUNCATE TABLE __Steuern_2017_ZO_Road 














/*
-- https://docs.microsoft.com/en-us/sql/t-sql/queries/at-time-zone-transact-sql?view=sql-server-2017
SELECT 
	 DATEDIFF(second, '19700101', GETUTCDATE()) AS Unix_Time 
	,DATEDIFF(second, '20181121', CURRENT_TIMESTAMP)/(60.0*60.0) AS NOT_UnixTime  
*/
