SELECT public."InsertBulkEmployees"(
    ARRAY['Тестовая группа 1'],
	ARRAY['Иван'],
	ARRAY['Иванович'],
	ARRAY['Иванов']
) as inserted_count;