CREATE TABLE IF NOT EXISTS public."observer_cities" (
    "RowID" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "EmployeeRowID" UUID NOT NULL,
    "CityRowID" UUID NOT NULL,
    "IsObserver" BOOLEAN NOT NULL
) TABLESPACE pg_default;

CREATE INDEX idx_observer_cities_employee_city_observer 
ON public."observer_cities"("EmployeeRowID", "CityRowID", "IsObserver");

CREATE UNIQUE INDEX uq_observer_cities_employee_city 
ON public."observer_cities"("EmployeeRowID", "CityRowID");