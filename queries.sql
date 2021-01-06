--Part 1
select column_name, data_type from information_schema.columns where table_schema = 'techjobs' and table_name = 'jobs'
    --Id int
    --Name longtext
    --EmployerId int
--Part 2
select Name from employers where location='St. Louis City'
--Part 3
SELECT Name FROM skills
INNER JOIN jobskills on skills.Id = jobskills.SkillId
WHERE jobskills.SkillId IS NOT NULL
ORDER BY skills.Name ASC
