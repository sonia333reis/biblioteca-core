create database libcore;

create table users(
UserId int primary key auto_increment,
UserName varchar(255),
UserCpf varchar(11),
UserEmail varchar(255));

create table books(
BookId int primary key auto_increment,
BookName varchar(255),
BookWritter varchar(255),
BookRelease datetime);

create table bookings(
BookingId int primary key auto_increment,
BookingId_BookId int,
BookingId_UserId int, 
FOREIGN KEY (BookingId_BookId) REFERENCES books(BookId),
FOREIGN KEY (BookingId_UserId) REFERENCES users(UserId),
);

USERS
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

BOOKS
	DROP PROCEDURE IF EXISTS selectBook;
	DELIMITER |
	CREATE PROCEDURE selectBook (IN bId INT)
	BEGIN
	    SELECT * 
	    FROM books
	    WHERE books.BookId = bId;
	END
	|
	DELIMITER ;

	DROP PROCEDURE IF EXISTS createBook;
	DELIMITER |
	CREATE PROCEDURE createBook (IN bName varchar(255), bWritter varchar(255), bRelease varchar(255))
	BEGIN
	    INSERT INTO books values(0, bName, bWritter, bRelease); 
	END
	|
	DELIMITER ;

	DROP PROCEDURE IF EXISTS updateBook;
	DELIMITER |
	CREATE PROCEDURE updateBook (IN bName varchar(255), bWritter varchar(255), bRelease varchar(255), bId INT)
	BEGIN
	    UPDATE books
	    SET 
			books.BookName = bName,
	        books.BookWritter = bWritter,
	        books.BookRelease= bRelease
		WHERE
			books.BookId = bId; 
	END
	|
	DELIMITER ;

	DROP PROCEDURE IF EXISTS deleteBook;
	DELIMITER |
	CREATE PROCEDURE deleteBook (IN bId INT)
	BEGIN
		DELETE 
	    FROM books 
	    WHERE books.BookId = bId;
	END
	|
	DELIMITER ;

BOOKINGS
	DROP PROCEDURE IF EXISTS selectAllBookings;
	DELIMITER |
	CREATE PROCEDURE selectAllBookings ()
	BEGIN
	    SELECT * 
	    FROM bookings;
	END
	|
	DELIMITER ;

	DROP PROCEDURE IF EXISTS selectBooking;
	DELIMITER |
	CREATE PROCEDURE selectBooking (IN uId INT)
	BEGIN
	    SELECT * 
	    FROM bookings
	    WHERE bookings.BookingId = uId;
	END
	|
	DELIMITER ;

	DROP PROCEDURE IF EXISTS createBooking;
	DELIMITER |
	CREATE PROCEDURE createBooking (IN bId INT, uId INT)
	BEGIN
	    INSERT INTO bookings values(0, bId, uId); 
	END
	|
	DELIMITER ;

	DROP PROCEDURE IF EXISTS updateBooking;
	DELIMITER |
	CREATE PROCEDURE updateBooking (IN bookingId INT, bId INT, uId INT)
	BEGIN
	    UPDATE bookings
	    SET 
			bookings.BookingId_BookId = bId,
	        bookings.BookingId_UserIdId = uId
		WHERE
			bookings.bookingId = bookingId; 
	END
	|
	DELIMITER ;

	DROP PROCEDURE IF EXISTS deleteBooking;
	DELIMITER |
	CREATE PROCEDURE deleteBooking (IN bId INT)
	BEGIN
		DELETE 
	    FROM bookings 
	    WHERE bookings.bookingId = bId;
	END
	|
	DELIMITER ;