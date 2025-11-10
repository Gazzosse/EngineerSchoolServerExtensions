CREATE OR REPLACE FUNCTION public."observer_cities_role"(val_cardids uuid[])
RETURNS TABLE("CardID" uuid, "Value" uuid, "Type" integer)
AS $function$
BEGIN
	RETURN QUERY
	SELECT
		main_info."InstanceID" AS "CardID",
		observer_cities."EmployeeRowID" AS "Value",
		13 AS "Type"
	FROM public."dvtable_{30eb9b87-822b-4753-9a50-a1825dca1b74}" main_info
	JOIN public."observer_cities" observer_cities
		ON observer_cities."CityRowID" = main_info."Cities"
	WHERE main_info."Kind" = '6cf0521a-a2f2-4595-a65b-3c26f17f60a8'
		AND main_info."InstanceID" = ANY(val_cardids);
END;
$function$
LANGUAGE plpgsql;