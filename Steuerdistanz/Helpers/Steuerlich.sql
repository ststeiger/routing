

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__Steuern_2017_ZO_Road]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__Steuern_2017_ZO_Road]
(
	[RD_UID] [uniqueidentifier] NOT NULL,
	[RD_GemeindeNummer] [int] NULL,
	[RD_Ort] [nvarchar](255) NULL,
	[RD_Entfernung] [float] NULL,
	[RD_Dauer] [float] NULL
);
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__Steuern_2017_ZO_Rail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__Steuern_2017_ZO_Rail]
(
	[RL_UID] [uniqueidentifier] NOT NULL,
	[RL_GemeindeNummer] [int] NULL,
	[RL_Ort] [nvarchar](255) NULL,
	[RL_Dauer] [float] NULL
);
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

	,AutoErlen.RD_Ort 
	,AutoErlen.RD_Entfernung 
	,AutoErlen.RD_Dauer 

	--,AutoZurich.RD_Ort 
	--,AutoZurich.RD_Entfernung 
	--,AutoZurich.RD_Dauer 

	
	--,AutoSanktGallen.RD_Ort 
	--,AutoSanktGallen.RD_Entfernung 
	--,AutoSanktGallen.RD_Dauer 

		
	--,AutoBasel.RD_Ort 
	--,AutoBasel.RD_Entfernung 
	--,AutoBasel.RD_Dauer 

	--,AutoWinterthur.RD_Ort 
	--,AutoWinterthur.RD_Entfernung 
	--,AutoWinterthur.RD_Dauer 

	,ZugErlen.RL_Ort 
	,ZugErlen.RL_Dauer 

	--,ZugZurich.RL_Ort 
	--,ZugZurich.RL_Dauer 

	--,ZugSanktGallen.RL_Ort 
	--,ZugSanktGallen.RL_Dauer 

	--,ZugBasel.RL_Ort 
	--,ZugBasel.RL_Dauer 

	--,ZugSanktGallen.RL_Ort 
	--,ZugSanktGallen.RL_Dauer 
	
	--,ZugWinterthur.RL_Ort 
	--,ZugWinterthur.RL_Dauer 


	,AutoErlen.RD_Dauer 
	+ AutoZurich.RD_Dauer 
	+ AutoSanktGallen.RD_Dauer 
	+ AutoBasel.RD_Dauer 
	+ AutoSanktGallen.RD_Dauer 
	+ AutoWinterthur.RD_Dauer 
	AS AutoDauer 

	,ZugErlen.RL_Dauer 
	+ ZugZurich.RL_Dauer 
	+ ZugSanktGallen.RL_Dauer 
	+ ZugBasel.RL_Dauer 
	+ ZugSanktGallen.RL_Dauer 
	+ ZugWinterthur.RL_Dauer 
	AS ZugDauer 
FROM __Steuern_2017 

LEFT JOIN __Steuern_2017_ZO_Road AS AutoErlen -- 3033 vs. 3022
	ON AutoErlen.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoErlen.RD_Ort = 'Erlen' 


LEFT JOIN __Steuern_2017_ZO_Road AS AutoZurich -- 3033 vs. 3022
	ON AutoZurich.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoZurich.RD_Ort = 'Zürich HB'

LEFT JOIN __Steuern_2017_ZO_Road AS AutoSanktGallen-- 3033 vs. 3022
	ON AutoSanktGallen.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoSanktGallen.RD_Ort = 'St. Gallen HB'

LEFT JOIN __Steuern_2017_ZO_Road AS AutoBasel -- 3033 vs. 3022
	ON AutoBasel.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoBasel.RD_Ort = 'Basel SBB'

LEFT JOIN __Steuern_2017_ZO_Road AS AutoWinterthur -- 3033 vs. 3022
	ON AutoWinterthur.RD_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND AutoWinterthur.RD_Ort = 'Winterthur'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugErlen
	ON ZugErlen.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugErlen.RL_Ort = 'Erlen' 

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugZurich
	ON ZugZurich.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugZurich.RL_Ort = 'Zürich HB'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugSanktGallen
	ON ZugSanktGallen.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugSanktGallen.RL_Ort = 'St. Gallen HB'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugBasel
	ON ZugBasel.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugBasel.RL_Ort = 'Basel SBB'

LEFT JOIN __Steuern_2017_ZO_Rail AS ZugWinterthur 
	ON ZugWinterthur.RL_GemeindeNummer = __Steuern_2017.gemeindenummer 
	AND ZugWinterthur.RL_Ort = 'Winterthur'

-- WHERE __Steuern_2017_ZO_Road.RD_UID IS NULL 
WHERE ZugErlen.RL_UID IS NOT NULL 


