SELECT 	Name,
		d.Description
FROM Names
INNER JOIN Descriptor d
WHERE Names.Gender == 'M' AND d.Gender == Names.Gender

ORDER BY
	RANDOM()
LIMIT 1;