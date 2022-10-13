CREATE TRIGGER UpdateTimeUser
ON ApplicationUser
AFTER UPDATE AS
  UPDATE ApplicationUser
  SET ApplicationUser.UpdatedAt = GETDATE()
  WHERE ApplicationUser.Id IN (SELECT DISTINCT Id FROM Inserted)

-- Do Same Thing To ToDo => Just replace ApplicationUser with ToDo
