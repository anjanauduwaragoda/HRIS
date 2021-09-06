---------------------------------------------------------------------------------------------------------------------
-- CREATE VIEW SCRIPTS
-- © 2014 SwarnaSolutions Ltd
---------------------------------------------------------------------------------------------------------------------

--Pramodya
--2014/06/18 - Created
--2014/09/24 - Added Brach, Cost Center, Profit Center
--2014/09/30 - Added Designation
create or replace view v_employee_search as
(
SELECT e.EMPLOYEE_ID, e.EPF_NO, e.TITLE, e.KNOWN_NAME , e.EMP_NIC, e.EMPLOYEE_STATUS, es.DESCRIPTION , c.COMPANY_ID, c.COMP_NAME, dp.DEPT_ID, dp.DEPT_NAME, dv.DIVISION_ID, dv.DIV_NAME, e.BRANCH_ID, br.BRANCH_NAME, e.COST_CENTER, e.PROFIT_CENTER, e.DESIGNATION_ID, ed.DESIGNATION_NAME
FROM COMPANY c, DEPARTMENT dp, DIVISION dv, EMPLOYEE_STATUS es, EMPLOYEE_DESIGNATION ed, EMPLOYEE e left outer join  COMPANY_BRANCH br on e.BRANCH_ID = br.BRANCH_ID 
WHERE e.COMPANY_ID = c.COMPANY_ID
AND e.DEPT_ID = dp.DEPT_ID
and e.DIVISION_ID = dv.DIVISION_ID
and e.EMPLOYEE_STATUS = es.STATUS_CODE
and e.DESIGNATION_ID = ed.DESIGNATION_ID
order by e.KNOWN_NAME
)


