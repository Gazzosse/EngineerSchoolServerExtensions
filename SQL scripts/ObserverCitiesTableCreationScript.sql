CREATE TABLE IF NOT EXISTS public."observer_cities" (
    "EmployeeRowID" UUID NOT NULL,
    "CityRowID" UUID NOT NULL,
	PRIMARY KEY ("EmployeeRowID", "CityRowID")
) TABLESPACE pg_default;

CREATE INDEX idx_observer_cities_city_employee 
ON public."observer_cities"("CityRowID", "EmployeeRowID");