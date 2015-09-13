namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Clientes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumeroIdentificacionFiscal = c.String(maxLength: 50),
                        NombreOEmpresa = c.String(maxLength: 50),
                        NombreComercial = c.String(maxLength: 50),
                        Direccion = c.String(maxLength: 50),
                        Localidad = c.String(maxLength: 50),
                        Provincia = c.String(maxLength: 50),
                        CodigoPostal = c.String(maxLength: 10),
                        Email = c.String(maxLength: 50),
                        PersonaContacto = c.String(maxLength: 50),
                        ComentarioInterno = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Facturas", "IdComprador");
            AddForeignKey("dbo.Facturas", "IdComprador", "dbo.Clientes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facturas", "IdComprador", "dbo.Clientes");
            DropIndex("dbo.Facturas", new[] { "IdComprador" });
            DropTable("dbo.Clientes");
        }
    }
}
