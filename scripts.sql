create database libcore;

create table users(
UserId int primary key auto_increment,
UserName varchar(255),
UserCpf varchar(11),
UserEmail varchar(255));

DROP PROCEDURE IF EXISTS selectAllUsers;
DELIMITER |
CREATE PROCEDURE selectAllUsers ()
BEGIN
    SELECT * 
    FROM users;
END
|
DELIMITER ;

DROP PROCEDURE IF EXISTS selectUser;
DELIMITER |
CREATE PROCEDURE selectUser (IN uId INT)
BEGIN
    SELECT * 
    FROM users
    WHERE users.UserId = uId;
END
|
DELIMITER ;

DROP PROCEDURE IF EXISTS createUser;
DELIMITER |
CREATE PROCEDURE createUser (IN uName varchar(255), uCpf varchar(11), uEmail varchar(255))
BEGIN
    INSERT INTO users values(0, uName, uCpf, uEmail); 
END
|
DELIMITER ;

DROP PROCEDURE IF EXISTS updateUser;
DELIMITER |
CREATE PROCEDURE updateUser (IN uName varchar(255), uCpf varchar(11), uEmail varchar(255), uId INT)
BEGIN
    UPDATE users
    SET 
		users.UserName = uName,
        users.UserCpf = uCpf,
        users.UserEmail= uEmail
	WHERE
		users.UserId = uId; 
END
|
DELIMITER ;

DROP PROCEDURE IF EXISTS deleteUser;
DELIMITER |
CREATE PROCEDURE deleteUser (IN uId INT)
BEGIN
	DELETE 
    FROM users 
    WHERE users.UserId = uId;
END
|
DELIMITER ;