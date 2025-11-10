CREATE OR REPLACE FUNCTION public."InsertBulkObserverCities"(
    employee_ids UUID[],
    city_ids UUID[],
    is_observer_flags BOOLEAN[]
)
	RETURNS BIGINT
	LANGUAGE plpgsql
	AS $function$
	DECLARE
	    inserted_count BIGINT;
	BEGIN
	    IF array_length(employee_ids, 1) != array_length(city_ids, 1) 
	       OR array_length(employee_ids, 1) != array_length(is_observer_flags, 1) THEN
	        RAISE EXCEPTION 'Все массивы должны быть одинаковой длины';
	    END IF;
	    
	    INSERT INTO observer_cities ("EmployeeRowID", "CityRowID", "IsObserver")
	    SELECT 
	        UNNEST(employee_ids),
	        UNNEST(city_ids),
	        UNNEST(is_observer_flags);
	    
	    GET DIAGNOSTICS inserted_count = ROW_COUNT;
	    
	    RETURN inserted_count;
	END;
	$function$;