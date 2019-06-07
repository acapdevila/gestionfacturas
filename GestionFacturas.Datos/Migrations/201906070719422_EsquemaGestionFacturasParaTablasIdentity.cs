namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EsquemaGestionFacturasParaTablasIdentity : DbMigration
    {
        public override void Up()
        {
            // Se hace manualmente duplicando las tablas para no eliminar las tablas compartidas por otras app's
            //MoveTable(name: "dbo.AspNetUsers", newSchema: "GestionFacturas");
            //MoveTable(name: "dbo.AspNetUserClaims", newSchema: "GestionFacturas");
            //MoveTable(name: "dbo.AspNetUserLogins", newSchema: "GestionFacturas");
            //MoveTable(name: "dbo.AspNetUserRoles", newSchema: "GestionFacturas");
            //MoveTable(name: "dbo.AspNetRoles", newSchema: "GestionFacturas");
        }
        
        public override void Down()
        {
            MoveTable(name: "GestionFacturas.AspNetRoles", newSchema: "dbo");
            MoveTable(name: "GestionFacturas.AspNetUserRoles", newSchema: "dbo");
            MoveTable(name: "GestionFacturas.AspNetUserLogins", newSchema: "dbo");
            MoveTable(name: "GestionFacturas.AspNetUserClaims", newSchema: "dbo");
            MoveTable(name: "GestionFacturas.AspNetUsers", newSchema: "dbo");
        }
    }
}
