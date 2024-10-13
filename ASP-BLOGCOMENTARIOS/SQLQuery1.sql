	use Blog;
	go
	create procedure sp_usuarios
	as begin 
	select  * from Usuario;
	end
	go
	create procedure sp_comentarios
	as begin 
	select top 3 Comentario.id,Comentario.contenido
	,Comentario.fecha,Usuario.nombre
	from comentario left join Usuario
	on Comentario.idUsuario = Usuario.id;
	end
	go
	create procedure sp_registrar
	@Nombre varchar(10),
	@Apellido varchar(10),
	@Edad int
	as begin
	insert into Usuario(nombre,apellido,edad) 
	values (@nombre,@apellido,@edad)
	end
	go
	create procedure sp_comentar
	@contenido varchar(200),
	@date datetime,
	@idUsuario int
	as begin
	insert into Comentario(contenido,fecha,idUsuario)values (@contenido,@date,@idUsuario)
	end

	go
	create procedure sp_comentariosTotales
	as begin 
	select Comentario.id,Comentario.contenido
	,Comentario.fecha,Usuario.nombre
	from comentario left join Usuario
	on Comentario.idUsuario = Usuario.id;
	end