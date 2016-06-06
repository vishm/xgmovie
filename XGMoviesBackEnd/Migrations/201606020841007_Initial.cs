namespace XGMoviesBackEnd.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        TheMovideDbOrgId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieCharacters",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PlayedBy_Id = c.Int(),
                        Movie_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Actors", t => t.PlayedBy_Id)
                .ForeignKey("dbo.Movies", t => t.Movie_Id)
                .Index(t => t.PlayedBy_Id)
                .Index(t => t.Movie_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieCharacters", "Movie_Id", "dbo.Movies");
            DropForeignKey("dbo.MovieCharacters", "PlayedBy_Id", "dbo.Actors");
            DropIndex("dbo.MovieCharacters", new[] { "Movie_Id" });
            DropIndex("dbo.MovieCharacters", new[] { "PlayedBy_Id" });
            DropTable("dbo.MovieCharacters");
            DropTable("dbo.Movies");
            DropTable("dbo.Actors");
        }
    }
}
