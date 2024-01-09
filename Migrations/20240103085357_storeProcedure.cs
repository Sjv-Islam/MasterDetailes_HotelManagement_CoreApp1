using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterDetailes_HotelManagement_CoreApp.Migrations
{
    /// <inheritdoc />
    public partial class storeProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string SpInsertGuest = @"CREATE or ALTER PROCEDURE dbo.SpInsertGuest
    @GuestName nvarchar(50),
    @Address nvarchar(max) ,
    @ContactNo nvarchar(max) ,
    @GuestPicture nvarchar(max)
AS
INSERT INTO[dbo].[Guests]
           ([GuestName]
           , [Address]
           , [ContactNo]
           , [GuestPicture])
     VALUES
           (@GuestName
           , @Address
           , @ContactNo
           , @GuestPicture)
        return @@identity
GO";

            migrationBuilder.Sql(SpInsertGuest);

            string SpInsertReservation = @"CREATE or ALTER PROCEDURE dbo.SpInsertReservation
    @RoomId int,
    @Check_In_Date datetime2(7), 
	@Check_Out_Date datetime2(7),
	@GuestId int
AS


INSERT INTO [dbo].[Reservations]
           ([RoomId]
           ,[Check_In_Date]
           ,[Check_Out_Date]
           ,[GuestId])
     VALUES
           (@RoomId,
           @Check_In_Date,
           @Check_Out_Date,
           @GuestId)
         return @@Rowcount
GO";

            migrationBuilder.Sql(SpInsertReservation);
            string SpUpdateGuest = @"CREATE or ALTER PROCEDURE dbo.SpUpdateGuest
    @GuestId int,
    @GuestName nvarchar(50),
    @Address nvarchar(max),
	@ContactNo nvarchar(max),
	@GuestPicture nvarchar(max)
AS
   
UPDATE [dbo].[Guests]
   SET [GuestName] = @GuestName,
      [Address] = @Address,
      [ContactNo] = @ContactNo,
      [GuestPicture] = @GuestPicture
 WHERE Id = @GuestId
 delete from Reservations where GuestId = @GuestId

 return @@rowcount
GO";
            migrationBuilder.Sql(SpUpdateGuest);

            string SpDeleteGuest = @"CREATE or ALTER PROCEDURE dbo.SpDeleteGuest 
    @GuestId int
AS
	  delete from Reservations where GuestId = @GuestId
	   delete from [Guests] where Id = @GuestId

	  return @@rowcount
GO";

            migrationBuilder.Sql(SpDeleteGuest);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc SpInsertGuest");
            migrationBuilder.Sql("drop proc SpInsertReservation");
            migrationBuilder.Sql("drop proc SpUpdateGuest");
            migrationBuilder.Sql("drop proc SpDeleteGuest");
        }
    }
}
