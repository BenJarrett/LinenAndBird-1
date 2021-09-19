insert into Birds(Type,Color,Size,Name)
values(0,'yellow','small','darcy')

update Birds
Set Color = 'blueberry',
	Size = 'small',
	--Name = Name + ' - ' + cast(o.Price as varchar(10)) + 'SOLD'
	Name = Name + ' - SOLD'
output inserted.*, deleted.* -- because update does both inserting and deleting
from birds b
	/*join orders o 
		on o.BirdId = b.Id*/
--Where Color = 'yellow' -- if no where clause is used then all rows Color will be set

--select * from birds;
--select * from orders;

--update Birds
--Set Color = @color,
--	Name = @name,
--	Type = @type,
--	Size = @size,
----Where Id = @id